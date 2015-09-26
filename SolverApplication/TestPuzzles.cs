using System.Collections.Generic;
using Griddler.PuzzleModel;

namespace Griddler.Solver
{
    public static class TestPuzzles
    {
        public static Dictionary<string, Puzzle> Puzzles
        {
            get
            {
                var dict = new Dictionary<string, Puzzle>();
                dict.Add("5x5 1", PuzzleFactory.Instance().MakePuzzle(5, 5, "1,1; 5; 1,1; 5; 1,1;", "1,1; 5; 1,1; 5; 1,1"));
                dict.Add("5x5 2", PuzzleFactory.Instance().MakePuzzle(5, 5, "5; 1,1; 1,1; 1,1; 5;", "5; 1,1; 1,1; 1,1; 5"));
                dict.Add("5x5 3", PuzzleFactory.Instance().MakePuzzle(5, 5, "3,1; 1,1; 3; 1,1; 1,3;", "2,2; 1,1; 1,1,1; 1,1; 2,2"));
                dict.Add("5x5 4", PuzzleFactory.Instance().MakePuzzle(5, 5, "1; 3; 5; 1; 1;", "1; 2; 5; 2; 1"));
                dict.Add("5x6 1", PuzzleFactory.Instance().MakePuzzle(5, 6, "5; 5; 5; 5; 5; 5;", "6; 6; 6; 6; 6"));
                dict.Add("10x10 1", PuzzleFactory.Instance().MakePuzzle(10, 10, "2; 1; 3; 3; 3,4; 3,5; 3,5; 2,6; 6; 2,2;", "2,6; 1,6; 3,1; 2; 6; 6; 6; 6; 1,3; 1"));
                dict.Add("10x10 2", PuzzleFactory.Instance().MakePuzzle(10, 10, "1,2,2,1; 1,4; 1,1,4; 1,1,2; 2,1,1; 4,1; 3; 2,1; 2,1,1,1; 3,2,2;"," 2,7; 1,6 ;1,1,2,1; 1,1,1,2; 1,1; 3,1; 3,2; 3; 3,1; 1,1,2"));
                dict.Add("10x10 3", PuzzleFactory.Instance().MakePuzzle(10, 10, "1; 4; 2,3; 6; 1,4; 1,1,4; 1,1,3; 1,4; 1,4; 1,6;", "4; 3,1; 3,1; 1,2,1; 6,1; 9; 8; 5; 3; 1"));
                dict.Add("15x15 1", PuzzleFactory.Instance().MakePuzzle(15, 15, "1,3;3,5;1,2,4,2;4,5,1;3,1,5;3,1,3;4,3;5;3,5;4,3;11;10;11;1,4,4;3,4,4;", "4,1;1,8,2;12,1;12;2,6;2,5;1,5;1,3,1;3,5;3,1,5;5,1,5;7,5;2,4,3;2,2,2;3,1;"));
                dict.Add("30x30 1", PuzzleFactory.Instance().MakePuzzle(30,30, 
                    "4;5;9,6;14,6;22; 10,9;5,2,8;9,11;26;5,11,3; 2,4,3,3;5,4,2,2,2;5,4,3,1;3,11,3,2;3,4,3,2,1; 2,3,2,2,1;1,3,2,3;2,2,2;1,2;2,2,1,2; 4,2,3;2,1,3,1;2,1,3,2;2,2,3,2;4,1,4,2; 4,1,4,3;2,6,3;1,8,2;4,3,1;2,2;",   
                    "2;4;3,1;5,1;6,1,3; 6,5;6,3,2;7,2,2;4,2,1,2;7,2,2,2; 8,1,2,1;4,2,1,2,2;4,3,1,3,2;3,6,2;3,7,3; 3,11;2,3,7,2,3;8,4,4,4;9,1,4;8,2,4; 6,1,4;6,2,5;7,2,2,2,4;7,3,2,3,3;9,3,7; 5,4,3,3,3;4,4,7,3;3,4,4,3;2,4,4;1,3;"
                    ));

                return dict;
            }
        }
    }
}