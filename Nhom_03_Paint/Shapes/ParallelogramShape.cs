using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using Nhom_03_Paint.Brushes;

namespace Nhom_03_Paint.Shapes
{
    internal class ParallelogramShape : Shape
    {
        public ParallelogramShape()
        {
        }

        public override void Draw(Graphics g)
        {
            var r = GetBoundingRectangle();
            int offset = Math.Min(30, r.Width / 4);

            Point p1 = new Point(r.Left + offset, r.Top);
            Point p2 = new Point(r.Right, r.Top);
            Point p3 = new Point(r.Right - offset, r.Bottom);
            Point p4 = new Point(r.Left, r.Bottom);

            var pts = new Point[] { p1, p2, p3, p4 };

            // [Khoa] Vẽ hình bình hành: tạo Brush nếu chưa tồn tại, vẽ fill rồi vẽ viền.
            // Cuối cùng gọi DrawSelection để hiển thị khung nếu hình đang được chọn.
            using (var pen = new Pen(BorderColor, BorderWidth))
            {
                if (Brush == null) Brush = new SolidBrush(FillColor);
                g.FillPolygon(Brush, pts);
                g.DrawPolygon(pen, pts);
            }

            DrawSelection(g);
        }

        public override bool Contains(Point p)
        {
            var r = GetBoundingRectangle();
            int offset = Math.Min(30, r.Width / 4);
            Point p1 = new Point(r.Left + offset, r.Top);
            Point p2 = new Point(r.Right, r.Top);
            Point p3 = new Point(r.Right - offset, r.Bottom);
            Point p4 = new Point(r.Left, r.Bottom);
            // [Khoa] Dùng GraphicsPath để kiểm tra point-in-polygon cho hình bình hành
            var path = new GraphicsPath();
            path.AddPolygon(new Point[] { p1, p2, p3, p4 });
            bool contains = path.IsVisible(p);
            path.Dispose();
            return contains;
        }
    }
}
