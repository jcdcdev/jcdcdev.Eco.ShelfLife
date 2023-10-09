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
        _generatedFilesPath = PluginManager.Controller.BaseStorage
            .GetOrCreateDirectoryAsync("Mods/UserCode/jcdcdev.Eco.ShelfLife/Generated")
            .GetAwaiter()
            .GetResult();
    }

    protected override void PluginsInitialized()
    {
        var objects = _objects.ToList();
        if (PluginManager.Controller.Plugins.Any(x => x.GetType().Name == "SeedStoragePlugin"))
        {
            objects.Add("WoodenSeedBoxObject");
            objects.Add("SeedBankObject");
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
        builder.AppendLine("You MUST restart the server changes to take effect", ConsoleColor.DarkYellow);
        builder.Log(ModName);
    }

    private bool GenerateClasses(IEnumerable<string> objects)
    {
        var updated = false;
        var builder = ColorLogBuilder.Create();
        builder.AppendLine("Applying Shelf Life Modifiers", ConsoleColor.DarkYellow);
        builder.Log(ModName);

        foreach (var objectName in objects)
        {
            var shelfLifeValue = Config.GetShelfLife(objectName);
            builder.Append($"{objectName}", ConsoleColor.Cyan);
            builder.Append(" => ", ConsoleColor.DarkGray);
            if (GenerateClassIfNeeded(objectName, shelfLifeValue))
            {
                updated = true;
                builder.Append("ShelfLife", ConsoleColor.DarkYellow);
                builder.Append(" = ", ConsoleColor.DarkGray);
                builder.Append($"ShelfLife = {shelfLifeValue:F}", ConsoleColor.DarkGreen);
            }
            else
            {
                builder.Append("No changes detected.", ConsoleColor.DarkGray);
            }

            builder.Log();
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
              using Eco.Gameplay.Components;
              using Eco.Shared.Localization;
              using Eco.Shared.Utils;
              using Eco.Mods.TechTree;
              namespace Eco.Mods.TechTree {
                  public partial class {{objectName}}
                  {
                      partial void ModsPostInitialize()
                      {
                            var storage = this.GetComponent<PublicStorageComponent>();
                            storage.ShelfLifeMultiplier = {{shelfLifeValue:F}}f;
                            Log.WriteLine(new LocString($"{{objectName}}: ShelfLife = {{shelfLifeValue:F}}"));
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