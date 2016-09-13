using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Eto.Parse.Grammars;

namespace Bot_Application1.SynonymClient
{
    public class SynonymsService
    {
        public async Task<string> GetSimilarSentenceAsync(AnalyzeTextResult[] sentence)
        {
            var analyzedSentence = sentence[1].Result.ToString();

            var regex = new Regex(@"\(\s*(?<part>DT|NN|NNP|NNS|NNPS|JJ|JJR|JJS|TO|PRP|RB|RBR|RBS|VB|VBP|VBZ|VBD|VBG|VBN|WRB)\s+(?<word>\w+)\s*\)");

            var matches = regex.Matches(analyzedSentence);

            SynonymClient cln = new SynonymClient();

            string result = string.Empty;

            foreach (Match match in matches)
            {
                var part = match.Groups["part"].Success ? match.Groups["part"].Value : String.Empty;
                var word = match.Groups["word"].Success ? match.Groups["word"].Value : String.Empty;

                if(string.IsNullOrEmpty(part) || string.IsNullOrEmpty(word))
                    continue;

                var synonyms = await cln.GetSynonymAsync<SynonymResult>(word);

                string synonym = String.Empty;

                if (synonyms != null)
                {
                    switch (part)
                    {
                        case "DT":
                        case "PRP":
                        case "WRB":
                        case "TO":
                            break;
                        case "NNP":
                        case "NN":
                        case "NNS":
                        case "NNPS":
                            var pos = synonyms.noun;
                            synonym = GetSynonymByPos(pos);
                            break;
                        case "VBZ":
                        case "VBP":
                        case "VB":
                        case "VBD":
                        case "VBG":
                        case "VBN":
                            var verb = synonyms.verb;
                            synonym = GetSynonymByPos(verb);
                            break;
                        case "JJ":
                        case "JJR":
                        case "JJS":
                            var adj = synonyms.adjective;
                            synonym = GetSynonymByPos(adj);
                            break;
                        case "RB":
                        case "RBS":
                        case "RBR":
                            var adv = synonyms.adverb;
                            synonym = GetSynonymByPos(adv);
                            break;
                        default:
                            break;
                    }
                }
                if (string.IsNullOrEmpty(synonym))
                {
                    result += word + " ";
                }
                else
                {
                    result += synonym + " ";
                }
            }
            
            
            return result;

        }

        private string GetSynonymByPos(Adverb pos)
        {
            string synonym = String.Empty;
            var rnd = new Random();
            if (pos.syn != null && pos.syn.Count > 0)
            {
                int index = rnd.Next(0, pos.syn.Count - 1);
                synonym = pos.syn[index];
            }
            else if (pos.sim != null && pos.sim.Count > 0)
            {
                int index = rnd.Next(0, pos.sim.Count - 1);
                synonym = pos.sim[index];
            }
            else if (pos.rel != null && pos.rel.Count > 0)
            {
                int index = rnd.Next(0, pos.rel.Count - 1);
                synonym = pos.rel[index];
            }
            return synonym;
        }

        private string GetSynonymByPos(Adjective pos)
        {
            string synonym = String.Empty;
            var rnd = new Random();
            if (pos.syn != null && pos.syn.Count > 0)
            {
                int index = rnd.Next(0, pos.syn.Count - 1);
                synonym = pos.syn[index];
            }
            else if (pos.sim != null && pos.sim.Count > 0)
            {
                int index = rnd.Next(0, pos.sim.Count - 1);
                synonym = pos.sim[index];
            }
            else if (pos.rel != null && pos.rel.Count > 0)
            {
                int index = rnd.Next(0, pos.rel.Count - 1);
                synonym = pos.rel[index];
            }
            return synonym;
        }

        private static string GetSynonymByPos(Noun pos)
        {
            string synonym = String.Empty;
            var rnd = new Random();
            if (pos.syn != null && pos.syn.Count > 0)
            {
                int index = rnd.Next(0, pos.syn.Count - 1);
                synonym = pos.syn[index];
            }
            else if (pos.sim != null && pos.sim.Count > 0)
            {
                int index = rnd.Next(0, pos.sim.Count - 1);
                synonym = pos.sim[index];
            }
            else if (pos.rel != null && pos.rel.Count > 0)
            {
                int index = rnd.Next(0, pos.rel.Count - 1);
                synonym = pos.rel[index];
            }
            return synonym;
        }

        private static string GetSynonymByPos(Verb pos)
        {
            string synonym = String.Empty;
            var rnd = new Random();
            if (pos.syn != null && pos.syn.Count > 0)
            {
                int index = rnd.Next(0, pos.syn.Count - 1);
                synonym = pos.syn[index];
            }
            else if (pos.sim != null && pos.sim.Count > 0)
            {
                int index = rnd.Next(0, pos.sim.Count - 1);
                synonym = pos.sim[index];
            }
            else if (pos.rel != null && pos.rel.Count > 0)
            {
                int index = rnd.Next(0, pos.rel.Count - 1);
                synonym = pos.rel[index];
            }
            return synonym;
        }
    }
}

//var grammar = new EbnfGrammar(EbnfStyle.BracketComments).Build(@"
            //    (* := is an extension to define a literal with no whitespace between repeats and sequences *)
            //    ws := {? Terminals.WhiteSpace ?};

            //    letter := ? Terminals.LetterOrDigit ?;

            //    simple value := letter, {letter};

            //    bracket value = simple value, {simple value};

            //    optional bracket = '(', bracket value, ')' | simple value;

            //    first = optional bracket;

            //    second = optional bracket;

            //    grammar = ws, first, second, ws;
            //    ", "grammar");


            //var match = grammar.Match(analyzedSentence);

            //var firstValue = match["first"]["value"].Value;
            //var secondValue = match["second"]["value"].Value;