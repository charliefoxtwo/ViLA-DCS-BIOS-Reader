using System.Net;
using System.Threading.Tasks;
using DcsBios.Communicator;
using DcsBios.Communicator.Configuration;
using Microsoft.Extensions.Logging;

namespace ViLA.Extensions.DcsBiosReader
{
    public class BiosSubscriberPlugin : PluginBase.PluginBase
    {
        public override async Task<bool> Start()
        {
            var log = LoggerFactory.CreateLogger<BiosSubscriberPlugin>();

            var client = new BiosUdpClient(IPAddress.Parse("239.255.50.10"), 7778, 5010, LoggerFactory.CreateLogger<BiosUdpClient>());
            client.OpenConnection();

            var biosListener = new BiosListener(client, new Translator(LoggerFactory.CreateLogger<Translator>(), SendData), LoggerFactory.CreateLogger<BiosListener>());

            var configLocation = "%userprofile%/Saved Games/DCS.openbeta/Scripts/DCS-BIOS/doc/json/";
            foreach (var config in await AircraftBiosConfiguration.AllConfigurations(configLocation))
            {
                biosListener.RegisterConfiguration(config);
            }

            log.LogInformation("Starting bios listener...");

            biosListener.Start();
            return true;
        }
    }
}