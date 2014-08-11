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

        protected int Key(Cell cell)
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
                return cells;
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
                if (!PossCells.Contains(value)) throw new Exception("Invalid cell");
                var section = PossSections.Single(s => s.Contains(value));

                section.RemoveAll(cell => OwnerLine.Cells.IndexOf(cell) < OwnerLine.Cells.IndexOf(value));
            }
        }

        public Cell Last
        {
            get { return PossCells.Last(); }
            set
            {
                if (!PossCells.Contains(value)) throw new Exception("Invalid cell");
                var section = PossSections.Single(s => s.Contains(value));

                section.RemoveAll(cell => OwnerLine.Cells.IndexOf(cell) > OwnerLine.Cells.IndexOf(value));
            }
        }

        public Cell FirstEnd
        {
            get
            {
                return OwnerLine.Cells.ElementAt<Cell>(Key(First) + Value);
            }
        }

        public Cell LastEnd
        {
            get
            {
                return OwnerLine.Cells.ElementAt<Cell>(Key(Last) - Value);
            }
        }

        public int Start
        {
            get { return Key(First); }
        }

        public int End
        {
            get { return Key(Last) + 1; }
        }

        public bool IsComplete { get; private set; }

        public void UpdateSections()
        {
            CompleteAny();
            var sections = new List<List<Cell>>();
            var section = new List<Cell>();
            foreach (var cell in PossCells)
            {
                if (cell.Value != -1 && cell.IsAvaliable(this))
                {
                    section.Add(cell);
                }
                else
                {
                    if (section.Count >= Value)
                    {
                        sections.Add(section);
                    }
                    section = new List<Cell>();
                }
            }
            if (section.Count >= Value)
            {
                sections.Add(section);
            }
            if (sections.Count == 0)
            {
                throw new Exception(String.Format("No room for clue: {0} {1}", OwnerLine.Key, OwnerLine.IsRow));
            }
            PossSections = sections;
        }

        public void FillSections()
        {
            UpdateSections();
            foreach (var section in PossSections)
            {
                //Section which contains definite filled cell
                if (
                    section.Any(
                        cell =>
                            (cell.Value == 1) &&
                            (cell.IsOnlyClue(this) &&
                            cell.IsAvaliable(this))))
                {
                    if (section.Count == Value)
                    {
                        Complete(section);
                        break;
                    }
                    if (section.Count < Value)
                    {
                        throw new Exception("bad section");
                    }
                    if (section.Count > Value)
                    {
                        PossSections = new List<List<Cell>> { section };
                        FillSection(section);
                    }

                }
                //If only one section
                if (PossSections.Count == 1)
                {
                    FillSection(section);
                }
            }
        }

        private void FillSection(List<Cell> section)
        {
            //Normal Fill
            foreach (var cell in section)
            {
                if (Key(cell) < Key(section.First()) + Value &&
                    Key(cell) > Key(section.Last()) - Value)
                {
                    cell.UpdateCell(1);
                    cell.Claim(this);
                }
            }

            //Extend existing cells
            //TODO: backwards? and update

            Cell anchorCell = PossCells.FirstOrDefault(c => c.Value == 1 && c.IsOnlyClue(this));
            if (anchorCell != null)
                foreach (Cell cell in PossCells)
                {
                    if (Key(cell) > Key(anchorCell) && Key(cell) < Start + Value)
                    {
                        cell.UpdateCell(1);
                        cell.Claim(this);
                    }
                    if (Key(cell) >= Key(anchorCell) + Value && cell.IsOnlyClue(this))
                    {
                        cell.UpdateCell(-1);
                    }
                }
            Cell anchorCell2 = PossCells.LastOrDefault(c => c.Value == 1 && c.IsOnlyClue(this));
            if (anchorCell2 != null)
                foreach (Cell cell in PossCells)
                {
                    if (Key(cell) < Key(anchorCell2) && Key(cell) > End - Value)
                    {
                        cell.UpdateCell(1);
                        cell.Claim(this);
                    }
                    if (Key(cell) <= Key(anchorCell2) - Value && cell.IsOnlyClue(this))
                    {
                        cell.UpdateCell(-1);
                    }
                }
            
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
            if (x != null)
            {
                List<Cell> cells = new List<Cell>();
                cells.Add(x);

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
                for (int i = Key(x) +1; i < OwnerLine.Length; i++)
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

                //Complete if right number of filled cells
                if (cells.Count == Value)
                {
                    Complete(cells);
                }
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
