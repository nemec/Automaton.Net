using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Automaton.Net;
using log4net;

[assembly: log4net.Config.XmlConfigurator]

namespace Automaton.Client
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (Program));
        static void Main()
        {
            var registrar = new Registrar();
            var pluginAsm = Assembly.LoadFrom(Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "Automaton.Plugins.dll"));
            foreach (var pluginType in pluginAsm.GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(IPlugin))))
            {
                var constructor = pluginType.GetConstructor(Type.EmptyTypes);
                if (constructor == null)
                {
                    Logger.WarnFormat(
                        "Empty constructor for plugin {0} not found. Skipping.",
                        pluginType.FullName);
                    continue;
                }
                var plugin = (IPlugin) Activator.CreateInstance(pluginType);
                plugin.Register(registrar);
            }

            
            var interpreter = new Interpreter(registrar, false);
            string next = null;
            while (true)
            {
                if (next == null)
                {
                    Console.WriteLine("Please ask a question:");
                    Console.Write(">> ");
                    next = Console.ReadLine();
                }

                var c = interpreter.Interpret(next).ToList();
                if (c.Count <= 0)
                {
                    Logger.Info("No commands found.");
                    next = null;
                    continue;
                }
                var elem = c[0];
                Logger.InfoFormat("Parsed command '{0}': {1}",
                    elem.Service.Name, elem.Similarity);
                var convo = elem.Service.StartConversation(elem.Arguments);

                var response = convo.Next();
                do
                {
                    Console.WriteLine(response);
                    Console.Write(">> ");
                    next = Console.ReadLine();
                }
                while (convo.TrySend(next, out response));
            }
        }
    }
}
