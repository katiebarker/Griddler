using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Griddler.PuzzleModel 
{
    public class PuzzleSolver
    {
        private Puzzle puzzle;
        private List<LineSolver> lineSolvers;
        private List<CellSolver> cellSolvers;

        public PuzzleSolver(Puzzle puzzle)
        {
            this.puzzle = puzzle;

            lineSolvers = new List<LineSolver>();
            foreach (Line line in puzzle.Lines)
            {
                var lineSolver = new LineSolver(line);
                lineSolver.OwnerPuzzle = this;
                lineSolvers.Add(lineSolver);
            }
            cellSolvers = new List<CellSolver>();
            foreach (Cell cell in puzzle.Cells)
            {
                var cellSolver = new CellSolver(cell);
                cellSolver.OwnerPuzzle = this;
                cellSolvers.Add(cellSolver);
            }
            foreach (var row in lineSolvers.Where(l => l.line.IsRow))
            {
                foreach (var column in lineSolvers.Where(l => !l.line.IsRow))
                {
                    var cell = cellSolvers.Find(c => c.cell.Row == row.line && c.cell.Column == column.line);
                    row.cellSolvers.Add(cell);
                    foreach (var clue in row.clueSolvers)
                    {
                        clue.PossSections.First().Add(cell);
                    }
                    column.cellSolvers.Add(cell);
                }
            }
        }

        public bool Solve()
        {
            try
            {
                ResetPuzzle();

                foreach (var line in lineSolvers)
                {
                    line.OriginalCalculateFirstLast();
                }

                while (true)
                {
                    Changed = false;

                    foreach (var line in lineSolvers)
                    {
                        line.Solve();
                    }

                    if (Changed)
                    {
                        continue;
                    }
                    break;

                }

            }
            catch (InvalidPuzzleException ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
            return true;
        }

        public void ResetPuzzle()
        {
            foreach (var cell in cellSolvers)
            {
                cell.Reset();
            }
            foreach (var line in lineSolvers)
            {
                line.Reset();
            }
        }
        

        public int NumFilledCells
        {
            get
            {
                int num = 0;
                foreach (var row in lineSolvers.Where(l => l.line.IsRow))
                {
                    num += row.cellSolvers.Count(c => c.cell.Value != 0);
                }
                return num;
            }
        }

        public string ErrorMessage { get; set; }

        public bool Changed { get; set; }
    }
}
