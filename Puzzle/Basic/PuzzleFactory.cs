using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Griddler.PuzzleModel.Basic
{
    public class PuzzleFactory
    {
        private static PuzzleFactory instance;

        public static PuzzleFactory Instance()
        {
            if (instance == null)
            {
                instance = new PuzzleFactory();
            }

            return instance;
        }

        public Puzzle MakePuzzle(int width, int height, List<Line> lines)
        {
            Puzzle puzzle = new Puzzle();
            puzzle.Width = width;
            puzzle.Height = height;
            puzzle.Lines = lines;
            puzzle.Cells = new Cell[width, height];

            var rows = new List<Row>(lines.OfType<Row>());
            var columns = new List<Column>(lines.OfType<Column>());

            foreach (Row row in rows)
            {
                row.OwnerPuzzle = puzzle;
                foreach (Column column in columns)
                {
                    column.OwnerPuzzle = puzzle;
                    var cell = new Cell(row, column);
                    puzzle.Cells[column.Key, row.Key] = cell;
                    row.AddCell(cell);
                    column.AddCell(cell);
                }
            }
            CheckTotal(puzzle);

            return puzzle;
        }

        public Puzzle MakePuzzle(int width, int height, string rowClues, string colClues)
        {
            return MakePuzzle(width, height, StringToList(rowClues),StringToList(colClues));        
        }

        public Puzzle MakePuzzle(int width, int height, List<int[]> rowClues, List<int[]> colClues)
        {
            return MakePuzzle(width, height, ToLines(width, height, rowClues, colClues));
        }

        private void CheckTotal(Puzzle puzzle)
        {
            var rowTotal = 0;
            var colTotal = 0;

            foreach (Line line in puzzle.Lines)
            {
                if (line.IsRow)
                {
                    rowTotal += line.Total;
                }
                else
                {
                    colTotal += line.Total;
                }
            }

            if (rowTotal != colTotal)
            {
                throw new Exception("Invalid Puzzle");
            }
        }

        private static List<int[]> StringToList(string clues)
        {
            var clueList = new List<int[]>();
            var clueStrings = clues.Split(';');
            foreach (var clueString in clueStrings)
            {
                if (clueString != string.Empty)
                {
                    var stringClues = clueString.Split(',');

                    var intClues = new int[stringClues.Length];

                    for (var i = 0; i < stringClues.Length; i++)
                    {
                        int clue;
                        if (int.TryParse(stringClues[i].Trim(), out clue))
                        {
                            intClues[i] = clue;
                        }
                        else
                        {
                            throw new ApplicationException("String Parse Error");
                        }
                    }
                    clueList.Add(intClues);
                }
            }
            return clueList;
        }

        private static List<Line> ToLines(int width, int height, List<int[]> rowClues, List<int[]> colClues)
        {
            if (colClues.Count != width && rowClues.Count != height)
            {
                throw new InvalidPuzzleException("Invalid clues for puzzle");
            }
            var lines = new List<Line>();
            for (var i = 0; i < height; i++)
            {
                lines.Add(new Row(rowClues[i].ToList(), i));
            }
            for (var i = 0; i < width; i++)
            {
                lines.Add(new Column(colClues[i].ToList(), i));
            }
            return lines;
        }

    }
}
