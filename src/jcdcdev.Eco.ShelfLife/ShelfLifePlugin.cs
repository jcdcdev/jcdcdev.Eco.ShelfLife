using Eco.Core;
using Eco.Core.FileStorage;
using Eco.Core.Plugins;
using Eco.Core.Utils;
using jcdcdev.Eco.Core;
using jcdcdev.Eco.Core.Extensions;
using jcdcdev.Eco.Core.Logging;

namespace jcdcdev.Eco.ShelfLife;

public class ShelfLifePlugin : PluginBase<ShelfLifeConfig>
{
    private readonly IFileStorage _fileSystem = PluginManager.Controller.BaseStorage;

    private readonly string[] _objects =
    {
        "IceboxObject",
        "IndustrialRefrigeratorObject",
        "RefrigeratorObject",
        "StorageSiloObject",
        "PoweredStorageSiloObject"
    };

    private IFileStorage? _generatedFilesPath;

    protected override void InitializeMod(TimedTask timer)
    {
        EnsureDataDirectory()
            .GetAwaiter()
            .GetResult();
    }

    private async Task EnsureDataDirectory()
    {
        _generatedFilesPath = await _fileSystem.GetOrCreateDirectoryAsync("Mods/UserCode/jcdcdev.Eco.ShelfLife/UserCode/AutoGen/WorldObjects");

        // 🧹 TODO remove in future version
        await DeleteLegacyDirectory();
    }

    private async Task DeleteLegacyDirectory()
    {
        var legacyPath = "Mods/UserCode/jcdcdev.Eco.ShelfLife/Generated";
        if (await _fileSystem.DirectoryExistsAsync(legacyPath))
        {
            Logger.WriteLine($"Deleting legacy directory {legacyPath}", ConsoleColor.DarkYellow);
            await _fileSystem.DeleteDirectoryAsync(legacyPath);
        }
    }

    protected override void PluginsInitialized()
    {
        var objects = _objects.ToList();

        var moddedObjects = new Dictionary<string, string[]>
        {
            {
                "SeedStoragePlugin",
                new[]
                {
                    "WoodenSeedBoxObject",
                    "SeedBankObject"
                }
            },
            // Add modded objects here to enable shelf life configuration
            // MyMod
            // {
            //     "MyModName",
            //     new[]
            //     {
            //         "MyModdedObject"
            //     }
            // }
        };

        foreach (var (pluginName, objectNames) in moddedObjects)
        {
            if (PluginManager.Controller.Plugins.Any(x => x.GetType().Name == pluginName))
            {
                objects.AddRange(objectNames);
            }
        }

        var updated = GenerateClasses(objects);
        if (!updated)
        {
            return;
        }

        var builder = ColorLogBuilder.Create();
        builder.AppendLine("RESTART REQUIRED!", ConsoleColor.Red);
        builder.AppendLine("The server will now shutdown. Please manually start the server again.", ConsoleColor.DarkYellow);
        builder.AppendLine("The server has not crashed!", ConsoleColor.DarkGreen);
        builder.Log(ModName);
        Thread.Sleep(TimeSpan.FromSeconds(3));
        PluginManager.Controller.FireShutdown(ApplicationExitCodes.ApplicationRestart);
    }

    protected override void OnConfigChanged(string propertyChanged)
    {
        Logger.WriteLine(propertyChanged, ConsoleColor.Magenta);
        var objects = new[] { propertyChanged.EnsureEndsWith("Object") };
        var updated = GenerateClasses(objects);
        if (!updated)
        {
            return;
        }

        var builder = ColorLogBuilder.Create();
        builder.AppendLine("RESTART REQUIRED!", ConsoleColor.Red);
        builder.AppendLine("You MUST restart the server for changes to take effect", ConsoleColor.DarkYellow);
        builder.Log(ModName);
    }

    private bool GenerateClasses(IEnumerable<string> objects)
    {
        Logger.WriteLine("Applying Shelf Life Modifiers", ConsoleColor.DarkGray);

        var updated = false;
        foreach (var objectName in objects)
        {
            var shelfLifeValue = Config.GetShelfLife(objectName);
            LoggerExtensions.LogShelfLife(objectName, shelfLifeValue);

            if (GenerateClassIfNeeded(objectName, shelfLifeValue))
            {
                updated = true;
            }
        }

        return updated;
    }

    private bool GenerateClassIfNeeded(string objectName, float shelfLifeValue)
    {
        if (_generatedFilesPath == null)
        {
            throw new Exception();
        }

        var code =
            $$"""
              using Eco.Gameplay.Components.Storage;
              using Eco.Shared.Utils;
              using Eco.Mods.TechTree;
              using Eco.Shared.Localization;
              using Eco.Shared.Logging;
              
              namespace Eco.Mods.TechTree
              {
                  public partial class {{objectName}}
                  {
                      partial void ModsPostInitialize()
                      {
                          var storage = this.GetComponent<PublicStorageComponent>();
                          storage.ShelfLifeMultiplier = {{shelfLifeValue:F}}f;
                          Log.Write(new LocString("Shelf Life: {{objectName}} = {{shelfLifeValue:F}}"));
                      }
                  }
              }
              """;

        var filePath = Path.Combine(_generatedFilesPath.QualifiedName, $"{objectName}.cs");
        if (File.Exists(filePath))
        {
            var existing = File.ReadAllText(filePath);
            if (existing == code)
            {
                return false;
            }
        }

        File.WriteAllText(filePath, code);

        return true;
    }
}