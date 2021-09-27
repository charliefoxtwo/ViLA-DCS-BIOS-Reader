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
        /// <summary>
        /// ConfigPath is relative to ViLA, *not* to this dll
        /// </summary>
        public const string ConfigPath = "Plugins/DcsBiosReader/config.json";

        public override async Task<bool> Start()
        {
            var log = LoggerFactory.CreateLogger<BiosSubscriberPlugin>();

            PluginConfiguration pluginConfig;

            try
            {
                pluginConfig = await GetConfiguration();
            }
            catch (JsonSerializationException ex)
            {
                log.LogError("Encountered error while loading configuration for DcsBiosReader. Skipping...");
                log.LogDebug(ex, "Exception:");
                return false;
            }

            var client = new BiosUdpClient(pluginConfig.Export!.Ip, pluginConfig.Export!.SendPort, pluginConfig.Export!.ReceivePort, LoggerFactory.CreateLogger<BiosUdpClient>());
            client.OpenConnection();

            var biosListener = new BiosListener(client, new Translator(LoggerFactory.CreateLogger<Translator>(), SendData, SendString), LoggerFactory.CreateLogger<BiosListener>());

            var configsLoaded = 0;
            foreach (var config in await AircraftBiosConfiguration.AllConfigurations(pluginConfig.AliasesFileName, LoggerFactory.CreateLogger<AircraftBiosConfiguration>(), pluginConfig.ConfigLocations.ToArray()))
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

        /// <summary>
        /// Gets the existing plugin configuration, or writes a new configuration file.
        /// </summary>
        /// <exception cref="JsonSerializationException">Thrown when deserializing the plugin config into the expected POCO fails.</exception>
        private static async Task<PluginConfiguration> GetConfiguration()
        {
            if (File.Exists(ConfigPath))
            {
                var configString = await File.ReadAllTextAsync(ConfigPath);
                return JsonConvert.DeserializeObject<PluginConfiguration>(configString) ?? throw new JsonSerializationException("Result was null");
            }

            var pluginConfig = new PluginConfiguration();
            await File.WriteAllTextAsync(ConfigPath, JsonConvert.SerializeObject(pluginConfig));

            return pluginConfig!;
        }
    }
}