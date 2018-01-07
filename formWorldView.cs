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
        int tileRenderSize = 8;
        World world;
        Dictionary<Group, Color> groupRenderColors = new Dictionary<Group, Color>();
        Random rng = new Random();

        public formWorldView()
        {
            this.InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            var generation = new WorldGeneration();
            generation.TargetGenerationSize = 1000;
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
                using (var brush = new SolidBrush(Color.Black))
                {
                    foreach (var tile in this.world.Tiles)
                    {
                        var tileRenderRectangle = new Rectangle(renderOrigin.X + (tile.Position.X * this.tileRenderSize), renderOrigin.Y + (tile.Position.Y * this.tileRenderSize), this.tileRenderSize, this.tileRenderSize);
                        if (tile.MineralNode) e.Graphics.FillRectangle(Brushes.Green, tileRenderRectangle);
                        e.Graphics.DrawRectangle(Pens.Black, tileRenderRectangle);

                        if (tile.UnitLayer != null)
                        {
                            brush.Color = this.GetGroupColor(tile.UnitLayer.Group);
                            var widthShift = tileRenderRectangle.Width / 2;
                            var heightShift = tileRenderRectangle.Height / 2;
                            e.Graphics.FillRectangle(brush, new Rectangle(tileRenderRectangle.X + (widthShift / 2), tileRenderRectangle.Y + (heightShift / 2), tileRenderRectangle.Width - widthShift, tileRenderRectangle.Height - heightShift));
                            e.Graphics.DrawRectangle(Pens.Black, new Rectangle(tileRenderRectangle.X + (widthShift / 2), tileRenderRectangle.Y + (heightShift / 2), tileRenderRectangle.Width - widthShift, tileRenderRectangle.Height - heightShift));
                        }
                    }
                }
            }
        }

        private Color GetGroupColor(Group group)
        {
            if (!this.groupRenderColors.ContainsKey(group)) this.groupRenderColors.Add(group, Color.FromArgb(this.rng.Next(256), this.rng.Next(256), this.rng.Next(256)));
            return this.groupRenderColors[group];
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
