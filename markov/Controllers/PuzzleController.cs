using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace markov.Controllers
{
    [Route("api/[controller]")]
    public partial class puzzleController : Controller
    {
        [HttpPost("[action]")]
        public MarkovRequest Markov([FromBody] MarkovRequest markovRequest)
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

            return markovRequest;
        }
    }
}
