using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Griddler.PuzzleModel
{
    public class Cell : ICell
    {
        public Cell(Row row, Column column)
        {
            Row = row;
            Column = column;
            Key = new Point(Column.Key, Row.Key);

            if (Row.OwnerPuzzle != Column.OwnerPuzzle)
            {
                throw new Exception("Incompatible Row and Column");
            }

            ConfirmedClues = new Pair<Clue>(null, null);
        }

        public int Value { get; private set; }

        public string KeyString
        {
            get
            {
                return String.Format("Cell: ({0}, {1})", (Key.X + 1), (Key.Y + 1));
            }
        }

        public Point Key { get; private set; }

        protected Puzzle OwnerPuzzle
        {
            get
            {
                return Row.OwnerPuzzle;
            }
        }

        public int GetKey(Line line)
        {
            if (line == Row)
            {
                return Key.X;
            }
            if (line == Column)
            {
                return Key.Y;
            }
            throw new Exception("Not a valid owner line");
        }

        public Row Row { get; private set; }

        public Column Column { get; private set; }

        private readonly Pair<Clue> ConfirmedClues;

        internal void UpdateCell(int newValue)
        {
            if (newValue == 0)
            {
                return;
            }
            if (Value == 0)
            {
                Value = newValue;
                OwnerPuzzle.Changed = true;
            }
            else
            {
                if (Value != newValue)
                {
                    throw new Exception(String.Format("Conflict of Solution: ({0}, {1})", Key.X + 1, Key.Y + 1));
                }
            }
        }

        private List<Clue> FindClues(Line line)
        {
            return line.OpenClues.Where(clue => clue.PossCells.Contains(this)).ToList();
        }

        internal bool IsOnlyClue(Clue clue)
        {
            var list = FindClues(clue.OwnerLine);
            return list.Contains(clue) && list.Count == 1;
        }

        internal Clue GetConfirmedClue(Line line)
        {
            if (line == Row)
            {
                return ConfirmedClues.X;
            }
            if (line == Column)
            {
                return ConfirmedClues.Y;
            }
            throw new Exception("Not a valid owner line");
        }

        internal bool IsAvaliable(Clue clue)
        {
            return (GetConfirmedClue(clue.OwnerLine) == clue || GetConfirmedClue(clue.OwnerLine) == null);
        }

        internal void Claim(Clue clue)
        {
            if (clue.OwnerLine == Row)
            {
                if (ConfirmedClues.X == null) OwnerPuzzle.Changed = true;
                ConfirmedClues.X = clue;
            }
            if (clue.OwnerLine == Column)
            {
                if (ConfirmedClues.Y == null) OwnerPuzzle.Changed = true;
                ConfirmedClues.Y = clue;
            }
        }

        internal void Reset()
        {
            Value = 0;
        }
    }
}
