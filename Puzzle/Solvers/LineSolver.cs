using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Griddler.PuzzleModel
{
    public class LineSolver
    {
        public Line line;
        public List<ClueSolver> clueSolvers;
        public List<CellSolver> cellSolvers;

        public LineSolver(Line line)
        {
            // TODO: Complete member initialization
            this.line = line;

            cellSolvers = new List<CellSolver>();
            foreach (var cell in line.Cells)
            {
                
            }

            clueSolvers = new List<ClueSolver>();
            foreach (var clue in line.Clues)
            {
                var clueSolver = new ClueSolver(clue);
                clueSolver.OwnerLine = this;
                clueSolvers.Add(clueSolver);
            }
        }

        public void Solve()
        {
            SolveUpdateSections();
            SolveFillSections();
            SolveFillDots();
            SolveCheckComplete();
            CheckSingular();
        }

        public void OriginalCalculateFirstLast()
        {
            var start = 0;
            var end = line.Length - 1;
            foreach (var clue in OpenClues)
            {
                clue.First = cellSolvers[start];
                start += clue.clue.Value + 1;
            }
            foreach (var clue in OpenClues.AsEnumerable().Reverse())
            {
                clue.Last = cellSolvers[end];
                end -= clue.clue.Value + 1;
            }
        }

        private void SolveUpdateSections()
        {
            foreach (var clue in clueSolvers.Where(c => c.IsComplete == false) /*.OpenClues*/)
            {
                clue.UpdateSections();
            }
        }

        private void SolveFillSections()
        {
            foreach (var clue in clueSolvers.Where(c => c.IsComplete == false) /*.OpenClues*/)
            {
                clue.FillSections();
            }
        }

        private void SolveFillDots()
        {
            var cells = new List<CellSolver>();
            //Find all blank cells which have a possible clue
            foreach (var clue in clueSolvers.Where(c => c.PossSections.Count != 0))
            {
                foreach (var section in clue.PossSections)
                {
                    foreach (var cell in section)
                    {
                        if (!cells.Contains(cell) && cell.cell.Value == 0)
                        {
                            cells.Add(cell);
                        }
                    }
                }
            }
            //Fill all blank cells with no possible clue
            foreach (var cell in cellSolvers.Where(cell => !cells.Contains(cell) && cell.cell.Value == 0))
            {
                cell.UpdateCell(-1);
            }
        }

        private void SolveCheckComplete()
        {
            foreach (var clue in OpenClues)
            {
                clue.CompleteAny();
            }
        }

        private void CheckSingular()
        {
            foreach (var clue in OpenClues)
            {
                clue.ClaimAny();
            }
        }

        public void Reset()
        {
            foreach (var clue in clueSolvers)
            {
                clue.Reset();
            }
        }
        
        public List<ClueSolver> OpenClues
        {
            get
            {
                return new List<ClueSolver>(clueSolvers.Where(c => c.IsComplete == false));
            }
        }


        public PuzzleSolver OwnerPuzzle { get; set; }
    }
}
