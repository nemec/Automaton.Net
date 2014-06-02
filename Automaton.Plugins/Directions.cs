using System.Threading.Tasks;
using Automaton.Net;
using System;
using GeneratorAsync;

namespace Automaton.Plugins
{
    public class Directions : IPlugin
    {
        private IGrammar _grammar;

        public string Name { get { return "Directions"; } }

        public void Register(IRegistrar registrar)
        {
            var builder = new GrammarBuilder<DirectionsArgs>();
            builder.AddArgument(a => a.Source, "starting at", "from");
            builder.AddArgument(a => a.Destination, "ending at", "to");
            _grammar = builder.Grammar;

            registrar.Register<DirectionsArgs>("directions", _grammar, GetDirections);
        }

        public async Task GetDirections(IYield<string, string> gen, DirectionsArgs args)
        {
            await PluginUtils.AskForPropertiesWhileNull(gen, args, _grammar);

            await gen.Yield(
                String.Format("You have traveled from {0} to {1}.",
                args.Source, args.Destination));
        }

        public class DirectionsArgs
        {
            [ArgumentDescription("The starting point.")]
            [ArgumentQuestion("Where are you starting from?")]
            public string Source { get; set; }

            [ArgumentDescription("The ending point.")]
            [ArgumentQuestion("Where are you headed?")]
            public string Destination { get; set; }
        }
    }
}
