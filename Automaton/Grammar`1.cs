using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Automaton.Net
{
    public class Grammar<TArg> : IGrammar
    {
        private readonly List<GrammarToken<TArg>> _tokens;

        internal Grammar(List<GrammarToken<TArg>> tokens)
        {
            _tokens = tokens;
        }

        public IEnumerable<string> Identifiers
        {
            get
            {
                foreach(var tok in _tokens)
                {
                    foreach(var ident in tok.Identifiers)
                    {
                        yield return ident;
                    }
                }
            }
        }

        public IEnumerable<PropertyInfo> Properties
        {
            get
            {
                foreach(var tok in _tokens)
                {
                    var getter = tok.Getter;
                    if (getter.NodeType == ExpressionType.Lambda)
                    {
                        var body = getter.Body as MemberExpression;
                        if (body != null)
                        {
                            var prop = body.Member as PropertyInfo;
                            if (prop != null)
                            {
                                yield return prop;
                            }
                        }
                    }
                }
            }
        }
    }

    internal class GrammarToken<TArg>
    {
        public Expression<Func<TArg, object>> Getter { get; set; }

        public List<string> Identifiers { get; set; }
    }
}
