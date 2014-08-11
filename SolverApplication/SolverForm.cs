using Griddler.PuzzleModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Griddler.Solver
{
    public partial class SolverForm : Form
    {
        private const int gridSize = 20;
        private readonly List<Shape> gridLines;
        private readonly List<Label> labels;
        private Dictionary<Point, PictureBox> boxes;
        private Dictionary<int, TextBox> rowTextBoxs;
        private Dictionary<int, TextBox> columnTextBoxs;
        private Dictionary<string, Puzzle> testPuzzles;
        private int numberOfColumns;
        private int numberOfRows;

        public SolverForm()
        {
            numberOfRows = 5;
            numberOfColumns = 5;
            rowTextBoxs = new Dictionary<int, TextBox>();
            columnTextBoxs = new Dictionary<int, TextBox>();

            for (int i = 0; i < numberOfRows; i++)
            {
                AddTextBox(true, i);
            }

            for (int i = 0; i < numberOfColumns; i++)
            {
                AddTextBox(false, i);
            }

            boxes = new Dictionary<Point, PictureBox>();
            gridLines = new List<Shape>();
            labels = new List<Label>();

            InitializeComponent();

            testPuzzles = TestPuzzles.Puzzles;

            chooseTest.Items.AddRange(testPuzzles.Keys.ToArray());
        }

        #region Puzzle Display
        private void UpdateDisplay(Puzzle puzzle)
        {
            RemoveGrid();

            for (int row = 0; row < puzzle.Height; row++)
            {
                AddGridLine(puzzle, true, row);
                AddLabel(puzzle, true, row);
                for (int col = 0; col < puzzle.Width; col++)
                {
                    AddGridLine(puzzle, false, col);
                    AddLabel(puzzle, false, col);
                    var p = new Point(row, col);
                    AddBox(p);

                    switch (puzzle.Cells[col, row].Value)
                    {
                        case 1:
                            boxes[p].BackColor = Color.Black;
                            break;
                        case -1:
                            boxes[p].BackColor = Color.LightGray;
                            break;
                        case 0:
                            boxes[p].BackColor = Color.White;
                            break;
                        default:
                            throw new Exception("Wrong Cell Value");
                    }
                }
                AddGridLine(puzzle, true, puzzle.Height);
                AddGridLine(puzzle, false, puzzle.Width);
            }
        }

        private void AddLabel(Puzzle puzzle, bool isRow, int num)
        {
            string type = isRow ? "hori" : "vert";
            int x = isRow ? 400 : 400 + gridSize * num;
            int y = isRow ? 100 + gridSize * num : 100;
            string name = String.Format("{0}Label{1}", type, num);

            var clues = isRow ? puzzle.GetRowClues(num) : puzzle.GetColumnClues(num);

            Label label;

            if (Controls.ContainsKey(name))
            {
                label = (Label)Controls[name];
                label.Show();
            }
            else
            {
                label = new Label
                {
                    Text = clues,
                    Location = new Point(x, y),
                    Name = name,
                    Font = new Font("Microsoft Sans Serif", 7),

                };
                Controls.Add(label);
            }
            label.Text = clues;
            label.AutoSize = true;
            labels.Add(label);
            label.Location = isRow ? new Point(x - label.Width - 5, y + 3) : new Point(x + 3, y - label.Height - 5);
        }

        private void AddGridLine(Puzzle puzzle, bool isRow, int num)
        {
            var p = new Point(400, 100);
            var s = new Size(1, 1);
            if (isRow)
            {
                p.Y += gridSize * num;
                s.Width += puzzle.Width * gridSize;
            }
            if (!isRow)
            {
                p.X += gridSize * num;
                s.Height += puzzle.Height * gridSize;
            }

            AddGridLine(p, s);
        }

        private void AddGridLine(Point location, Size size)
        {
            if (size.Height != 1 && size.Width != 1)
            {
                throw new Exception("Not a line");
            }
            string type = size.Height == 1 ? "hori" : "vert";
            string name = String.Format("{0}Gridline_{1}_{2}", type, location.X, location.Y);
            if (Controls.ContainsKey(name))
            {
                Controls[name].Show();
                gridLines.Add((Shape)Controls[name]);
                Controls[name].Size = size;
            }
            else
            {
                var gridLine = new Shape
                {
                    Location = location,
                    Name = name,
                    ShapeColor = Color.Black,
                    Size = size
                };

                Controls.Add(gridLine);
                gridLines.Add(gridLine);
            }
        }

        private void RemoveGrid()
        {
            foreach (var square in boxes)
            {
                square.Value.Hide();
            }
            foreach (Shape shape in gridLines)
            {
                shape.Hide();
            }
            foreach (var label in labels)
            {
                label.Hide();
            }

            gridLines.Clear();
            boxes.Clear();
        }

        private void AddBox(Point p)
        {
            string name = String.Format("square_{0}_{1}", p.X, p.Y);
            if (Controls.ContainsKey(name))
            {
                Controls[name].Show();
                boxes.Add(p, (PictureBox)Controls[name]);
            }
            else
            {
                var square = new PictureBox();
                boxes.Add(p, square);

                int x = 402 + gridSize * p.Y;
                int y = 102 + gridSize * p.X;

                square.BackColor = Color.White;
                square.Location = new Point(x, y);
                square.Name = name;
                square.Size = new Size(gridSize - 3, gridSize - 3);
                square.TabStop = false;

                Controls.Add(square);
            }
        }

        private void AddTextBox(bool isRow, int num)
        {
            string type = isRow ? "row" : "column";
            Dictionary<int, TextBox> dict = isRow ? rowTextBoxs : columnTextBoxs;
            int x = isRow ? 47 : 188;
            int y = 120 + 25 * num;
            string name = String.Format("{0}TextBox{1}", type, num);

            if (Controls.ContainsKey(name))
            {
                Controls[name].Show();
                dict.Add(num, Controls[name] as TextBox);
            }
            else
            {
                var textBox = new TextBox();
                dict.Add(num, textBox);

                textBox.Location = new Point(x, y);
                textBox.Name = name;
                textBox.Size = new Size(100, 20);

                Controls.Add(textBox);
            }
        }

        private void RemoveTextBox(bool isRow, int num)
        {
            Dictionary<int, TextBox> dict = isRow ? rowTextBoxs : columnTextBoxs;
            dict[num].Hide();
            dict.Remove(num);
        } 
        #endregion

        #region Events
        private void solveButton_Click(object sender, EventArgs e)
        {
            //try
            //{
            var lineString = "";
            foreach (var box in rowTextBoxs)
            {
                lineString += box.Value.Text + ";";
            }
            foreach (var box in rowTextBoxs)
            {
                lineString += box.Value.Text + ";";
            }

            var puzzle = new Puzzle(numberOfRows, numberOfColumns, lineString);

            puzzle.Solve();
            UpdateDisplay(puzzle);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void testButton_Click(object sender, EventArgs e)
        {
            var puzzle = testPuzzles[chooseTest.Text];
            //try
            //{
            puzzle.Solve();
            //}
            //catch (Exception ex)
            //{
            //MessageBox.Show(ex.Message);
            //}

            UpdateDisplay(puzzle);
        }

        private void numRows_ValueChanged(object sender, EventArgs e)
        {
            if (numberOfRows < rowNumerator.Value)
                for (int i = numberOfRows; i < rowNumerator.Value; i++)
                    AddTextBox(true, i);
            else
                for (var i = (int)rowNumerator.Value; i < numberOfRows; i++)
                    RemoveTextBox(true, i);

            numberOfRows = (int)rowNumerator.Value;
        }

        private void numColumns_ValueChanged(object sender, EventArgs e)
        {
            if (numberOfColumns < columnNumerator.Value)
                for (int i = numberOfColumns; i < columnNumerator.Value; i++)
                    AddTextBox(false, i);
            else
                for (var i = (int)columnNumerator.Value; i < numberOfColumns; i++)
                    RemoveTextBox(false, i);

            numberOfColumns = (int)columnNumerator.Value;
        } 
        #endregion
    }
}