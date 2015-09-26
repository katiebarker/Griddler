using System;
using System.Collections.Generic;
using System.Linq;

namespace Griddler.PuzzleModel
{
    public class Puzzle : IPuzzle
    {
        public Puzzle(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public Cell[,] Cells {get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public List<Line> Lines { get; set; }

        public List<Row> Rows
        {
            get
            {
                return Lines.OfType<Row>().ToList();
            }
        }

        public List<Column> Columns
        {
            get
            {
                return Lines.OfType<Column>().ToList();
            }
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
    }
}
