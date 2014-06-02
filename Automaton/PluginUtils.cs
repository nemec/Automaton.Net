using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GeneratorAsync;

namespace Automaton.Net
{
    public static class PluginUtils
    {
        /// <summary>
        /// Iterate through all properties on <paramref name="args" /> and
        /// return IYields until all properties are non-null. Questions are
        /// pulled from either the property's
        /// <see cref="ArgumentQuestionAttribute"/>,
        /// <see cref="ArgumentDescriptionAttribute"/>,
        /// or the property name (in that order).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gen"></param>
        /// <param name="args"></param>
        /// <param name="culture">Culture to localize the question.</param>
        /// <returns></returns>
        public static async Task AskForPropertiesWhileNull<T>(
            IYield<string, string> gen, T args, CultureInfo culture = null)
        {
            await AskForPropertiesWhileNull(gen, args, typeof(T).GetProperties(), culture);
        }

        /// <summary>
        /// Iterate through the given properties on <paramref name="args" />
        /// and return IYields until all properties are non-null. Questions are
        /// pulled from either the property's
        /// <see cref="ArgumentQuestionAttribute"/>,
        /// <see cref="ArgumentDescriptionAttribute"/>,
        /// or the property name (in that order).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gen"></param>
        /// <param name="args"></param>
        /// <param name="propertyNames"></param>
        /// <param name="culture">Culture to localize the question.</param>
        /// <returns></returns>
        public static async Task AskForPropertiesWhileNull<T>(
           IYield<string, string> gen, T args, IList<string> propertyNames, CultureInfo culture = null)
        {
            await AskForPropertiesWhileNull(gen, args,
                typeof(T).GetProperties().Where(p => propertyNames.Contains(p.Name)),
                culture);
        }

        /// <summary>
        /// Iterate through the given properties on <paramref name="args" />
        /// and return IYields until all properties are non-null. Questions are
        /// pulled from either the property's
        /// <see cref="ArgumentQuestionAttribute"/>,
        /// <see cref="ArgumentDescriptionAttribute"/>,
        /// or the property name (in that order).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gen"></param>
        /// <param name="args"></param>
        /// <param name="grammar"></param>
        /// <param name="culture">Culture to localize the question.</param>
        /// <returns></returns>
        public static async Task AskForPropertiesWhileNull<T>(
            IYield<string, string> gen, T args, IGrammar grammar, CultureInfo culture = null)
        {
            var registeredProperties = grammar.Properties.Select(p => new
            {
                p.DeclaringType,
                p.Name
            });

            await AskForPropertiesWhileNull(gen, args,
                typeof(T).GetProperties()
                    .Where(p => registeredProperties.Contains(new
                    {
                        p.DeclaringType,
                        p.Name
                    })),
                culture);
        }

        private static async Task AskForPropertiesWhileNull<T>(IYield<string, string> gen,
            T args, IEnumerable<PropertyInfo> propertyNames, CultureInfo culture)
        {
            if (ReferenceEquals(args, null))
            {
                throw new ArgumentNullException("args");
            }
            foreach (var prop in propertyNames)
            {
                var value = prop.GetValue(args);
                while(value == null)
                {
                    value = await gen.Yield(GetQuestion(prop, culture));

                    prop.SetValue(args, value);
                }
            }
        }

        private static string GetQuestion(MemberInfo prop, CultureInfo culture)
        {
            var question = prop.GetCustomAttribute<ArgumentQuestionAttribute>();
            if (question != null)
            {
                // TODO use user's culture by default?
                return question.GetQuestionForLocale(culture ?? CultureInfo.InvariantCulture);
            }
            var desc = prop.GetCustomAttribute<ArgumentDescriptionAttribute>();
            if(desc != null)
            {
                return desc.Description;
            }

            return prop.Name;
        }
    }
}
