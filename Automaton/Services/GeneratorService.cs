using System;
using System.Threading.Tasks;
using GeneratorAsync;

namespace Automaton.Net.Services
{
    public class GeneratorService<TArg> : IService
    {
        public string Name { get; private set; }

        private readonly Func<IYield<string, string>, TArg, Task> _genInit;

        private IDisposable _conversation;

        public GeneratorService(string name, Func<IYield<string, string>, TArg, Task> exec)
        {
            Name = name;
            _genInit = exec;
        }

        public IGenerator<string, string> StartConversation(object args)
        {
            var gen = new Generator<string, string, TArg>(_genInit, (TArg)args);
            _conversation = gen;
            return gen;
        }
    }
}
