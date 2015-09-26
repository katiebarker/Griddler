using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Griddler.PuzzleModel
{
    public class ClueSolver
    {
        public Clue clue;

        public ClueSolver(Clue clue)
        {
            this.clue = clue;
            PossSections = new List<List<CellSolver>>();
            PossSections.Add(new List<CellSolver>());
        }

        public void UpdateSections()
        {
            CompleteAny();

            //if (PossSections.Any(l => l.Count < clue.Value) || PossCells.Any(c => c.cell.Value == -1 || c.IsAvaliable(this) == false))
            {
                var sections = new List<List<CellSolver>>();
                var section = new List<CellSolver>();
                foreach (var cell in OwnerLine.cellSolvers)
                {
                    if (PossCells.Contains(cell) && cell.cell.Value != -1 && cell.IsAvaliable(this))
                    {
                        section.Add(cell);
                    }
                    else if (section.Count > 0)
                    {
                        if (section.Count >= clue.Value)
                        {
                            sections.Add(section);
                        }
                        section = new List<CellSolver>();
                    }
                }
                if (section.Count >= clue.Value)
                {
                    sections.Add(section);
                }

                if (sections.Count == 0)
                {
                    throw new InvalidPuzzleException(String.Format("No room for clue: {0} {1}", clue.OwnerLine.Key, clue.OwnerLine.IsRow));
                }

                for (int i = 0; i < sections.Count; i++)
                {
                    if (sections.Count != PossSections.Count)
                    {
                        OwnerLine.OwnerPuzzle.Changed = true;
                        break;
                    }
                    if (!sections[i].SequenceEqual(PossSections[i]))
                    {
                        OwnerLine.OwnerPuzzle.Changed = true;
                        break;
                    }
                }

                PossSections = sections;
                UpdateEnds();
            }
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
                            (cell.cell.Value == 1) &&
                            (cell.IsOnlyClue(this) &&
                            cell.IsAvaliable(this))))
                {
                    if (section.Count == clue.Value)
                    {
                        Complete(section);
                        break;
                    }
                    if (section.Count < clue.Value)
                    {
                        throw new Exception("bad section");
                    }
                    if (section.Count > clue.Value)
                    {
                        PossSections = new List<List<CellSolver>> { section };
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

        private void FillSection(List<CellSolver> section)
        {
            //Normal Fill
            foreach (var cell in section)
            {
                if (clue.Key(cell.cell) < clue.Key(section.First().cell) + clue.Value &&
                    clue.Key(cell.cell) > clue.Key(section.Last().cell) - clue.Value)
                {
                    cell.UpdateCell(1);
                    cell.Claim(this);
                }
            }

            //Extend existing cells
            //TODO: backwards? and update

            var anchorCell = PossCells.FirstOrDefault(c => c.cell.Value == 1 && c.IsOnlyClue(this));
            if (anchorCell != null)
                foreach (var cell in PossCells)
                {
                    if (clue.Key(cell.cell) > clue.Key(anchorCell.cell) && clue.Key(cell.cell) < Start + clue.Value)
                    {
                        cell.UpdateCell(1);
                        cell.Claim(this);
                    }
                    if (clue.Key(cell.cell) >= clue.Key(anchorCell.cell) + clue.Value && cell.IsOnlyClue(this))
                    {
                        cell.UpdateCell(-1);
                    }
                }
            var anchorCell2 = PossCells.LastOrDefault(c => c.cell.Value == 1 && c.IsOnlyClue(this));
            if (anchorCell2 != null)
                foreach (var cell in PossCells)
                {
                    if (clue.Key(cell.cell) < clue.Key(anchorCell2.cell) && clue.Key(cell.cell) > End - clue.Value)
                    {
                        cell.UpdateCell(1);
                        cell.Claim(this);
                    }
                    if (clue.Key(cell.cell) <= clue.Key(anchorCell2.cell) - clue.Value && cell.IsOnlyClue(this))
                    {
                        cell.UpdateCell(-1);
                    }
                }
        }

        public List<CellSolver> PossCells
        {
            get
            {
                var cells = new List<CellSolver>();
                foreach (var section in PossSections)
                {
                    foreach (var cell in section)
                    {
                        cells.Add(cell);
                    }
                }
                return cells.OrderBy(c => c.cell.Key.X).OrderBy(c => c.cell.Key.Y).ToList();
            }
        }


        public List<List<CellSolver>> PossSections { get; set; }



        public CellSolver First
        {
            get { return PossCells.First(); }

            set
            {
                foreach (var sect in PossSections)
                {
                    sect.RemoveAll(cell => OwnerLine.cellSolvers.IndexOf(cell) < OwnerLine.cellSolvers.IndexOf(value));
                }
            }
        }

        public CellSolver Last
        {
            get { return PossCells.Last(); }
            set
            {
                foreach (var section in PossSections)
                {
                    section.RemoveAll(cell => OwnerLine.cellSolvers.IndexOf(cell) > OwnerLine.cellSolvers.IndexOf(value));
                }
            }
        }

        public CellSolver FirstEnd
        {
            get
            {
                return OwnerLine.cellSolvers.ElementAt(clue.Key(First.cell) + clue.Value);
            }
        }

        public CellSolver LastEnd
        {
            get
            {
                return OwnerLine.cellSolvers.ElementAt(clue.Key(Last.cell) - clue.Value);
            }
        }

        public int Start
        {
            get { return clue.Key(First.cell); }
            set
            {
                if (value >= 0 && value < OwnerLine.line.Length)
                    First = OwnerLine.cellSolvers[value];
            }
        }

        public int End
        {
            get { return clue.Key(Last.cell) + 1; }
            set
            {
                if (value >= 0 && value < OwnerLine.line.Length)
                    Last = OwnerLine.cellSolvers[value - 1];
            }
        }

        public bool IsComplete { get; private set; }

        public void UpdateEnds()
        {
            if (PreviousClue != null)
                First = OwnerLine.cellSolvers[clue.Key(PreviousClue.FirstEnd.cell) + 1];
            if (NextClue != null)
                Last = OwnerLine.cellSolvers[clue.Key(NextClue.LastEnd.cell) - 1];

        }


        public void CompleteAny()
        {
            //right number of poss cells
            if (End - Start == clue.Value)
            {
                Complete(PossCells);
                return;
            }

            //Find filled confirmed clue + all filled consecutive
            var x = PossCells.FirstOrDefault(c => (c.cell.Value == 1 && c.IsAvaliable(this) && c.IsOnlyClue(this)));
            if (x == null) return;

            var cells = new List<CellSolver>();
            cells.Add(x);

            //Consequetive cells to left
            for (int i = clue.Key(x.cell) - 1; i > 0; i--)
            {
                var cell = PossCells.SingleOrDefault(c => clue.Key(c.cell) == i);
                if (cell != null && cell.cell.Value == 1)
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
            for (int i = clue.Key(x.cell) + 1; i < OwnerLine.line.Length; i++)
            {
                var cell = PossCells.SingleOrDefault(c => clue.Key(c.cell) == i);
                if (cell != null && cell.cell.Value == 1)
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
            Start = cells.Last().cell.GetKey(OwnerLine.line) - clue.Value;
            End = cells.First().cell.GetKey(OwnerLine.line) + clue.Value;

            //Complete if right number of filled cells
            if (cells.Count == clue.Value)
            {
                Complete(cells);
            }

        }

        public void Complete(IEnumerable<CellSolver> finalCells)
        {
            finalCells = finalCells.OrderBy(c => c.cell.Key.X).OrderBy(c => c.cell.Key.Y);
            int first = clue.Key(finalCells.First().cell);
            int last = clue.Key(finalCells.Last().cell);
            foreach (var cell in finalCells)
            {
                cell.UpdateCell(1);
                cell.Claim(this);
            }
            if (first > 0) OwnerLine.cellSolvers[first - 1].UpdateCell(-1);
            if (last + 1 < OwnerLine.line.Length) OwnerLine.cellSolvers[last + 1].UpdateCell(-1);
            IsComplete = true;
        }

        public void ClaimAny()
        {
            foreach (var cell in PossCells.Where(cell => cell.IsOnlyClue(this) && cell.cell.Value == 1))
            {
                cell.Claim(this);
            }
        }

        public void Reset()
        {
            PossSections = new List<List<CellSolver>>();
            PossSections.Add(new List<CellSolver>(OwnerLine.cellSolvers));
            IsComplete = false;
        }

        internal ClueSolver PreviousClue
        {
            get
            {
                return OwnerLine.clueSolvers.ElementAtOrDefault(clue.Index - 1);
            }
        }

        public ClueSolver NextClue
        {
            get
            {
                return OwnerLine.clueSolvers.ElementAtOrDefault(clue.Index + 1);
            }
        }

        public LineSolver OwnerLine { get; set; }
    }
}
