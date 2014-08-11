using System.Collections.Generic;

namespace Griddler.PuzzleModel
{
    public class  Column : Line
    {
        public Column(IList<int> clues, int key) : base(clues, key)
        {
            IsRow = false;
        }
    }
}