using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automaton.Net
{
    internal interface IUserIdentity
    {
        CultureInfo Locale { get; set; }
    }
}
