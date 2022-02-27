using System;
using DcsBios.Communicator;
using Microsoft.Extensions.Logging;

namespace ViLA.Extensions.DcsBiosReader;

public class Translator : IBiosTranslator
{
    private readonly ILogger<Translator> _log;

    private readonly Action<string, dynamic>? _onDataReceive;

    private readonly Action _clearStateAction;

    private string _lastAircraftName = "";

    public Translator(ILogger<Translator> log, Action<string, dynamic>? onDataReceive, Action clearStateAction)
    {
        _onDataReceive = onDataReceive;
        _log = log;
        _clearStateAction = clearStateAction;
    }

    public void FromBios<T>(string biosCode, T data)
    {
        _log.LogTrace("Got data {{{Data}}} from biosCode {{{BiosCode}}}", data, biosCode);

        if (biosCode == BiosListener.AircraftNameBiosCode && data is string s && s != _lastAircraftName)
        {
            _lastAircraftName = s;
            _clearStateAction();
        }

        _onDataReceive?.Invoke(biosCode, data);
    }
}
