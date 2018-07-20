using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace markov.Controllers
{
    [Route("api/[controller]")]
    public class puzzleController : Controller
    {
        [HttpPost("[action]")]
        public IEnumerable<MarkovRequest> Markov([FromBody] MarkovRequest markovRequest)
        {
            List<List<Value>> SortedList = new List<List<Value>>();

            //Sort values matrix
            foreach (var row in markovRequest.values)
            {
                List<Value> sorteRow = row.OrderBy(o => o.order).ToList();
                SortedList.Add(sorteRow);
            }
            markovRequest.values = SortedList;

            //Apply Markov Algorithm
            for (int row = 0; row < markovRequest.cypher.Count; row++)
            {
                for (int ruleNum = 0; ruleNum < markovRequest.values[row].Count; ruleNum++)
                {

                    //get current value
                    Value value = markovRequest.values[row][ruleNum];

                    //get current rule
                    Rule rule = markovRequest.rules[value.rule];

                    //Search for the rule in the current string been analyzed
                    if (markovRequest.cypher[row].IndexOf(rule.source, 0) >= 0)
                    {
                        markovRequest.cypher[row] = markovRequest.cypher[row].Replace(rule.source, rule.replacement);
                        if (value.isTermination)
                        {
                            break;
                        }
                        ruleNum = 0;
                    }
                }
            }

            //Convert Cypher to a matrix
            List<List<char>> puzzle = new List<List<char>>();
            foreach (var row in markovRequest.cypher)
            {
                List<char> charRow = new List<char>();
                charRow.AddRange(row);
                puzzle.Add(charRow);
            }

            markovRequest.puzzle = puzzle;

            PuzzleWordsSearch puzzleWordsSearch = new PuzzleWordsSearch(markovRequest.puzzle);

            foreach (var word in markovRequest.words)
            {
                Word wordFound = puzzleWordsSearch.wordsearch(word);
                markovRequest.wordsBreakdown.Add(wordFound);
            }


            //search Words

            // List<Result> results = new List<Result>();
            // results.Add(result);
            List<MarkovRequest> results = new List<MarkovRequest>();
            results.Add(markovRequest);
            return results;
        }
        public class PuzzleWordsSearch
        {
            private int[] xd = { -1, -1, 0, +1, +1, +1, 0, -1 };
            private int[] yd = { 0, -1, -1, -1, 0, +1, +1, +1 };
            private List<List<char>> puzzle { get; set; }
            private int X
            {
                get
                {
                    return this.puzzle.Count;
                }
            }
            private int Y
            {
                get
                {
                    if (this.X != 0)
                        return this.puzzle[0].Count;
                    return 0;
                }
            }
            public PuzzleWordsSearch(List<List<char>> puzzle)
            {
                this.puzzle = puzzle;
            }
            private bool wordsearch(string w, int x, int y, int d, Word word)
            {
                if (w.Length == 0)
                {
                    return true; // end of word
                }
                if (x < 0 || y < 0 || x >= this.X || y >= this.Y)
                {
                    if (word.breakdown.Count > 0)
                        word.breakdown.RemoveAt(word.breakdown.Count - 1);
                    return false; // out of bounds
                }
                if (this.puzzle[x][y] != w.First())
                {
                    if (word.breakdown.Count > 0)
                        word.breakdown.RemoveAt(word.breakdown.Count - 1);
                    return false; // wrong character  
                }
                // otherwise scan forwards
                word.breakdown.Add(new Breakdown(w.First(), x, y));
                return wordsearch(w.Substring(1), x + xd[d], y + yd[d], d, word);
            }
            private bool wordsearch(string w, int x, int y, Word word)
            {
                int d;
                for (d = 0; d < 8; d++)
                    if (wordsearch(w, x, y, d, word)) return true;
                return false;
            }
            public Word wordsearch(string w)
            {
                int x, y;
                Word word = new Word();
                word.word = w;
                for (x = 0; x < this.X; x++) for (y = 0; y < this.Y; y++)
                        if (wordsearch(w, x, y, word)) return word;
                return word;
            }
        }



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
        public class Letter
        {
            public Letter(char character, int row, int column)
            {
                this.character = character;
                this.column = column;
                this.row = row;
            }
            public char character { get; set; }
            public int row { get; set; }
            public int column { get; set; }
        }
        public class Word
        {
            public Word()
            {
                this.breakdown = new List<Breakdown>();
            }
            public string word { get; set; }
            public List<Breakdown> breakdown { get; set; }
        }
        public class Breakdown
        {
            public Breakdown(char character, int row, int column)
            {
                this.character = character;
                this.row = row;
                this.column = column;
            }
            public char character { get; set; }
            public int row { get; set; }
            public int column { get; set; }
        }
        public class Value
        {
            public int order { get; set; }
            public int rule { get; set; }
            public bool isTermination { get; set; }

        }
        public class Rule
        {
            public string source { get; set; }
            public string replacement { get; set; }

        }
    }

    [Route("api/[controller]")]
    public class WeatherForecastController : Controller
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> markov()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }


        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }
    }
}
