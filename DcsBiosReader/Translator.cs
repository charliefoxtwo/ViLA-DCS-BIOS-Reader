using System;
using DcsBios.Communicator;
using Microsoft.Extensions.Logging;

namespace ViLA.Extensions.DcsBiosReader
{
    public class Translator : IBiosTranslator
    {
        private readonly ILogger<Translator> _log;

        private readonly Action<string, int>? _onReceive;

        public Translator(ILogger<Translator> log, Action<string, int>? onReceive)
        {
            _onReceive = onReceive;
            _log = log;
        }

        public void FromBios<T>(string biosCode, T data)
        {
            if (data is not int intData) return; // we can only handle int data currently
            _log.LogDebug("Got data {{{Data}}} from biosCode {{{BiosCode}}}", intData, biosCode);
            _onReceive?.Invoke(biosCode, intData);
        }
    }
}