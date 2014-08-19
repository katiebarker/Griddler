using System.Windows.Forms;

namespace Griddler.Solver
{
    partial class SolverForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.solveButton = new System.Windows.Forms.Button();
            this.rowLabel = new System.Windows.Forms.Label();
            this.columnLabel = new System.Windows.Forms.Label();
            this.testButton = new System.Windows.Forms.Button();
            this.chooseTest = new System.Windows.Forms.ComboBox();
            this.rowNumerator = new System.Windows.Forms.NumericUpDown();
            this.columnNumerator = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.rowNumerator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.columnNumerator)).BeginInit();
            this.SuspendLayout();
            // 
            // solveButton
            // 
            this.solveButton.Location = new System.Drawing.Point(364, 12);
            this.solveButton.Name = "solveButton";
            this.solveButton.Size = new System.Drawing.Size(75, 23);
            this.solveButton.TabIndex = 1;
            this.solveButton.Text = "Solve";
            this.solveButton.UseVisualStyleBackColor = true;
            this.solveButton.Click += new System.EventHandler(this.solveButton_Click);
            // 
            // rowLabel
            // 
            this.rowLabel.AutoSize = true;
            this.rowLabel.Location = new System.Drawing.Point(71, 96);
            this.rowLabel.Name = "rowLabel";
            this.rowLabel.Size = new System.Drawing.Size(34, 13);
            this.rowLabel.TabIndex = 12;
            this.rowLabel.Text = "Rows";
            // 
            // columnLabel
            // 
            this.columnLabel.AutoSize = true;
            this.columnLabel.Location = new System.Drawing.Point(212, 96);
            this.columnLabel.Name = "columnLabel";
            this.columnLabel.Size = new System.Drawing.Size(47, 13);
            this.columnLabel.TabIndex = 13;
            this.columnLabel.Text = "Columns";
            // 
            // testButton
            // 
            this.testButton.Location = new System.Drawing.Point(188, 14);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(75, 23);
            this.testButton.TabIndex = 17;
            this.testButton.Text = "Test";
            this.testButton.UseVisualStyleBackColor = true;
            this.testButton.Click += new System.EventHandler(this.testButton_Click);
            // 
            // chooseTest
            // 
            this.chooseTest.FormattingEnabled = true;
            this.chooseTest.Location = new System.Drawing.Point(47, 16);
            this.chooseTest.Name = "chooseTest";
            this.chooseTest.Size = new System.Drawing.Size(121, 21);
            this.chooseTest.TabIndex = 18;
            // 
            // rowNumerator
            // 
            this.rowNumerator.Location = new System.Drawing.Point(47, 63);
            this.rowNumerator.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.rowNumerator.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.rowNumerator.Name = "rowNumerator";
            this.rowNumerator.Size = new System.Drawing.Size(58, 20);
            this.rowNumerator.TabIndex = 19;
            this.rowNumerator.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.rowNumerator.ValueChanged += new System.EventHandler(this.numRows_ValueChanged);
            // 
            // columnNumerator
            // 
            this.columnNumerator.Location = new System.Drawing.Point(188, 63);
            this.columnNumerator.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.columnNumerator.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.columnNumerator.Name = "columnNumerator";
            this.columnNumerator.Size = new System.Drawing.Size(58, 20);
            this.columnNumerator.TabIndex = 20;
            this.columnNumerator.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.columnNumerator.ValueChanged += new System.EventHandler(this.numColumns_ValueChanged);
            // 
            // SolverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 323);
            this.Controls.Add(this.columnNumerator);
            this.Controls.Add(this.rowNumerator);
            this.Controls.Add(this.chooseTest);
            this.Controls.Add(this.testButton);
            this.Controls.Add(this.columnLabel);
            this.Controls.Add(this.rowLabel);
            this.Controls.Add(this.solveButton);
            this.Name = "SolverForm";
            this.Text = "Solver";
            ((System.ComponentModel.ISupportInitialize)(this.rowNumerator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.columnNumerator)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button solveButton;
        private System.Windows.Forms.Label rowLabel;
        private System.Windows.Forms.Label columnLabel;
        private System.Windows.Forms.Button testButton;
        private System.Windows.Forms.ComboBox chooseTest;
        private NumericUpDown rowNumerator;
        private NumericUpDown columnNumerator;
    }
}

