using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Griddler.PuzzleModel
{
    public class Cell
    {
        public string KeyString
        {
            get
            {
                return String.Format("Cell: ({0}, {1})", (Key.X + 1), (Key.Y + 1));
            }
        }

        public Point Key { get; set; }
        
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

        public Row Row { get; set; }

        public Column Column { get; set; }
        
        public int Value
        {
            get;
            set;
        }
    }
}
