using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Griddler.PuzzleModel
{
    public class Solver
    {
        private static Solver instance;
        public static Solver Instance()
        {
            if (instance == null)
            {
                instance = new Solver();
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
                clue.UpdateSections();
            }
        }

        private void SolveFillSections(Line line)
        {
            foreach (Clue clue in line.OpenClues)
            {
                clue.FillSections();
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

    }
}
