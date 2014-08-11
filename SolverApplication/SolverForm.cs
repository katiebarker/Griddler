using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Griddler.PuzzleModel;

namespace Griddler.Solver
{
    public partial class SolverForm : Form
    {
        private const int GridSize = 20;
        private readonly List<Shape> GridLines;
        private readonly List<Label> Labels;
        private int NumberOfColumns;
        private int NumberOfRows;
        private Puzzle puzzle;

        public SolverForm()
        {
            NumberOfRows = 5;
            NumberOfColumns = 5;
            rowTextBoxs = new Dictionary<int, TextBox>();
            columnTextBoxs = new Dictionary<int, TextBox>();

            for (int i = 0; i < NumberOfRows; i++)
            {
                AddTextBox(true, i);
            }

            for (int i = 0; i < NumberOfColumns; i++)
            {
                AddTextBox(false, i);
            }

            Boxes = new Dictionary<int, PictureBox>();
            GridLines = new List<Shape>();
            Labels = new List<Label>();

            InitializeComponent();

            InitialisePuzzles();

            chooseTest.Items.AddRange(testPuzzles.Keys.ToArray());
        }

        private static int ToCode(int i, int j)
        {
            return i * 100 + j;
        }

        private void InitialisePuzzles()
        {
            testPuzzles = TestPuzzles.Puzzles;
        }

        private void UpdateDisplay()
        {
            RemoveGrid();

            for (int row = 0; row < puzzle.Height; row++)
            {
                AddGridLine(true, row);
                AddLabel(true, row);
                for (int col = 0; col < puzzle.Width; col++)
                {
                    AddGridLine(false, col);
                    AddLabel(false, col);
                    AddSquare(row, col);

                    switch (puzzle.Cells[col, row].Value)
                    {
                        case 1:
                            Boxes[ToCode(row, col)].BackColor = Color.Black;
                            break;
                        case -1:
                            Boxes[ToCode(row, col)].BackColor = Color.LightGray;
                            break;
                        case 0:
                            Boxes[ToCode(row, col)].BackColor = Color.White;
                            break;
                        default:
                            throw new Exception("Wrong Cell Value");
                    }
                }
                AddGridLine(true, puzzle.Height);
                AddGridLine(false, puzzle.Width);
            }
        }

        private void AddLabel(bool isRow, int i)
        {
            string type = isRow ? "hori" : "vert";
            int x = isRow ? 400 : 400 + GridSize * i;
            int y = isRow ? 100 + GridSize * i : 100;
            string name = String.Format("{0}Label{1}", type, i);
            
            var clues = isRow ? puzzle.GetRowClues(i) : puzzle.GetColumnClues(i);
            
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
            Labels.Add(label);
            label.Location = isRow ? new Point(x - label.Width - 5,y + 3) : new Point(x + 3, y - label.Height - 5);
        }

        private static int[] StringToIntClues(string input)
        {
            string[] stringClues = input.Split(',');

            var intClues = new int[stringClues.Length];

            for (int i = 0; i < stringClues.Length; i++)
            {
                int clue;
                if (int.TryParse(stringClues[i].Trim(), out clue)) intClues[i] = clue;
                else throw new ApplicationException("String Parse Error");
            }
            return intClues;
        }

        private void RemoveGrid()
        {
            foreach (var square in Boxes)
            {
                square.Value.Hide();
            }
            foreach (Shape shape in GridLines)
            {
                shape.Hide();
            }
            foreach (var label in Labels)
            {
                label.Hide();
            }

            GridLines.Clear();
            Boxes.Clear();
        }

        private void AddGridLine(bool isRow, int i)
        {
            string type = isRow ? "hori" : "vert";
            int x = isRow ? 400 : 400 + GridSize * i;
            int y = isRow ? 100 + GridSize * i : 100;
            int sizeX = isRow ? puzzle.Width * GridSize + 1 : 1;
            int sizeY = isRow ? 1 : puzzle.Height * GridSize + 1;
            string name = String.Format("{0}Gridline{1}", type, i);
            if (Controls.ContainsKey(name))
            {
                Controls[name].Show();
                GridLines.Add((Shape)Controls[name]);
                Controls[name].Size = new Size(sizeX, sizeY);
            }
            else
            {
                var gridLine = new Shape
                {
                    Location = new Point(x, y),
                    Name = name,
                    ShapeColor = Color.Black,
                    Size = new Size(sizeX, sizeY)
                };

                Controls.Add(gridLine);
                GridLines.Add(gridLine);
            }
        }

        private void AddSquare(int i, int j)
        {
            string name = String.Format("square_{0}_{1}", i, j);
            if (Controls.ContainsKey(name))
            {
                Controls[name].Show();
                Boxes.Add(ToCode(i, j), (PictureBox)Controls[name]);
            }
            else
            {
                var square = new PictureBox();
                Boxes.Add(ToCode(i, j), square);

                int x = 402 + GridSize * j;
                int y = 102 + GridSize * i;

                square.BackColor = Color.White;
                square.Location = new Point(x, y);
                square.Name = name;
                square.Size = new Size(GridSize - 3, GridSize - 3);
                square.TabStop = false;

                Controls.Add(square);
            }
        }

        private void AddTextBox(bool isRow, int i)
        {
            string type = isRow ? "row" : "column";
            Dictionary<int, TextBox> dict = isRow ? rowTextBoxs : columnTextBoxs;
            int x = isRow ? 47 : 188;
            int y = 120 + 25 * i;
            string name = String.Format("{0}TextBox{1}", type, i);

            if (Controls.ContainsKey(name))
            {
                Controls[name].Show();
                dict.Add(i, Controls[name] as TextBox);
            }
            else
            {
                var textBox = new TextBox();
                dict.Add(i, textBox);

                textBox.Location = new Point(x, y);
                textBox.Name = name;
                textBox.Size = new Size(100, 20);

                Controls.Add(textBox);
            }
        }

        private void RemoveTextBox(bool isRow, int i)
        {
            Dictionary<int, TextBox> dict = isRow ? rowTextBoxs : columnTextBoxs;
            dict[i].Hide();
            dict.Remove(i);
        }

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

            puzzle = new Puzzle(NumberOfRows, NumberOfColumns, lineString);
            
            puzzle.Solve();
            UpdateDisplay();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void testButton_Click(object sender, EventArgs e)
        {
            puzzle = testPuzzles[chooseTest.Text];
            //try
            //{
                puzzle.Solve();
            //}
            //catch (Exception ex)
            //{
                //MessageBox.Show(ex.Message);
            //}

            UpdateDisplay();
        }

        private void numRows_ValueChanged(object sender, EventArgs e)
        {
            if (NumberOfRows < rowNumerator.Value)
                for (int i = NumberOfRows; i < rowNumerator.Value; i++)
                    AddTextBox(true, i);
            else
                for (var i = (int)rowNumerator.Value; i < NumberOfRows; i++)
                    RemoveTextBox(true, i);

            NumberOfRows = (int)rowNumerator.Value;
        }

        private void numColumns_ValueChanged(object sender, EventArgs e)
        {
            if (NumberOfColumns < columnNumerator.Value)
                for (int i = NumberOfColumns; i < columnNumerator.Value; i++)
                    AddTextBox(false, i);
            else
                for (var i = (int)columnNumerator.Value; i < NumberOfColumns; i++)
                    RemoveTextBox(false, i);

            NumberOfColumns = (int)columnNumerator.Value;
        }
    }
}