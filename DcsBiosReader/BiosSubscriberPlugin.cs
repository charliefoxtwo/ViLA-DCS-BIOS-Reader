using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DcsBios.Communicator;
using DcsBios.Communicator.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ViLA.Extensions.DcsBiosReader;

public class BiosSubscriberPlugin : PluginBase.PluginBase, IDisposable
{
    /// <summary>
    /// ConfigPath is relative to ViLA, *not* to this dll
    /// </summary>
    public const string ConfigPath = "Plugins/DcsBiosReader/config.json";

    private BiosListener? _biosListener;
    private BiosUdpClient? _client;

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

        _client = new BiosUdpClient(pluginConfig.Export.Ip, pluginConfig.Export.SendPort, pluginConfig.Export.ReceivePort, LoggerFactory.CreateLogger<BiosUdpClient>());
        _client.OpenConnection();

        _biosListener = new BiosListener(_client, new Translator(LoggerFactory.CreateLogger<Translator>(), Send), LoggerFactory.CreateLogger<BiosListener>());

        var configsLoaded = 0;
        foreach (var config in await AircraftBiosConfiguration.AllConfigurations(pluginConfig.AliasesFileName, LoggerFactory.CreateLogger<AircraftBiosConfiguration>(), pluginConfig.ConfigLocations.ToArray()))
        {
            _biosListener.RegisterConfiguration(config);
            configsLoaded++;
        }

        if (configsLoaded == 0)
        {
            var configPaths = string.Join(", ", pluginConfig.ConfigLocations);
            log.LogWarning("No configuration files found at {ConfigPaths} - check config.json", configPaths);
        }

        log.LogInformation("Starting bios listener...");

        _biosListener.Start();
        return true;
    }

    public override Task Stop()
    {
        Dispose();
        return Task.CompletedTask;
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

        return pluginConfig;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _biosListener?.Dispose();
        _client?.Dispose();
    }
}
