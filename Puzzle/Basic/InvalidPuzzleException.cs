using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Griddler.PuzzleModel
{
    public class InvalidPuzzleException : Exception
    {
        public InvalidPuzzleException(string message) : base(message)
        {
       
        }
    }
}
