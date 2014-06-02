using System;
using GeneratorAsync;

namespace Automaton.Net
{

    internal class DelegateGenerator<TArg> : IGenerator<string, string>
    {
        private readonly Func<TArg, string> _exec;

        private readonly TArg _arg;

        private bool _exhausted;

        public DelegateGenerator(Func<TArg, string> exec, TArg arg)
        {
            _exec = exec;
            _arg = arg;
            _exhausted = false;
        }

        public string Next()
        {
            if (_exhausted)
            {
                throw new GeneratorExhaustedException();
            }
            _exhausted = true;
            return _exec(_arg);
        }

        public string Send(string obj)
        {
            throw new InvalidOperationException();
        }

        public bool TryNext(out string result)
        {
            if (_exhausted)
            {
                result = null;
                return false;
            }
            _exhausted = true;
            result = _exec(_arg);
            return true;
        }

        public bool TrySend(string obj, out string result)
        {
            result = null;
            return false;
        }
    }
}
