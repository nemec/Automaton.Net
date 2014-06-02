using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Automaton.Net.Services;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.tagger.maxent;
using java.util;
using System.Linq;

namespace Automaton.Net
{
    public class Interpreter
    {
        public Interpreter(IRegistrar registrar, bool usePosTagger = true)
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _entries = registrar.Entries;
            if (usePosTagger)
            {
                PosTagger = new MaxentTagger(
                    Path.Combine(baseDir, "data", "english-bidirectional-distsim.tagger"));
            }
        }

        private readonly List<RegistrarItem> _entries = new List<RegistrarItem>();

        private const double MinSimilarityThreshold = 0.0;
        // Possibly define a "warn threshold" to confirm service with user if
        // similarity is > min-threshold but not very similar (<0.5)?

        private MaxentTagger PosTagger { get; set; }

        private string CleanText(string rawText)
        {
            // stemming
            // might also want to clean grammar
            return rawText.Trim().ToLowerInvariant();  // TODO clean up text
        }

        private List<Token> TokenizeandTag(string rawText)
        {
            var sentences = MaxentTagger.tokenizeText(new java.io.StringReader(rawText));
            var tokenList = new List<Token>();
            foreach (List o in sentences.toArray())
            {

                if (PosTagger == null)
                {
                    tokenList.AddRange(o
                        .toArray()
                        .Cast<HasWord>()
                        .Select(w => new Token
                    {
                        Text = w.word(),
                        PartOfSpeech = "SYM"
                    }));
                }
                else
                {
                    tokenList.AddRange(PosTagger
                    .tagSentence(o)
                    .toArray()
                    .Cast<TaggedWord>()
                    .Select(w => new Token
                    {
                        Text = w.word(),
                        PartOfSpeech = w.tag()
                    }));
                }
            }
            return tokenList;
        }

        public IEnumerable<ParsedResult> Interpret(string rawText)
        {
            rawText = CleanText(rawText);
            var tokens = TokenizeandTag(rawText);
            var results = (from entry in _entries
                           let similarity = GetSimilarity(tokens, entry.Grammar, entry.Service.Name)
                           where similarity > MinSimilarityThreshold
                           let args = ExtractArguments(tokens, entry.Grammar, entry.Service)
                           select new ParsedResult
                           {
                               Service = entry.Service,
                               Similarity = similarity,
                               Arguments = args
                           });

            return results.OrderByDescending(k => k.Similarity);
        }

        private static double GetSimilarity(IReadOnlyList<Token> tokens, IGrammar grammar, string serviceName)
        {
            var similarity = 0.0;
            for (var idx = 0; idx < tokens.Count; idx++)
            {
                var tok = tokens[idx];
                // TODO pick a culture?
                if (StringComparer.CurrentCultureIgnoreCase.Equals(tok.Text, serviceName))
                {
                    similarity += 10;

                    var positionModifier = 15 * ((1.0 * tok.Text.Length - idx) / tok.Text.Length);
                    similarity += positionModifier;

                    if (tok.PartOfSpeech.StartsWith("VB"))
                    {
                        similarity += 3;
                    }
                }

                var props = grammar.Properties.ToList();
                var idents = String.Join(" ", grammar.Identifiers);

                var numArgs = props.Count;
                var numMatch = 0;

                if (numArgs > 0)
                {
                    numMatch += props.Count(prop => 
                        idents.IndexOf(prop.Name, StringComparison.Ordinal) >= 0);

                    if (idents.IndexOf(tok.Text, StringComparison.Ordinal) >= 0)
                    {
                        numMatch++;
                    }

                    similarity += 5.0 * numMatch / numArgs;
                }
            }
            return similarity;
        }

        private static object ExtractArguments(List<Token> tokens, IGrammar grammar, IService service)
        {
            var arg = grammar.Properties.FirstOrDefault();
            if (arg == null)
            {
                return null;
            }
            // TODO find a better way to create arguments...
            return Activator.CreateInstance(arg.DeclaringType);
        }

        [DebuggerDisplay("<Token '{Text}' | {PartOfSpeech}>")]
        private class Token
        {
            public string Text { get; set; }

            public string PartOfSpeech { get; set; }
        }
    }
}
