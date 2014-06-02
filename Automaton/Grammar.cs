using System.Collections.Generic;
using System.Reflection;

namespace Automaton.Net
{
    public class Grammar : IGrammar
    {
        public IEnumerable<string> Identifiers { get; private set; }

        public IEnumerable<PropertyInfo> Properties { get; private set; }

        public Grammar()
        {
            Identifiers = new string[0];
            Properties = new PropertyInfo[0];
        }

        public static readonly Grammar EmptyGrammar = new Grammar();
    }
}
