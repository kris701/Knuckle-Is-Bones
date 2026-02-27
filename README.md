<p align="center">
    <img src="https://github.com/user-attachments/assets/c3a19ae7-e8b4-45c6-817a-139f5ea1e889" width="600" height="200" />
</p>

[![Build and Publish](https://github.com/kris701/Knuckle-Is-Bones/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/kris701/Uni.API/actions/workflows/dotnet-desktop.yml)
[![General badge](https://img.shields.io/badge/steam-store-blue.svg)](https://store.steampowered.com/app/4456210/Knuckle_Is_Bones/)
![GitHub last commit (branch)](https://img.shields.io/github/last-commit/kris701/Knuckle-Is-Bones/main)
![GitHub commit activity (branch)](https://img.shields.io/github/commit-activity/m/kris701/Knuckle-Is-Bones)
![Static Badge](https://img.shields.io/badge/Platform-Windows-blue)
![Static Badge](https://img.shields.io/badge/Platform-Linux-blue)
![Static Badge](https://img.shields.io/badge/Framework-dotnet--10.0-green)
![GitHub Release](https://img.shields.io/github/v/release/kris701/Knuckle-Is-Bones)

# Knuckle Is Bones
This is a simple fan game version of the minigame Knucklebones from Cult of the Lamb, with the somewhat visuals of Baba is You.
The game is all about ***Dice***, and getting the biggest combos possible to beat your opponents!

The game currenctly contains:
* 6 opponents
* 6 dice
* 8 board
* 18 shop items

The game has a shop system, where the the more points you gain by winning against opponents, the better dice er larger boards you get get!

This game is also fully moddable! You can add boards, dice, opponents, shop items, change textures and much more.

This game is still Work in Progress and i appreciate any input you can give :)

![gif](https://github.com/user-attachments/assets/11ac6359-60d8-43dc-af79-ca9156a84abd)

## Modding
This game supports modding of both visual content and game content.
At the root of the game, there is a folder called "Mods" (if this folder does not exist, simply create it).
This folder is where you put your mods.

Each mod has its own folder in the "Mods" folder, a resource definition, and whatever mod content you want.
This essentially creates a folder structure as:
* `Mods/`
    * `YourModName/`
        * `definition.json`
        * `Dice/`
        * `Boards/`
        * `Opponents/`
        * `Shop/`
        * `Textures/`
        * `TextureSets/`
        * `Songs/`
        * `Fonts/`
        * `SoundEffects/`

The resource definition gives some basic information on what the mod is, and is structured as follows:
```json
{
    "ID":"e2587f49-23df-4875-b91b-44c31855aa32"
    "Name":"Some Name",
    "Description":"Description text",
    "Version":"v0.0.0"
}
```
You can then add resources under each folder.
The `Dice`, `Boards`, `Opponents` and `Shop` folder is for game content, the rest is for visual content.

As an example, if you wanted to add a new dice, simply create a JSON file under the `Dice/` folder that looks like this:
```json
{
  "ID": "963dcef5-7afb-4083-aaaa-a4fbddf85a08",
  "Name": "Your Dice",
  "Description": "Modded dice",
  "Options": [1, 2, 3],
  "Value": 0,
  "IsPurchasable": false
}
```
When you start the game, this dice will be loaded in and will be shown as an option when
trying to start a new game.

Visual resources generally consists of two things, a definition file and the resource file.
As an example, if you where to add a new texture, you have to structure the contents as follows:
* `Mods/`
    * `YourModName/`
        * `definition.json`
        * `Textures/`
            * `Content/`
                * `texture1.png`
                * `texture2.png`
            * `textures.json`

Where the JSON file represents meta data for the textures in the `Content`, and is structured as follows:
```json
[
    {
	    "ID":"f86c92d7-3611-4b61-b381-67542b4ed2c0",
	    "Content":"texture1"
    },
    {
	    "ID":"b9c03de5-8e40-4dbf-9885-31f1d7972451",
	    "Content":"texture2"
    }
]
```

> [!IMPORTANT]
> You can override existing resources!
> E.g. if you make a texture set with the id `af1a8619-0867-44ce-89ab-e2d42912ba44`, you will override the default "title" on the main menu!

## Music
I made the music using this website: https://www.beepbox.co

## Sound effects
I made the sound effects using this website: https://sfxr.me/
