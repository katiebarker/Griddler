using System.Collections.Generic;
using System;

namespace Griddler.PuzzleModel
{
    public interface IPuzzle
    {
        int Width { get; }

        int Height { get; }

        string GetRowClues(int row);

        string GetColumnClues(int col);
    }
}
