using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Griddler.PuzzleModel
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
            Puzzle puzzle = new Puzzle(width, height);
            
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
                    var cell = MakeCell(row, column);
                    puzzle.Cells[column.Key, row.Key] = cell;
                    AddCell(row, cell);
                    AddCell(column, cell);
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

        public static List<int[]> StringToList(string clues)
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

        private List<Line> ToLines(int width, int height, List<int[]> rowClues, List<int[]> colClues)
        {
            if (colClues.Count != width && rowClues.Count != height)
            {
                throw new InvalidPuzzleException("Invalid clues for puzzle");
            }
            var lines = new List<Line>();
            for (var i = 0; i < height; i++)
            {
                var row = MakeLine<Row>(rowClues[i].ToList(), i);
                lines.Add(row);
            }
            for (var i = 0; i < width; i++)
            {
                lines.Add(MakeLine<Column>(colClues[i].ToList(), i));
            }
            return lines;
        }

        public Line MakeLine<T>(IEnumerable<int> clues, int key) where T : Line, new()
        {
            T line = new T();
            line.Key = key;
            line.Cells = new List<Cell>();
            line.Clues = new List<Clue>();
            foreach (var clue in clues)
            {
                line.Clues.Add(new Clue(clue, line));
            }

            return line;
        }        

        public void AddCell(Line line, Cell cell)
        {
            line.Cells.Add(cell);
        }

        private Cell MakeCell(Row row, Column column)
        {
            var cell = new Cell();

            cell.Row = row;
            cell.Column = column;
            cell.Key = new Point(cell.Column.Key, cell.Row.Key);

            if (cell.Row.OwnerPuzzle != cell.Column.OwnerPuzzle)
            {
                throw new Exception("Incompatible Row and Column");
            }


            return cell;
        }
    }
}
