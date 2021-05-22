using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DcsBios.Communicator;
using DcsBios.Communicator.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ViLA.Extensions.DcsBiosReader
{
    public class BiosSubscriberPlugin : PluginBase.PluginBase
    {
        public const string ConfigPath = "config.json";

        public override async Task<bool> Start()
        {
            var log = LoggerFactory.CreateLogger<BiosSubscriberPlugin>();

            var pluginConfig = await GetConfiguration();

            var client = new BiosUdpClient(pluginConfig.Export!.Ip, pluginConfig.Export!.SendPort, pluginConfig.Export!.ReceivePort, LoggerFactory.CreateLogger<BiosUdpClient>());
            client.OpenConnection();

            var biosListener = new BiosListener(client, new Translator(LoggerFactory.CreateLogger<Translator>(), SendData, SendString), LoggerFactory.CreateLogger<BiosListener>());

            var configsLoaded = 0;
            foreach (var config in await AircraftBiosConfiguration.AllConfigurations(pluginConfig.ConfigLocations.ToArray()))
            {
                biosListener.RegisterConfiguration(config);
                configsLoaded++;
            }

            if (configsLoaded == 0)
            {
                var configPaths = string.Join(", ", pluginConfig.ConfigLocations);
                log.LogWarning("No configuration files found at {ConfigPaths} - check config.json", configPaths);
            }

            log.LogInformation("Starting bios listener...");

            biosListener.Start();
            return true;
        }

        private static async Task<PluginConfiguration> GetConfiguration()
        {
            PluginConfiguration? pluginConfig = null;
            var getAndWriteDefaultConfig = true;

            if (File.Exists(ConfigPath))
            {
                getAndWriteDefaultConfig = false;

                var configString = await File.ReadAllTextAsync(ConfigPath);
                pluginConfig = JsonConvert.DeserializeObject<PluginConfiguration>(configString);

                if (pluginConfig is null)
                {
                    getAndWriteDefaultConfig = true;
                }
                else if (pluginConfig.Export is null)
                {
                    pluginConfig.Export = PluginConfiguration.Default().Export;
                }
            }

            if (getAndWriteDefaultConfig)
            {
                pluginConfig = PluginConfiguration.Default();
                await File.WriteAllTextAsync(ConfigPath, JsonConvert.SerializeObject(pluginConfig));
            }

            return pluginConfig!;
        }
    }
}