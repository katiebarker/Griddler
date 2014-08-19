using System;
using System.Collections.Generic;
using System.Linq;

namespace Griddler.PuzzleModel
{
    public class Puzzle : IPuzzle
    {
        public Puzzle(int width, int height, List<Line> lines)
        {
            Width = width;
            Height = height;
            Lines = lines;
            Cells = new Cell[Width, Height];

            var rows = new List<Row>(lines.OfType<Row>());
            var columns = new List<Column>(lines.OfType<Column>());

            foreach (Row row in rows)
            {
                row.OwnerPuzzle = this;
                foreach (Column column in columns)
                {
                    column.OwnerPuzzle = this;
                    var cell = new Cell(row, column);
                    Cells[column.Key, row.Key] = cell;
                    row.AddCell(cell);
                    column.AddCell(cell);
                }
            }
            CheckTotal();
        }

        public Puzzle(int width, int height, string rowClues, string colClues)
            : this(width, height, StringToList(rowClues),StringToList(colClues))
        {
        }

        public Puzzle(int width, int height, List<int[]> rowClues, List<int[]> colClues)
            : this(width, height, ToLines(width, height, rowClues,colClues))
        {
        }

        private static List<Line> ToLines(int width, int height, List<int[]> rowClues, List<int[]> colClues)
        {
            if (colClues.Count != width && rowClues.Count != height)
            {
                throw new Exception("Invalid clues for puzzle");
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

        
        
        public string GetRowClues(int row)
        {
            var clueValues = Lines[row].Clues.Select(c => c.Value.ToString()).ToList();
            var clues = string.Join(", ",clueValues);            
            return clues;
        }

        public string GetColumnClues(int col)
        {
            var clueValues = Lines[col + Height].Clues.Select(c => c.Value.ToString()).ToList();
            var clues = string.Join("\n",clueValues);            
            return clues;
        }

        private void CheckTotal()
        {
            var rowTotal = 0;
            var colTotal = 0;

            foreach (Line line in Lines)
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

        public Cell[,] Cells {get; private set; }

        public int Height { get; private set; }

        public int Width { get; private set; }

        public List<Line> Lines { get; private set; }

        public void Solve()
        {
            ResetPuzzle();

            foreach (var line in Lines)
            {
                line.OriginalCalculateFirstLast();
            }

            while (true)
            {
                Changed = false;

                foreach (Line line in Lines)
                {
                    line.Solve();
                }

                if (Changed)
                {
                    continue;
                }
                break;
            }
        }

        private void ResetPuzzle()
        {
            foreach (Cell cell in Cells)
            {
                cell.Reset();
            }
            foreach (Line line in Lines)
            {
                line.Reset();
            }
        }

        public bool Changed { get; set; }
    }
}
