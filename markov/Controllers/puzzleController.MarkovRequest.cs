using System.Collections.Generic;

namespace markov.Controllers
{
    public partial class puzzleController
    {
        public class MarkovRequest
        {
            public MarkovRequest()
            {
                this.cypher = new List<string>();
                this.values = new List<List<Value>>();
                this.words = new List<string>();
                this.puzzle = new List<List<char>>();
                this.wordsBreakdown = new List<Word>();
            }
            public List<string> cypher { get; set; }
            public List<List<Value>> values { get; set; }
            public List<string> words { get; set; }
            public Rule[] rules { get; set; }
            public List<List<char>> puzzle { get; set; }
            public List<Word> wordsBreakdown { get; set; }
        }
    }
}
