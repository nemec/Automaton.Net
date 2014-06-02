using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Automaton.Net.Services;
using System.Linq;
using GeneratorAsync;

namespace Automaton.Net
{
    public class Registrar : IRegistrar
    {
        private readonly Dictionary<string, RegistrarItem> _registrar; 

        public List<RegistrarItem> Entries
        {
            get
            {
                return _registrar.Values.ToList();
            }
        }
        
        public Registrar()
        {
            _registrar = new Dictionary<string, RegistrarItem>();
        }

        public void Register(string name, IGrammar grammar, Func<string> exec)
        {
            _registrar.Add(name, new RegistrarItem
            {
                Grammar = grammar,
                Service = new DelegateService(name, exec)
            });
        }

        public void Register<TArg>(string name, IGrammar grammar, Func<TArg, string> exec)
        {
            _registrar.Add(name, new RegistrarItem
            {
                Grammar = grammar,
                Service = new DelegateService<TArg>(name, exec)
            });
        }

        public void Register<TArg>(string name, IGrammar grammar, Func<IYield<string, string>, TArg, Task> exec)
        {
            _registrar.Add(name, new RegistrarItem
                {
                    Grammar = grammar,
                    Service = new GeneratorService<TArg>(name, exec)
                });
        }
    }
}
