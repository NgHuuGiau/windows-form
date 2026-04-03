using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nhom_03_Paint.Shapes
{
    internal class LineShape : Shape
    {
        public LineShape()
        {
        }

        public override void Draw(Graphics g)
        {
            using (var pen = new Pen(BorderColor, BorderWidth))
            {
                g.DrawLine(pen, StartPoint, EndPoint);
            }

            // [Khoa] Dòng cũng có thể được chọn; DrawSelection sẽ vẽ khung (dựa trên
            // bounding rectangle) nếu IsSelected = true.
            DrawSelection(g);
        }

        public override bool Contains(Point p)
        {
            // distance from point to line segment
            float dx = EndPoint.X - StartPoint.X;
            float dy = EndPoint.Y - StartPoint.Y;
            float l2 = dx * dx + dy * dy;
            // [Khoa] Nếu đoạn thẳng có chiều dài 0 (góc bắt đầu = kết thúc),
            // thì kiểm tra khoảng cách tới điểm start.
            if (l2 == 0)
            {
                var d = Distance(p, StartPoint);
                return d <= Math.Max(3, BorderWidth);
            }

            float t = ((p.X - StartPoint.X) * dx + (p.Y - StartPoint.Y) * dy) / l2;
            t = Math.Max(0, Math.Min(1, t));
            // [Khoa] Tính khoảng cách từ điểm tới đoạn thẳng (projection) để
            // quyết định xem click gần đường thẳng đủ để coi là chọn hay không.
            float projX = StartPoint.X + t * dx;
            float projY = StartPoint.Y + t * dy;
            var dist = Distance(p, new Point((int)projX, (int)projY));
            return dist <= Math.Max(4, BorderWidth + 2);
        }

        private float Distance(Point a, Point b)
        {
            float dx = a.X - b.X;
            float dy = a.Y - b.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
