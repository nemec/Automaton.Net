using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automaton.Hub
{
    public class HubConnectedEventArgs
    {
        public IHub Hub { get; set; }

        public HubConnectedEventArgs(IHub connectedHub)
        {
            Hub = connectedHub;
        }
    }
}
