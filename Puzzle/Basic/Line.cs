using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Griddler.PuzzleModel
{
    public class Line
    {
        protected Line(IEnumerable<int> clues, int key)
        {
            Key = key;
            Cells = new List<Cell>();
            Clues = new List<Clue>();
            foreach (var clue in clues)
            {
                Clues.Add(new Clue(clue, this));
            }
        }

        public void AddCell(Cell cell)
        {
            Cells.Add(cell);
            foreach (var clue in Clues)
            {
                clue.PossSections.First().Add(cell);
            }
        }

        public List<Cell> Cells { get; private set; }
        public List<Clue> Clues { get; private set; }
        public int Key { get; private set; }
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
