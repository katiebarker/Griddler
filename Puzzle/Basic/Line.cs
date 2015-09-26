using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Griddler.PuzzleModel
{
    public class Line
    {
        public List<Cell> Cells { get; set; }
        public List<Clue> Clues { get; set; }
        public int Key { get; set; }
        public bool IsRow { get; set; }
        public Puzzle OwnerPuzzle { get; set; }        

        public string KeyString
        {
            get
            {
                return IsRow ? "Row: " + (Key + 1) : "Column: " + (Key + 1);
            }
        }

        public int Length
        {
            get
            {
                return IsRow ? OwnerPuzzle.Width : OwnerPuzzle.Height;
            }
        }

        public int Total
        {
            get
            {
                return Clues.Sum(clue => clue.Value);
            }
        }

        public List<Clue> OpenClues
        {
            get
            {
                return new List<Clue>(Clues.Where(c => c.IsComplete == false));
            }
        }  

        public void Reset()
        {
            foreach (Clue clue in Clues)
            {
                clue.Reset();
            }
        }
    }
}
