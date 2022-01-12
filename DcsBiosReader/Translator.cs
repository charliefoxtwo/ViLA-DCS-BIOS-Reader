using System;
using DcsBios.Communicator;
using Microsoft.Extensions.Logging;

namespace ViLA.Extensions.DcsBiosReader;

public class Translator : IBiosTranslator
{
    private readonly ILogger<Translator> _log;

    private readonly Action<string, dynamic>? _onDataReceive;

    public Translator(ILogger<Translator> log, Action<string, dynamic>? onDataReceive)
    {
        _onDataReceive = onDataReceive;
        _log = log;
    }

    public void FromBios<T>(string biosCode, T data)
    {
        _log.LogTrace("Got data {{{Data}}} from biosCode {{{BiosCode}}}", data, biosCode);
        _onDataReceive?.Invoke(biosCode, data);
    }
}
