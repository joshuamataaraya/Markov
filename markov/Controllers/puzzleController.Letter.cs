namespace markov.Controllers
{
    public partial class puzzleController
    {
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
    }
}
