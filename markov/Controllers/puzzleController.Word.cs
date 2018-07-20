using System.Collections.Generic;

namespace markov.Controllers
{
    public partial class puzzleController
    {
        public class Word
        {
            public Word()
            {
                this.breakdown = new List<Breakdown>();
            }
            public string word { get; set; }
            public List<Breakdown> breakdown { get; set; }
        }
    }
}
