using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Basics
{
    public partial class Form1 : Form
    {
        World world = new World();
        bool mouseDown = false;
        Point mouseDownPoint;
        Point mouseDownCameraPoint;
        Point cameraPosition;
        int tileRenderSize = 16;

        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            foreach (var control in this.Controls)
            {
                if (control is CheckBox)
                {
                    var checkbox = (CheckBox)control;
                    checkbox.CheckStateChanged += new EventHandler(checkbox_CheckStateChanged);
                }
            }
        }

        void checkbox_CheckStateChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var canvasCenter = new Point(this.ClientRectangle.Width / 2, this.ClientRectangle.Height / 2);
            var renderOrigin = new Point(canvasCenter.X + this.cameraPosition.X, canvasCenter.Y + this.cameraPosition.Y);

            if (this.world != null)
            {
                foreach (var tile in this.world.Tiles.Values)
                {
                    if (this.checkBox2.Checked && this.world.UngeneratedTiles.Contains(tile)) e.Graphics.FillRectangle(Brushes.Red, new Rectangle(renderOrigin.X + (tile.Position.X * this.tileRenderSize), renderOrigin.Y + (tile.Position.Y * this.tileRenderSize), this.tileRenderSize - 1, this.tileRenderSize - 1));
                    if (this.checkBox3.Checked && this.world.GeneratableTiles.Contains(tile)) e.Graphics.FillRectangle(Brushes.Yellow, new Rectangle(renderOrigin.X + (tile.Position.X * this.tileRenderSize), renderOrigin.Y + (tile.Position.Y * this.tileRenderSize), this.tileRenderSize - 1, this.tileRenderSize - 1));
                    if (this.checkBox4.Checked && this.world.GeneratedTiles.Contains(tile)) e.Graphics.FillRectangle(Brushes.Green, new Rectangle(renderOrigin.X + (tile.Position.X * this.tileRenderSize), renderOrigin.Y + (tile.Position.Y * this.tileRenderSize), this.tileRenderSize - 1, this.tileRenderSize - 1));
                    if (this.checkBox5.Checked) e.Graphics.DrawString(tile.GenerationWeight.ToString(), this.Font, Brushes.Black, new PointF(renderOrigin.X + (tile.Position.X * this.tileRenderSize), renderOrigin.Y + (tile.Position.Y * this.tileRenderSize)));



                    if (this.checkBox1.Checked) e.Graphics.DrawRectangle(Pens.Black, new Rectangle(renderOrigin.X + (tile.Position.X * this.tileRenderSize), renderOrigin.Y + (tile.Position.Y * this.tileRenderSize), this.tileRenderSize - 1, this.tileRenderSize - 1));
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.mouseDown = true;
            this.mouseDownPoint = e.Location;
            this.mouseDownCameraPoint = this.cameraPosition;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (this.mouseDown)
            {
                var delta = new Point(this.mouseDownPoint.X - e.Location.X, this.mouseDownPoint.Y - e.Location.Y);
                this.cameraPosition = new Point(this.mouseDownCameraPoint.X - delta.X, this.mouseDownCameraPoint.Y - delta.Y);
                this.Invalidate();
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this.mouseDown = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.world = new World();
            this.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.world != null)
            {
                this.world.GenerationStep();
                this.Invalidate();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.world != null)
            {
                this.world.FullGeneration();
                this.Invalidate();
            }
        }
    }
}
