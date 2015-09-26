using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Griddler.PuzzleModel
{
    public interface ICell
    {
        Point Key { get; }
        string KeyString { get; }
        int Value { get; set; }
    }
}
