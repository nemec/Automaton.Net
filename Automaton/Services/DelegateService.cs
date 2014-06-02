using System;
using GeneratorAsync;

namespace Automaton.Net.Services
{
    public class DelegateService : IService
    {
        public string Name { get; private set; }

        private readonly Func<string> _exec; 

        public DelegateService(string name, Func<string> exec)
        {
            Name = name;
            _exec = exec;
        }

        public IGenerator<string, string> StartConversation(object args)
        {
            return new DelegateGenerator(_exec);
        }
    }
    public class DelegateService<TArg> : IService
    {
        public string Name { get; private set; }

        private readonly Func<TArg, string> _exec;

        public DelegateService(string name, Func<TArg, string> exec)
        {
            Name = name;
            _exec = exec;
        }

        public IGenerator<string, string> StartConversation(object args)
        {
            return new DelegateGenerator<TArg>(_exec, (TArg)args);
        }
    }
}
