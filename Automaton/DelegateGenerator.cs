using System;
using GeneratorAsync;

namespace Automaton.Net
{

    internal class DelegateGenerator : IGenerator<object, string>
    {
        private readonly Func<string> _exec;

        private bool _exhausted;

        public DelegateGenerator(Func<string> exec)
        {
            _exec = exec;
            _exhausted = false;
        }

        public string Next()
        {
            if (_exhausted)
            {
                throw new GeneratorExhaustedException();
            }
            _exhausted = true;
            return _exec();
        }

        public string Send(object obj)
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
            result = _exec();
            return true;
        }

        public bool TrySend(object obj, out string result)
        {
            result = null;
            return false;
        }
    }
}
