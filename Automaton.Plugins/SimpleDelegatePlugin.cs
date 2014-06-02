using Automaton.Net;

namespace Automaton.Plugins
{
    public class SimpleDelegatePlugin : IPlugin
    {
        public string Name {get { return "Simple"; } }

        public void Register(IRegistrar registrar)
        {
            registrar.Register("simple", Grammar.EmptyGrammar, DoThing);
        }

        public string DoThing()
        {
            return "Hello world!";
        }
    }
}
