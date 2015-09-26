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
        }

        public Line OwnerLine { get; private set; }
        public int Value { get; private set; }

        public int Index
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
    }
}
