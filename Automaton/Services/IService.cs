using GeneratorAsync;

namespace Automaton.Net.Services
{
    /// <summary>
    /// A service provided by a plugin
    /// </summary>
    /// 
    public interface IService
    {
        string Name { get; }

        IGenerator<string, string> StartConversation(object args);
    }
}
