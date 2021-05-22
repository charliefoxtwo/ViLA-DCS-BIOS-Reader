using System.Collections.Generic;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable MemberCanBePrivate.Global

namespace ViLA.Extensions.DcsBiosReader
{
    public class PluginConfiguration
    {
        public HashSet<string> ConfigLocations { get; set; } = null!;
        public EndpointConfiguration? Export { get; set; }

        public static PluginConfiguration Default()
        {
            return new()
            {
                ConfigLocations = new HashSet<string> { "%userprofile%/Saved Games/DCS.openbeta/Scripts/DCS-BIOS/doc/json/" },
                Export = new EndpointConfiguration
                {
                    IpAddress = "239.255.50.10",
                    SendPort = 7778,
                    ReceivePort = 5010,
                }
            };
        }
    }
}