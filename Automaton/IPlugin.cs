
namespace Automaton.Net
{
    /// <summary>
    /// Describes an Automaton plugin
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Register the plugin's features.
        /// </summary>
        /// <param name="registrar"></param>
        void Register(IRegistrar registrar);

        string Name { get; }
    }
}
