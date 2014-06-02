using System;

namespace Automaton.Net
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ArgumentDescriptionAttribute : Attribute
    {
        public string Description { get; private set; }

        /// <summary>
        /// Annotates an argument property with a description.
        /// </summary>
        /// <param name="description"></param>
        public ArgumentDescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}
