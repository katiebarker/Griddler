using System;
using System.Collections.Generic;
using System.Linq;

namespace Griddler.PuzzleModel
{
    public interface IClue
    {
        int Value { get; }
        
        string KeyString { get; }
        
    }
}
