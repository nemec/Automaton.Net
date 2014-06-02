using System;
using System.Globalization;

namespace Automaton.Net
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ArgumentQuestionAttribute : Attribute
    {
        private readonly string _question;

        /// <summary>
        /// A natural-language request for more information about an argument.
        /// </summary>
        /// <param name="question"></param>
        public ArgumentQuestionAttribute(string question)
        {
            _question = question;
        }

        public virtual string GetQuestionForLocale(CultureInfo info)
        {
            return _question;  // TODO localization ability
        }
    }
}
