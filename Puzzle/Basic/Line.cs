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

        public void Solve()
        {
            SolveUpdateSections();
            SolveFillSections();
            SolveFillDots();
            SolveCheckComplete();
        }

        private void SolveUpdateSections()
        {
            foreach (Clue clue in OpenClues)
            {
                clue.UpdateSections();
            }
        }

        private void SolveFillSections()
        {
            foreach (Clue clue in OpenClues)
            {
                clue.FillSections();
            }
        }

        private void SolveFillDots()
        {
            var newCells = new List<Cell>();
            foreach (Cell cell in OpenClues.SelectMany(clue => clue.PossCells.Where(cell => !newCells.Contains(cell))))
            {
                newCells.Add(cell);
            }
            foreach (Cell cell in Cells.Where(cell => !newCells.Contains(cell) && (cell.Value == 0)))
            {
                cell.UpdateCell(-1);
            }
            var cells = new List<Cell>();
            foreach (Clue clue in Clues.Where(c => c.PossSections.Count != 0))
            {
                foreach (var section in clue.PossSections)
                {
                    foreach (Cell cell in section)
                    {
                        if (!cells.Contains(cell) && cell.Value == 0)
                        {
                            cells.Add(cell);
                        }
                    }
                }
            }
            foreach (Cell cell in Cells.Where(cell => !cells.Contains(cell) && cell.Value == 0))
            {
                cell.UpdateCell(-1);
            }
        }

        private void SolveCheckComplete()
        {
            foreach (Clue clue in OpenClues)
            {
                clue.CompleteAny();
            }
        }

        public void OriginalCalculateFirstLast()
        {
            var start = 0;
            var end = Length - 1;
            foreach (Clue clue in OpenClues)
            {
                clue.First = Cells[start];
                start += clue.Value + 1;
            }
            foreach (Clue clue in OpenClues.AsEnumerable().Reverse())
            {
                clue.Last = Cells[end];
                end -= clue.Value + 1;
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
