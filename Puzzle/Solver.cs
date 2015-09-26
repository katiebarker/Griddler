using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Griddler.PuzzleModel
{
    public class PuzzleSolver
    {
        private static PuzzleSolver instance;
        public static PuzzleSolver Instance()
        {
            if (instance == null)
            {
                instance = new PuzzleSolver();
            }

            return instance;
        }

        public bool Solve(Puzzle puzzle)
        {
            try
            {
                //puzzle.Solve();

                puzzle.ResetPuzzle();

                foreach (var line in puzzle.Lines)
                {
                    OriginalCalculateFirstLast(line);
                }

                while (true)
                {
                    puzzle.Changed = false;

                    foreach (Line line in puzzle.Lines)
                    {
                        Solve(line);
                    }

                    if (puzzle.Changed)
                    {
                        continue;
                    }
                    break;

                }

            }
            catch (InvalidPuzzleException ex)
            {
                puzzle.ErrorMessage = ex.Message;
                return false;
            }
            return true;
        }

        public void Solve(Line line)
        {
            SolveUpdateSections(line);
            SolveFillSections(line);
            SolveFillDots(line);
            SolveCheckComplete(line);
            CheckSingular(line);
        }

        private void OriginalCalculateFirstLast(Line line)
        {
            var start = 0;
            var end = line.Length - 1;
            foreach (Clue clue in line.OpenClues)
            {
                clue.First = line.Cells[start];
                start += clue.Value + 1;
            }
            foreach (Clue clue in line.OpenClues.AsEnumerable().Reverse())
            {
                clue.Last = line.Cells[end];
                end -= clue.Value + 1;
            }
        }

        private void SolveUpdateSections(Line line)
        {
            foreach (Clue clue in line.OpenClues)
            {
                ClueSolver.Instance().UpdateSections(clue);
            }
        }

        private void SolveFillSections(Line line)
        {
            foreach (Clue clue in line.OpenClues)
            {
                ClueSolver.Instance().FillSections(clue);
            }
        }

        private void SolveFillDots(Line line)
        {
            var cells = new List<Cell>();
            //Find all blank cells which have a possible clue
            foreach (Clue clue in line.Clues.Where(c => c.PossSections.Count != 0))
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
            //Fill all blank cells with no possible clue
            foreach (Cell cell in line.Cells.Where(cell => !cells.Contains(cell) && cell.Value == 0))
            {
                cell.UpdateCell(-1);
            }
        }

        private void SolveCheckComplete(Line line)
        {
            foreach (Clue clue in line.OpenClues)
            {
                clue.CompleteAny();
            }
        }

        private void CheckSingular(Line line)
        {
            foreach (var clue in line.OpenClues)
            {
                clue.ClaimAny();
            }
        }

        private class ClueSolver
        {
            private static ClueSolver instance;
            public static ClueSolver Instance()
            {
                if (instance == null)
                {
                    instance = new ClueSolver();
                }
                return instance;
            }

            public void UpdateSections(Clue clue)
            {
                clue.CompleteAny();

                //if (PossSections.Any(l => l.Count < Value) || PossCells.Any(c => c.Value == -1 || c.IsAvaliable(this) == false))
                {
                    var sections = new List<List<Cell>>();
                    var section = new List<Cell>();
                    foreach (var cell in clue.OwnerLine.Cells)
                    {
                        if (clue.PossCells.Contains(cell) && cell.Value != -1 && cell.IsAvaliable(clue))
                        {
                            section.Add(cell);
                        }
                        else if (section.Count > 0)
                        {
                            if (section.Count >= clue.Value)
                            {
                                sections.Add(section);
                            }
                            section = new List<Cell>();
                        }
                    }
                    if (section.Count >= clue.Value)
                    {
                        sections.Add(section);
                    }

                    if (sections.Count == 0)
                    {
                        throw new Exception(String.Format("No room for clue: {0} {1}", clue.OwnerLine.Key, clue.OwnerLine.IsRow));
                    }

                    for (int i = 0; i < sections.Count; i++)
                    {
                        if (sections.Count != clue.PossSections.Count)
                        {
                            clue.OwnerLine.OwnerPuzzle.Changed = true;
                            break;
                        }
                        if (!sections[i].SequenceEqual(clue.PossSections[i]))
                        {
                            clue.OwnerLine.OwnerPuzzle.Changed = true;
                            break;
                        }
                    }

                    clue.PossSections = sections;
                    clue.UpdateEnds();
                }
            }

            public void FillSections(Clue clue)
            {
                UpdateSections(clue);
                foreach (var section in clue.PossSections)
                {
                    //Section which contains definite filled cell
                    if (
                        section.Any(
                            cell =>
                                (cell.Value == 1) &&
                                (cell.IsOnlyClue(clue) &&
                                cell.IsAvaliable(clue))))
                    {
                        if (section.Count == clue.Value)
                        {
                            clue.Complete(section);
                            break;
                        }
                        if (section.Count < clue.Value)
                        {
                            throw new Exception("bad section");
                        }
                        if (section.Count > clue.Value)
                        {
                            clue.PossSections = new List<List<Cell>> { section };
                            FillSection(clue, section);
                        }

                    }
                    //If only one section
                    if (clue.PossSections.Count == 1)
                    {
                        FillSection(clue, section);
                    }
                }
            }

            private void FillSection(Clue clue, List<Cell> section)
            {
                //Normal Fill
                foreach (var cell in section)
                {
                    if (clue.Key(cell) < clue.Key(section.First()) + clue.Value &&
                        clue.Key(cell) > clue.Key(section.Last()) - clue.Value)
                    {
                        cell.UpdateCell(1);
                        cell.Claim(clue);
                    }
                }

                //Extend existing cells
                //TODO: backwards? and update

                Cell anchorCell = clue.PossCells.FirstOrDefault(c => c.Value == 1 && c.IsOnlyClue(clue));
                if (anchorCell != null)
                    foreach (Cell cell in clue.PossCells)
                    {
                        if (clue.Key(cell) > clue.Key(anchorCell) && clue.Key(cell) < clue.Start + clue.Value)
                        {
                            cell.UpdateCell(1);
                            cell.Claim(clue);
                        }
                        if (clue.Key(cell) >= clue.Key(anchorCell) + clue.Value && cell.IsOnlyClue(clue))
                        {
                            cell.UpdateCell(-1);
                        }
                    }
                Cell anchorCell2 = clue.PossCells.LastOrDefault(c => c.Value == 1 && c.IsOnlyClue(clue));
                if (anchorCell2 != null)
                    foreach (Cell cell in clue.PossCells)
                    {
                        if (clue.Key(cell) < clue.Key(anchorCell2) && clue.Key(cell) > clue.End - clue.Value)
                        {
                            cell.UpdateCell(1);
                            cell.Claim(clue);
                        }
                        if (clue.Key(cell) <= clue.Key(anchorCell2) - clue.Value && cell.IsOnlyClue(clue))
                        {
                            cell.UpdateCell(-1);
                        }
                    }
            }
        }
    }
}
