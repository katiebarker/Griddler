using Griddler.PuzzleModel;
using Griddler.Solver;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Griddler.Solver
{
    public partial class SolverWebApp : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (PuzzleDropDown.Items.Count == 0)
            {
                List<ListItem> list = new List<ListItem>();
                foreach (var item in TestPuzzles.Puzzles.Keys)
                {
                    list.Add(new ListItem(item));
                }

                PuzzleDropDown.Items.AddRange(list.ToArray());                
            }
        }

        protected Table aspTable;

        protected void SolveButton_Click(object sender, EventArgs e)
        {
            //try
            //{
                Puzzle puzzle;
                if (!TestPuzzles.Puzzles.TryGetValue(PuzzleDropDown.Text, out puzzle))
                {
                    throw new InvalidPuzzleException("Select Valid Puzzle from list");
                }
                else
                {
                    puzzle.Solve();

                    UpdateDisplay(puzzle);
                }
            //}
            //catch (InvalidPuzzleException ex)
            //{
            //     MessageBox.Show(ex.Message);
            //}
        }

        private void UpdateDisplay(Puzzle puzzle)
        {
            foreach (var row in puzzle.Rows)
            {
                var aRow = new TableRow();
                aspTable.Rows.Add(aRow);

                foreach (var cell in row.Cells)
                {
                    var aCell = new TableCell();
                    aRow.Cells.Add(aCell);

                    switch (puzzle.Cells[cell.Key.X,cell.Key.Y].Value)
                    {
                        case 1:
                            aCell.BackColor = Color.Black;
                            break;
                        case -1:
                            aCell.BackColor = Color.LightGray;
                            break;
                        case 0:
                            aCell.BackColor = Color.White;
                            break;
                        default:
                            throw new Exception("Wrong Cell Value");
                    }
                }

            }
        }
    }
}