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
        private const int gridSize = 16;
        private readonly Point start = new Point(80, 80);
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

            boxes = new Dictionary<Point, PictureBox>();
            gridLines = new List<Shape>();
            labels = new List<Label>();

            InitializeComponent();

            for (int i = 0; i < numberOfRows; i++)
            {
                AddTextBox(true, i);
            }

            for (int i = 0; i < numberOfColumns; i++)
            {
                AddTextBox(false, i);
            }

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
            Point p = start;
            string type;
            string clues;
            if (isRow)
            {
                clues = puzzle.GetRowClues(num);
                type = "hori";
            }
            else
            {
                clues = puzzle.GetColumnClues(num);
                type = "vert";
            }

            string name = String.Format("{0}Label{1}", type, num);

            Label label;

            if (this.solutionPanel.Controls.ContainsKey(name))
            {
                label = (Label)this.solutionPanel.Controls[name];
                label.Show();
            }
            else
            {
                label = new Label
                {
                    Name = name,
                    Font = new Font("Microsoft Sans Serif", 7),
                    AutoSize = true
                };

                this.solutionPanel.Controls.Add(label);
            }

            label.Text = clues;
            labels.Add(label);

            if (isRow)
            {
                p.Offset(-label.Width - 5, gridSize * num + 3);
            }
            else
            {
                p.Offset(gridSize * num + 3, -label.Height - 5);
            }

            label.Location = p;
        }

        private void AddGridLine(Puzzle puzzle, bool isRow, int num)
        {
            var location = start;
            var size = new Size(1, 1);

            if (isRow)
            {
                location.Y += gridSize * num;
                size.Width += puzzle.Width * gridSize;
            }
            if (!isRow)
            {
                location.X += gridSize * num;
                size.Height += puzzle.Height * gridSize;
            }

            Shape gridline;

            string type = size.Height == 1 ? "hori" : "vert";
            string name = String.Format("{0}Gridline_{1}", type, num);

            if (this.solutionPanel.Controls.ContainsKey(name))
            {
                gridline = (Shape)this.solutionPanel.Controls[name];
                gridline.Show();
            }
            else
            {
                gridline = new Shape
                {
                    Location = location,
                    Name = name
                };
                gridline.ShapeColor = (num % 5 == 0) ? Color.Black : Color.LightPink;
                this.solutionPanel.Controls.Add(gridline);
                gridLines.Add(gridline);
            }
            gridline.Size = size;
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


            boxes.Clear();
        }

        private void AddBox(Point p)
        {
            string name = String.Format("square_{0}_{1}", p.X, p.Y);
            if (this.solutionPanel.Controls.ContainsKey(name))
            {
                this.solutionPanel.Controls[name].Show();
                boxes.Add(p, (PictureBox)this.solutionPanel.Controls[name]);
            }
            else
            {
                var square = new PictureBox();
                boxes.Add(p, square);
                Point q = start;
                q.X += 2 + gridSize * p.Y;
                q.Y += 2 + gridSize * p.X;

                square.BackColor = Color.White;
                square.Location = q;
                square.Name = name;
                square.Size = new Size(gridSize - 3, gridSize - 3);
                square.TabStop = false;

                this.solutionPanel.Controls.Add(square);
            }
        }

        private void AddTextBox(bool isRow, int num)
        {
            string type = isRow ? "row" : "column";
            Dictionary<int, TextBox> textBoxes = isRow ? rowTextBoxs : columnTextBoxs;
            Point p = new Point();
            p.X = isRow ? 10 : 150;
            p.Y = 100 + 25 * num;
            string name = String.Format("{0}TextBox{1}", type, num);
            TextBox textBox;
            if (this.tabPageManual.Controls.ContainsKey(name))
            {
                textBox = this.tabPageManual.Controls[name] as TextBox;
                textBox.Show();
            }
            else
            {
                textBox = new TextBox()
                {
                    Location = p,
                    Name = name,
                    Size = new Size(100, 20)
                };

                this.tabPageManual.Controls.Add(textBox);
                textBoxes.Add(num, textBox);
            }
        }

        private void RemoveTextBox(bool isRow, int num)
        {
            Dictionary<int, TextBox> textBoxes = isRow ? rowTextBoxs : columnTextBoxs;
            textBoxes[num].Hide();
            textBoxes.Remove(num);
        }
        #endregion

        #region Events
        private void solveButton_Click(object sender, EventArgs e)
        {
            try
            {
                var rowString = "";
                foreach (var box in rowTextBoxs)
                {
                    rowString += box.Value.Text + ";";
                }
                var colString = "";
                foreach (var box in rowTextBoxs)
                {
                    colString += box.Value.Text + ";";
                }

                var puzzle = new Puzzle(numberOfRows, numberOfColumns, rowString, colString);

                puzzle.Solve();
                UpdateDisplay(puzzle);
            }
            catch (InvalidPuzzleException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void testButton_Click(object sender, EventArgs e)
        {
            try
            {
                Puzzle puzzle;
                if (!testPuzzles.TryGetValue(chooseTest.Text, out puzzle))
                {
                    throw new InvalidPuzzleException("Select Valid Puzzle from list");
                }
                else
                {
                    puzzle.Solve();

                    UpdateDisplay(puzzle);
                }
            }
            catch (InvalidPuzzleException ex)
            {
                MessageBox.Show(ex.Message);
            }
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