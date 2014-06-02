using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Automaton.Net
{
    public class GrammarBuilder<TArg>
    {
        private readonly List<GrammarToken<TArg>> ArgumentTokens = new List<GrammarToken<TArg>>();

        private IGrammar _grammar;

        public IGrammar Grammar
        {
            get
            {
                return _grammar ?? (_grammar = CreateGrammar());
            }
        }

        public void AddArgument(Expression<Func<TArg, object>> getter, params string[] keywords)
        {
            if (getter.NodeType == ExpressionType.Lambda)
            {
                var body = getter.Body as MemberExpression;
                if (body != null)
                {
                    var prop = body.Member as PropertyInfo;
                    if (prop != null)
                    {
                        ArgumentTokens.Add(new GrammarToken<TArg>
                        {
                            Getter = getter,
                            Identifiers = keywords.ToList()
                        });
                        return;
                    }
                }
            }
            throw new ArgumentException("Getter must be of form (arg => arg.Property)");
        }

        private IGrammar CreateGrammar()
        {
            return new Grammar<TArg>(ArgumentTokens);
        }
    }
}
