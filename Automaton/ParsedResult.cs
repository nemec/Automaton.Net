using System.Diagnostics;
using Automaton.Net.Services;

namespace Automaton.Net
{

    [DebuggerDisplay("{Service.Name} : {Similarity}")]
    public class ParsedResult
    {
        public double Similarity { get; set; }

        public IService Service { get; set; }

        public object Arguments { get; set; }
    }
}
