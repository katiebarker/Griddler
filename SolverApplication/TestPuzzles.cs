using System.Collections.Generic;
using Griddler.PuzzleModel;

namespace Griddler.Solver
{
    internal static class TestPuzzles
    {
        internal static Dictionary<string, Puzzle> Puzzles
        {
            get
            {
                var puzzles = new Dictionary<string, Puzzle>
                {
                    {"5x5 1", new Puzzle(5, 5, "1,1; 5; 1,1; 5; 1,1; 1,1; 5; 1,1; 5; 1,1")},
                    {"5x5 2", new Puzzle(5, 5, "5; 1,1; 1,1; 1,1; 5; 5; 1,1; 1,1; 1,1; 5")},
                    {"5x5 3", new Puzzle(5, 5, "3,1; 1,1; 3; 1,1; 1,3; 2,2; 1,1; 1,1,1; 1,1; 2,2")},
                    {"5x5 4", new Puzzle(5, 5, "1; 3; 5; 1; 1; 1; 2; 5; 2; 1")},
                    {"5x6 1", new Puzzle(5, 6, "5; 5; 5; 5; 5; 5; 6; 6; 6; 6; 6")},
                    {"10x10 1", new Puzzle(10, 10, "2; 1; 3; 3; 3,4; 3,5; 3,5; 2,6; 6; 2,2; 2,6; 1,6; 3,1; 2; 6; 6; 6; 6; 1,3; 1")},
                    {"10x10 2", new Puzzle(10, 10, "1,2,2,1; 1,4; 1,1,4; 1,1,2; 2,1,1; 4,1; 3; 2,1; 2,1,1,1; 3,2,2; 2,7; 1,6 ;1,1,2,1; 1,1,1,2; 1,1; 3,1; 3,2; 3; 3,1; 1,1,2")},
                    {"10x10 3", new Puzzle(10, 10, "1; 4; 2,3; 6; 1,4; 1,1,4; 1,1,3; 1,4; 1,4; 1,6; 4; 3,1; 3,1; 1,2,1; 6,1; 9; 8; 5; 3; 1")}
                };

                return puzzles;
            }
        }
    }
}