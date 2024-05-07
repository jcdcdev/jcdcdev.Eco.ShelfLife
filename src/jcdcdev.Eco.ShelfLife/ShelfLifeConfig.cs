using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jcdcdev.Eco.ShelfLife;

public class ShelfLifeConfig
{
    [Description("The shelf life multiplier for Icebox")]
    [Category("Base Storage")]
    [Range(1.0f, int.MaxValue)]
    [DefaultValue(1.6f)]
    public float Icebox { get; set; } = 1.6f;

    [Description("The shelf life multiplier for Refrigerator")]
    [Category("Base Storage")]
    [Range(1, int.MaxValue)]
    [DefaultValue(1.9f)]
    public float Refrigerator { get; set; } = 1.9f;

    [Description("The shelf life multiplier for IndustrialRefrigerator")]
    [Category("Base Storage")]
    [Range(1.0f, int.MaxValue)]
    [DefaultValue(2.4f)]
    public float IndustrialRefrigerator { get; set; } = 2.4f;

    [Description("The shelf life multiplier for StorageSilo")]
    [Category("Base Storage")]
    [Range(1.0f, int.MaxValue)]
    [DefaultValue(1.6f)]
    public float StorageSilo { get; set; } = 1.6f;

    [Description("The shelf life multiplier for PoweredStorageSilo")]
    [Category("Base Storage")]
    [Range(1.0f, int.MaxValue)]
    [DefaultValue(2.4f)]
    public float PoweredStorageSilo { get; set; } = 2.4f;

    [Description("(Requires jcdcdev.Eco.SeedStorage) The shelf life multiplier for SeedBank")]
    [Category("Seed Storage")]
    [Range(1.0f, int.MaxValue)]
    [DefaultValue(4.8f)]
    public float SeedBank { get; set; } = 4.8f;

    [Description("(Requires jcdcdev.Eco.SeedStorage) The shelf life multiplier for SeedBox")]
    [Category("Seed Storage")]
    [Range(1.0f, int.MaxValue)]
    [DefaultValue(1.6f)]
    public float WoodenSeedBox { get; set; } = 1.6f;

    public float GetShelfLife(string objectName)
    {
        return objectName switch
        {
            "PoweredStorageSiloObject" => PoweredStorageSilo,
            "StorageSiloObject" => StorageSilo,
            "IndustrialRefrigeratorObject" => IndustrialRefrigerator,
            "RefrigeratorObject" => Refrigerator,
            "IceboxObject" => Icebox,
            "WoodenSeedBoxObject" => WoodenSeedBox,
            "SeedBankObject" => SeedBank,
            _ => throw new ArgumentOutOfRangeException(nameof(objectName), objectName, null)
        };
    }
}