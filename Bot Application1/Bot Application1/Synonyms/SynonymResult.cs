using System.Collections.Generic;

namespace Bot_Application1
{
    /// <summary>
    /// This is the result of analysis from one analyzer.
    /// </summary>
    public class SynonymResult
    {
        public Adjective adjective { get; set; }
        public Noun noun { get; set; }

        public Verb verb { get; set; }
        public Adverb adverb { get; set; }

    }

    public class Adjective
    {
        public List<string> syn { get; set; }
        public List<string> sim { get; set; }

        public List<string> rel { get; set; }
    }

    public class Adverb
    {
        public List<string> syn { get; set; }
        public List<string> sim { get; set; }

        public List<string> rel { get; set; }
    }

    public class Noun
    {
        public List<string> syn { get; set; }

        public List<string> sim { get; set; }

        public List<string> rel { get; set; }
    }

    public class Verb
    {
        public List<string> syn { get; set; }

        public List<string> sim { get; set; }

        public List<string> rel { get; set; }
    }
}