using System;
using DcsBios.Communicator;
using Microsoft.Extensions.Logging;

namespace ViLA.Extensions.DcsBiosReader
{
    public class Translator : IBiosTranslator
    {
        private readonly ILogger<Translator> _log;

        private readonly Action<string, int>? _onIntReceive;
        private readonly Action<string, string>? _onStringReceive;

        public Translator(ILogger<Translator> log, Action<string, int>? onIntReceive, Action<string, string>? onStringReceive)
        {
            _onIntReceive = onIntReceive;
            _onStringReceive = onStringReceive;
            _log = log;
        }

        public void FromBios<T>(string biosCode, T data)
        {
            switch (data)
            {
                case int intData:
                    _log.LogDebug("Got data {{{Data}}} from biosCode {{{BiosCode}}}", intData, biosCode);
                    _onIntReceive?.Invoke(biosCode, intData);
                    break;
                case string stringData:
                    _log.LogDebug("Got data {{{Data}}} from biosCode {{{BiosCode}}}", stringData, biosCode);
                    _onStringReceive?.Invoke(biosCode, stringData);
                    break;
            }
        }
    }
}