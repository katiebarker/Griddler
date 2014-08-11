﻿using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Griddler.Solver
{
    public partial class Shape : UserControl
    {
        public Shape()
        {
            InitializeComponent();
        }

        private Color m_ShapeColor = Color.Black;
        /// <summary>
        /// Gets or sets the color of the divider line
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the color of the divider line")]
        public Color ShapeColor
        {
            private get
            {
                return m_ShapeColor;
            }
            set
            {
                m_ShapeColor = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            using (var brush = new SolidBrush(ShapeColor))
            {
                pe.Graphics.FillRectangle(brush, pe.ClipRectangle);
            }
        }
    }
}