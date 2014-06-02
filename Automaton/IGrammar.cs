using System;
using System.Collections.Generic;
using System.Reflection;

namespace Automaton.Net
{
    public interface IGrammar
    {
        IEnumerable<string> Identifiers { get; }

        IEnumerable<PropertyInfo> Properties { get; }
    }
}
