using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Griddler.PuzzleModel
{
    public class Clue : IClue
    {
        public Clue(int value, Line ownerLine)
        {
            Value = value;
            OwnerLine = ownerLine;
            PossSections = new List<List<Cell>>();
            PossSections.Add(new List<Cell>());
        }

        public Line OwnerLine { get; private set; }
        public int Value { get; private set; }

        private int Index
        {
            get
            {
                return OwnerLine.Clues.IndexOf(this);
            }
        }

        public string KeyString
        {
            get
            {
                return String.Format("{0}, Clue Number: {1}, Value: {2}", OwnerLine.KeyString, Index, Value);
            }
        }

        public int Key(Cell cell)
        {
            return cell.GetKey(OwnerLine);
        }

        public List<Cell> PossCells
        {
            get
            {
                List<Cell> cells = new List<Cell>();
                foreach (var section in PossSections)
                {
                    foreach (var cell in section)
                    {
                        cells.Add(cell);
                    }
                }
                return cells.OrderBy(c => c.Key.X).OrderBy(c => c.Key.Y).ToList();
            }
        }

        public List<List<Cell>> PossSections { get; set; }

        internal Clue PreviousClue
        {
            get
            {
                return OwnerLine.Clues.ElementAtOrDefault(Index - 1);
            }
        }

        public Clue NextClue
        {
            get
            {
                return OwnerLine.Clues.ElementAtOrDefault(Index + 1);
            }
        }

        public Cell First
        {
            get { return PossCells.First(); }

            set
            {
                foreach (var sect in PossSections)
                {
                    sect.RemoveAll(cell => OwnerLine.Cells.IndexOf(cell) < OwnerLine.Cells.IndexOf(value));
                }
            }
        }

        public Cell Last
        {
            get { return PossCells.Last(); }
            set
            {
                foreach (var section in PossSections)
                {
                    section.RemoveAll(cell => OwnerLine.Cells.IndexOf(cell) > OwnerLine.Cells.IndexOf(value));
                }
            }
        }

        public Cell FirstEnd
        {
            get
            {
                return OwnerLine.Cells.ElementAt(Key(First) + Value);
            }
        }

        public Cell LastEnd
        {
            get
            {
                return OwnerLine.Cells.ElementAt(Key(Last) - Value);
            }
        }

        public int Start
        {
            get { return Key(First); }
            set
            {
                if (value >= 0 && value < OwnerLine.Length)
                    First = OwnerLine.Cells[value];
            }
        }

        public int End
        {
            get { return Key(Last) + 1; }
            set
            {
                if (value >= 0 && value < OwnerLine.Length)
                    Last = OwnerLine.Cells[value - 1];
            }
        }

        public bool IsComplete { get; private set; }
               
        public void UpdateEnds()
        {
            if (PreviousClue != null)
                First = OwnerLine.Cells[Key(PreviousClue.FirstEnd) + 1];
            if (NextClue != null)
                Last = OwnerLine.Cells[Key(NextClue.LastEnd) - 1];

        }

        
        public void CompleteAny()
        {
            //right number of poss cells
            if (End - Start == Value)
            {
                Complete(PossCells);
                return;
            }

            //Find filled confirmed clue + all filled consecutive
            var x = PossCells.FirstOrDefault(c => (c.Value == 1 && c.IsAvaliable(this) && c.IsOnlyClue(this)));
            if (x == null) return;

            List<Cell> cells = new List<Cell>();
            cells.Add(x);

            //Consequetive cells to left
            for (int i = Key(x) - 1; i > 0; i--)
            {
                var cell = PossCells.SingleOrDefault(c => Key(c) == i);
                if (cell != null && cell.Value == 1)
                {
                    if (!cell.IsAvaliable(this))
                    {
                        throw new Exception("Trying to claim unavailiable cell");
                    }
                    cells.Add(cell);
                }
                else break;
            }

            //Consequetive cells to right
            for (int i = Key(x) + 1; i < OwnerLine.Length; i++)
            {
                var cell = PossCells.SingleOrDefault(c => Key(c) == i);
                if (cell != null && cell.Value == 1)
                {
                    if (!cell.IsAvaliable(this))
                    {
                        throw new Exception("Trying to claim unavailiable cell");
                    }
                    cells.Add(cell);
                }
                else break;
            }

            foreach (var cell in cells)
            {
                cell.Claim(this);
            }
            Start = cells.Last().GetKey(OwnerLine) - Value;
            End = cells.First().GetKey(OwnerLine) + Value;

            //Complete if right number of filled cells
            if (cells.Count == Value)
            {
                Complete(cells);
            }

        }

        public void Complete(IEnumerable<Cell> finalCells)
        {
            finalCells = finalCells.OrderBy(c => c.Key.X).OrderBy(c => c.Key.Y);
            int first = Key(finalCells.First());
            int last = Key(finalCells.Last());
            foreach (Cell cell in finalCells)
            {
                cell.UpdateCell(1);
                cell.Claim(this);
            }
            if (first > 0) OwnerLine.Cells[first - 1].UpdateCell(-1);
            if (last + 1 < OwnerLine.Length) OwnerLine.Cells[last + 1].UpdateCell(-1);
            IsComplete = true;
        }

        public void ClaimAny()
        {
            foreach (var cell in PossCells.Where(cell => cell.IsOnlyClue(this) && cell.Value == 1))
            {
                cell.Claim(this);
            }
        }

        public void Reset()
        {
            PossSections = new List<List<Cell>>();
            PossSections.Add(new List<Cell>(OwnerLine.Cells));
            IsComplete = false;
        }
    }
}
