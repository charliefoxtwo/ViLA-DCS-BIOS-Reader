# ViLA DCS-BIOS Reader Plugin

[![.NET 5 CI build](https://github.com/charliefoxtwo/ViLA-DCS-BIOS-Reader/actions/workflows/ci-build.yml/badge.svg?branch=develop)](https://github.com/charliefoxtwo/ViLA-DCS-BIOS-Reader/actions/workflows/ci-build.yml)
![GitHub](https://img.shields.io/github/license/charliefoxtwo/ViLA-DCS-BIOS-Reader?style=flat-square)
![Discord](https://img.shields.io/discord/840762843917582347?style=flat-square)

DCS-BIOS Reader reads integer output data from DCS-BIOS and sends the read values the an id matching the DCS-BIOS id (not including the aircraft name). It is currently hardcoded to use DCS Open Beta, but will be expanded with configuration options in the future.


## Installation

To install the plugin, download the latest version from Releases and unzip it into the `Plugins/` folder of your ViLA installation (so you have `ViLA/Plugins/DcsBiosReader/<dll files>`.) ViLA should auto-detect it on next run.


## Configuring ViLA

When setting up your actions in ViLA, it's recommended to use the DCS-BIOS reference tool as a, well, reference. It will tell you the ids of various components, and their possible values. An example configuration block for the F/A-18C's master caution light might look something like this:
```json
{
    "color": "ff8000",
    "trigger": {
        "biosCode": "MASTER_CAUTION_LT",
        "value": 1,
        "comparator": "EqualTo"
    },
    "target": {
        "ledNumber": 1,
        "boardType": "OnBoard"
    }
}
```


## Roadmap

 - Unit tests
 - Configuration


## Acknowledgements

 - [readme tools](https://readme.so)
 - [badges](https://shields.io)