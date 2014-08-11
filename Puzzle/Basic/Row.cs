using System.Collections.Generic;

namespace Griddler.PuzzleModel
{
    public class Row : Line
    {
        public Row(IList<int> clues, int key) : base(clues, key)
        {
            IsRow = true;
        }
    }
}
