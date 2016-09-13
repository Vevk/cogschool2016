using System.Threading.Tasks;
using Eto.Parse.Grammars;

namespace Bot_Application1.SynonymClient
{
    public class SynonymsService
    {
        public async Task<string> GetSimilarSentenceAsync(AnalyzeTextResult[] sentence)
        {
            var analyzedSentence = sentence[1].Result.ToString();

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



            SynonymClient cln = new SynonymClient();
            
            
            var res = await cln.GetSynonymAsync<SynonymResult>("love");
            return "passion";

        }

    }
}