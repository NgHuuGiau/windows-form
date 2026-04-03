using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nhom_03_Paint.Shapes
{
    internal class RectangleShape : Shape
    {
        public override void Draw(Graphics g)
        {
            using (Pen pen = new Pen(BorderColor, BorderWidth))
            {
                g.DrawRectangle(pen, GetBoundingRectangle());
            }
            using (SolidBrush brush = new SolidBrush(FillColor))
            {
                g.FillRectangle(brush, GetBoundingRectangle());
            }
        }
    }
}
