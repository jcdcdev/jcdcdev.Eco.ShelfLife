## jcdcdev.Eco.ShelfLife

![Eco Version](https://badgen.net/static/Eco/v0.10.2.1+/3a93b4)
[![Latest version on Github](https://badgen.net/github/tag/jcdcdev/jcdcdev.Eco.ShelfLife?color=3a93b4&label=Mod)](https://github.com/jcdcdev/jcdcdev.Eco.ShelfLife/releases/latest)

Control the shelf life multiplier of your storage objects in Eco.

## Install Steps

- Download the latest version of this mod from [mod.io](https://mod.io/g/eco/m/jcdcdevecoshelflife)
- Download the latest version of `jcdcdev.Eco.Core` from [mod.io](https://mod.io/g/eco/m/jcdcdevecocore)
- Extract each zip
- Go to your Server's root folder
- Copy the extracted content to `./Mods/UserCode`
- Start the server
- On first run the server will shutdown
    - This is expected behaviour ✅
    - Please restart your server

## Configuration

You can configure the shelf life multiplier through the Server UI or directly in the config file.

**You must restart the server after making changes.**

The default values are the same as the vanilla game:

- `Icebox`: 1.6
- `Refrigerator`: 1.9
- `IndustrialRefrigerator`: 2.4
- `StorageSilo`: 1.6
- `PoweredStorageSilo`: 2.4
- `SeedBank`: 4.8 - (Requires [jcdcdev.Eco.SeedStorage](https://mod.io/g/eco/m/jcdcdevecoseedstorage))
- `WoodenSeedBox`: 1.6 - (Requires [jcdcdev.Eco.SeedStorage](https://mod.io/g/eco/m/jcdcdevecoseedstorage))

### Server UI

You can change these values to your needs, but keep in mind that higher values will make the items last longer and lower values will make them spoil faster.

![Server UI](https://raw.githubusercontent.com/jcdcdev/jcdcdev.Eco.ShelfLife/main/docs/screenshots/2-config.png)

### Config File

You can find the config file at `./Configs/jcdcdev.Eco.ShelfLife.eco`.

The default values are below:

```json
{
  "Icebox": 2.0,
  "Refrigerator": 2.5,
  "IndustrialRefrigerator": 3.0,
  "StorageSilo": 2.0,
  "PoweredStorageSilo": 3.0,
  "SeedBank": 4.0,
  "WoodenSeedBox": 2.0
}
```

You will notice that different storage containers will have different effects on the shelf life of the items stored in them. You can check the remaining shelf
life of an item by hovering over it in your inventory or storage UI.