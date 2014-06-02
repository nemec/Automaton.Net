using System.Diagnostics;
using Automaton.Net.Services;

namespace Automaton.Net
{
    [DebuggerDisplay("{Service.Name}")]
    public class RegistrarItem
    {
        public IService Service { get; set; }

        public IGrammar Grammar { get; set; }
    }
}
