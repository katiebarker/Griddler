using System;
using System.Collections.Generic;
using System.Linq;
using Griddler.PuzzleModel.Basic;

namespace Griddler.PuzzleModel
{
    public class Puzzle : IPuzzle
    {

        public Cell[,] Cells {get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public List<Line> Lines { get; set; }

        public string ErrorMessage { get; set; }

        public bool Changed { get; set; }

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

        public void ResetPuzzle()
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

        public int NumFilledCells
        {
            get
            {
                int num = 0;
                foreach (var row in Lines.OfType<Row>())
                {
                    num += row.Cells.Count(c => c.Value != 0);
                }
                return num;
            }
        }
    }
}
