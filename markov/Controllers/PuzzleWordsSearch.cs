using System.Collections.Generic;
using System.Linq;

namespace markov.Controllers
{
    public partial class puzzleController
    {
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
    }
}
