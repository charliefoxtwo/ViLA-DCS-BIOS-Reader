# ViLA DCS-BIOS Reader Plugin

[![.NET 5 CI build](https://github.com/charliefoxtwo/ViLA-DCS-BIOS-Reader/actions/workflows/ci-build.yml/badge.svg?branch=develop)](https://github.com/charliefoxtwo/ViLA-DCS-BIOS-Reader/actions/workflows/ci-build.yml)
[![GitHub](https://img.shields.io/github/license/charliefoxtwo/ViLA-DCS-BIOS-Reader?style=flat-square)](LICENSE)
[![Discord](https://img.shields.io/discord/840762843917582347?style=flat-square)](https://discord.gg/rWAF3AdsKT)

DCS-BIOS Reader reads integer output data from DCS-BIOS and sends the read values the an id matching the DCS-BIOS id (not including the aircraft name). It is currently hardcoded to use DCS Open Beta, but will be expanded with configuration options in the future.

## Prerequisites

This plugin requires you to have DCS-BIOS installed. Any version should work but the [Flightpanels fork](https://github.com/DCSFlightpanels/dcs-bios) is recommended.

For installation instructions, follow [this wiki](https://github.com/DCSFlightpanels/DCSFlightpanels/wiki/Installation) up until **Installation of DCSFP** (Flightpanels is **not** required for this plugin).

## Installation

To install the plugin, download the latest version from Releases and unzip it into the `Plugins/` folder of your ViLA installation (so you have `ViLA/Plugins/DcsBiosReader/<dll files>`.) ViLA should auto-detect it on next run.

For a quick start, copy the `DCSBiosReaderConfiguration` folder over to ViLA's configuration folder.

## Configuring ViLA

When setting up your actions in ViLA, it's recommended to use the DCS-BIOS reference tool as a, well, reference. It will tell you the ids of various components, and their possible values. An example configuration block for the F/A-18C's master caution light might look something like this:
```json
{
    "color": "ff8000",
    "trigger": {
        "biosCode": "MASTER_CAUTION_LT",
        "value": 1,
        "comparator": "EqualTo"
    }
}
```

## Configuring the DCS-BIOS Reader plugin
After running ViLA with this plugin for the first time, a `config.json` file will be automatically generated in the folder you put the plugin. You can edit the values in `configLocations` depending on your setup. For example, if you're not running open beta, you're running on steam, or you are using a different version of DCS-BIOS that just stores the config files somewhere else, you might need to change this value.

If you're using Steam or the stable version of DCS, change this value to:
 - `"configLocations" = [ "%userprofile%/Saved Games/DCS/Scripts/DCS-BIOS/doc/json/" ]` 

If you're using the original bios (not the recommended FlightPanels fork), try
 - `"configLocations" = [ "%appdata%/DCS-BIOS/control-reference/json/" ]`

When in doubt, just check these folders and see which one is full of json files!

## Roadmap

 - Unit tests


## Acknowledgements

 - [readme tools](https://readme.so)
 - [badges](https://shields.io)