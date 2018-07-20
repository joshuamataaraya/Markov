namespace markov.Controllers
{
    public partial class puzzleController
    {
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
    }
}
