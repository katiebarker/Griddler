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
            this.puzzleTabs = new System.Windows.Forms.TabControl();
            this.tabPageManual = new System.Windows.Forms.TabPage();
            this.tabPageExample = new System.Windows.Forms.TabPage();
            this.solutionPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.rowNumerator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.columnNumerator)).BeginInit();
            this.puzzleTabs.SuspendLayout();
            this.tabPageManual.SuspendLayout();
            this.tabPageExample.SuspendLayout();
            this.SuspendLayout();
            // 
            // solveButton
            // 
            this.solveButton.Location = new System.Drawing.Point(75, 6);
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
            this.rowLabel.Location = new System.Drawing.Point(10, 53);
            this.rowLabel.Name = "rowLabel";
            this.rowLabel.Size = new System.Drawing.Size(36, 14);
            this.rowLabel.TabIndex = 12;
            this.rowLabel.Text = "Rows";
            // 
            // columnLabel
            // 
            this.columnLabel.AutoSize = true;
            this.columnLabel.Location = new System.Drawing.Point(150, 53);
            this.columnLabel.Name = "columnLabel";
            this.columnLabel.Size = new System.Drawing.Size(54, 14);
            this.columnLabel.TabIndex = 13;
            this.columnLabel.Text = "Columns";
            // 
            // testButton
            // 
            this.testButton.Location = new System.Drawing.Point(151, 16);
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
            this.chooseTest.Location = new System.Drawing.Point(15, 16);
            this.chooseTest.Name = "chooseTest";
            this.chooseTest.Size = new System.Drawing.Size(121, 22);
            this.chooseTest.TabIndex = 18;
            // 
            // rowNumerator
            // 
            this.rowNumerator.Location = new System.Drawing.Point(56, 50);
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
            this.rowNumerator.Size = new System.Drawing.Size(58, 22);
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
            this.columnNumerator.Location = new System.Drawing.Point(215, 50);
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
            this.columnNumerator.Size = new System.Drawing.Size(58, 22);
            this.columnNumerator.TabIndex = 20;
            this.columnNumerator.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.columnNumerator.ValueChanged += new System.EventHandler(this.numColumns_ValueChanged);
            // 
            // puzzleTabs
            // 
            this.puzzleTabs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.puzzleTabs.Controls.Add(this.tabPageManual);
            this.puzzleTabs.Controls.Add(this.tabPageExample);
            this.puzzleTabs.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.puzzleTabs.Location = new System.Drawing.Point(12, 12);
            this.puzzleTabs.Name = "puzzleTabs";
            this.puzzleTabs.SelectedIndex = 0;
            this.puzzleTabs.Size = new System.Drawing.Size(339, 295);
            this.puzzleTabs.TabIndex = 21;
            // 
            // tabPageManual
            // 
            this.tabPageManual.AutoScroll = true;
            this.tabPageManual.BackColor = System.Drawing.Color.White;
            this.tabPageManual.Controls.Add(this.rowNumerator);
            this.tabPageManual.Controls.Add(this.columnNumerator);
            this.tabPageManual.Controls.Add(this.rowLabel);
            this.tabPageManual.Controls.Add(this.solveButton);
            this.tabPageManual.Controls.Add(this.columnLabel);
            this.tabPageManual.Location = new System.Drawing.Point(4, 23);
            this.tabPageManual.Name = "tabPageManual";
            this.tabPageManual.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageManual.Size = new System.Drawing.Size(331, 268);
            this.tabPageManual.TabIndex = 0;
            this.tabPageManual.Text = "Manual Puzzles";
            // 
            // tabPageExample
            // 
            this.tabPageExample.Controls.Add(this.chooseTest);
            this.tabPageExample.Controls.Add(this.testButton);
            this.tabPageExample.Location = new System.Drawing.Point(4, 22);
            this.tabPageExample.Name = "tabPageExample";
            this.tabPageExample.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageExample.Size = new System.Drawing.Size(331, 269);
            this.tabPageExample.TabIndex = 1;
            this.tabPageExample.Text = "Example Puzzles";
            this.tabPageExample.UseVisualStyleBackColor = true;
            // 
            // solutionPanel
            // 
            this.solutionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.solutionPanel.AutoScroll = true;
            this.solutionPanel.BackColor = System.Drawing.Color.White;
            this.solutionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.solutionPanel.Location = new System.Drawing.Point(377, 34);
            this.solutionPanel.Name = "solutionPanel";
            this.solutionPanel.Size = new System.Drawing.Size(271, 273);
            this.solutionPanel.TabIndex = 22;
            // 
            // SolverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(600, 300);
            this.ClientSize = new System.Drawing.Size(660, 323);
            this.Controls.Add(this.solutionPanel);
            this.Controls.Add(this.puzzleTabs);
            this.Name = "SolverForm";
            this.Text = "Solver";
            ((System.ComponentModel.ISupportInitialize)(this.rowNumerator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.columnNumerator)).EndInit();
            this.puzzleTabs.ResumeLayout(false);
            this.tabPageManual.ResumeLayout(false);
            this.tabPageManual.PerformLayout();
            this.tabPageExample.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button solveButton;
        private System.Windows.Forms.Label rowLabel;
        private System.Windows.Forms.Label columnLabel;
        private System.Windows.Forms.Button testButton;
        private System.Windows.Forms.ComboBox chooseTest;
        private NumericUpDown rowNumerator;
        private NumericUpDown columnNumerator;
        private TabControl puzzleTabs;
        private TabPage tabPageManual;
        private TabPage tabPageExample;
        private Panel solutionPanel;
    }
}

