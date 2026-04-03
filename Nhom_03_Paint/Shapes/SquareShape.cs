using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nhom_03_Paint.Shapes
{
    internal class SquareShape : Shape
    {
        public override void Draw(Graphics g)
        {
            int size = Math.Min(GetWidth(), GetHeight());
            Rectangle rect = new Rectangle(StartPoint.X, StartPoint.Y, size, size);
            using (Pen pen = new Pen(BorderColor, BorderWidth))
            {
                g.DrawRectangle(pen, rect);
            }
            using (SolidBrush brush = new SolidBrush(FillColor))
            {
                g.FillRectangle(brush, rect);
            }
        }
    }
}
