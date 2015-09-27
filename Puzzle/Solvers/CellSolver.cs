using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Griddler.PuzzleModel
{
    public class CellSolver 
    {
        public Pair<ClueSolver> ConfirmedClues;
        public Cell cell;
        public PuzzleSolver OwnerPuzzle;

        public CellSolver(Cell cell)
        {
            this.cell = cell;

            ConfirmedClues = new Pair<ClueSolver>(null, null);
        }

        private List<ClueSolver> FindClues(LineSolver line)
        {
            return line.OpenClues.Where(clue => clue.PossCells.Contains(this)).ToList();
        }

        internal bool IsOnlyClue(ClueSolver clue)
        {
            var list = FindClues(clue.OwnerLine);
            return list.Contains(clue) && list.Count == 1;
        }

        internal ClueSolver GetConfirmedClue(LineSolver line)
        {
            if (line.line == cell.Row)
            {
                return ConfirmedClues.X;
            }
            if (line.line == cell.Column)
            {
                return ConfirmedClues.Y;
            }
            throw new Exception("Not a valid owner line");
        }

        internal bool IsAvaliable(ClueSolver clue)
        {
            return (GetConfirmedClue(clue.OwnerLine) == clue || GetConfirmedClue(clue.OwnerLine) == null);
        }

        internal void Claim(ClueSolver clue)
        {
            if (clue.OwnerLine.line == cell.Row)
            {
                if (ConfirmedClues.X == null) OwnerPuzzle.Changed = true;
                ConfirmedClues.X = clue;
            }
            if (clue.OwnerLine.line == cell.Column)
            {
                if (ConfirmedClues.Y == null) OwnerPuzzle.Changed = true;
                ConfirmedClues.Y = clue;
            }
        }
        
        public void UpdateCell(int newValue)
        {
            if (newValue == 0)
            {
                return;
            }
            if ( cell.Value == 0)
            {
                 cell.Value = newValue;
                OwnerPuzzle.Changed = true;
            }
            else
            {
                if ( cell.Value != newValue)
                {
                    throw new InvalidPuzzleException(String.Format("Conflict of Solution: ({0}, {1})", cell.Key.X + 1, cell.Key.Y + 1));
                }
            }
        }

        internal void Reset()
        {
             cell.Value = 0;
        }

    }
}
