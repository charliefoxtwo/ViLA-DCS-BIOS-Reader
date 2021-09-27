using System.Collections.Generic;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable MemberCanBePrivate.Global

namespace ViLA.Extensions.DcsBiosReader
{
    public class PluginConfiguration
    {
        public string? AliasesFileName { get; set; } = "AircraftAliases.json";
        public HashSet<string> ConfigLocations { get; set; } = new() { "%userprofile%/Saved Games/DCS.openbeta/Scripts/DCS-BIOS/doc/json/" };

        public EndpointConfiguration Export { get; set; } = new()
        {
            IpAddress = "239.255.50.10",
            SendPort = 7778,
            ReceivePort = 5010,
        };
    }
}