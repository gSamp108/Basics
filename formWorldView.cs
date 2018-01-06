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
    public partial class formWorldView : Form
    {
        bool mouseDown = false;
        Point mouseDownPoint;
        Point mouseDownCameraPoint;
        Point cameraPosition;
        int tileRenderSize = 4;
        World world;

        public formWorldView()
        {
            this.InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            var generation = new WorldGeneration();
            generation.TargetGenerationSize = 10000;
            generation.FullGeneration();
            this.world = new World(generation);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var canvasCenter = new Point(this.ClientRectangle.Width / 2, this.ClientRectangle.Height / 2);
            var renderOrigin = new Point(canvasCenter.X + this.cameraPosition.X, canvasCenter.Y + this.cameraPosition.Y);

            if (this.world != null)
            {
                foreach (var tile in this.world.Tiles)
                {
                    var tileRenderRectangle = new Rectangle(renderOrigin.X + (tile.Position.X * this.tileRenderSize), renderOrigin.Y + (tile.Position.Y * this.tileRenderSize), this.tileRenderSize, this.tileRenderSize);
                    if (tile.MineralNode) e.Graphics.FillRectangle(Brushes.Green, tileRenderRectangle);
                    e.Graphics.DrawRectangle(Pens.Black, tileRenderRectangle);
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

    }
}
