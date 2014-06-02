using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeneratorAsync;

namespace Automaton.Net
{
    public interface IRegistrar
    {
        void Register(string name, IGrammar grammar, Func<string> exec);

        void Register<TArg>(string name, IGrammar grammar, Func<TArg, string> exec);

        void Register<TArg>(string name, IGrammar grammar, Func<IYield<string, string>, TArg, Task> exec);

        //void Register(string name, IGrammar grammar, IGenerator generator);

        List<RegistrarItem> Entries { get; }
    }
}
