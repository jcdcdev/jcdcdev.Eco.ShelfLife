## jcdcdev.Eco.ShelfLife

![Eco Version](https://badgen.net/static/Eco/v0.10.3+/3a93b4) 
[![Latest version on Github](https://badgen.net/github/tag/jcdcdev/jcdcdev.Eco.ShelfLife?color=3a93b4&label=Mod)](https://github.com/jcdcdev/jcdcdev.Eco.ShelfLife/releases/latest)

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

You can use the Server UI or the config file `jcdcdev.Eco.ShelfLife.eco` to make changes.

**You must restart the server after making changes.**

The default values are the same as the vanilla game:

- `Icebox`: 1.2
- `Refrigerator`: 1.4
- `IndustrialRefrigerator`: 1.5
- `StorageSilo`: 1.2
- `PoweredStorageSilo`: 1.5
- `SeedBank`: 4.0 - (Requires jcdcdev.Eco.SeedStorage)
- `WoodenSeedBox`: 1.5 - (Requires jcdcdev.Eco.SeedStorage)

You can change these values to your needs, but keep in mind that higher values will make the items last longer and lower values will make them spoil faster.

You will notice that different storage containers will have different effects on the shelf life of the items stored in them. You can check the remaining shelf life of an item by hovering over it in your inventory or storage UI.