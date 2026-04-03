using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Nhom_03_Paint.Brushes;

namespace Nhom_03_Paint.Shapes
{
    internal class TriangleShape : Shape
    {
        public TriangleShape()
        {
        }

        public override void Draw(Graphics g)
        {
            var rect = GetBoundingRectangle();

            Point p1 = new Point(rect.Left + rect.Width / 2, rect.Top);
            Point p2 = new Point(rect.Left, rect.Bottom);
            Point p3 = new Point(rect.Right, rect.Bottom);

            // [Khoa] Vẽ tam giác: nếu chưa có Brush thì tạo SolidBrush từ FillColor.
            // Sau khi vẽ, gọi DrawSelection để hiển thị khung nếu hình đang được chọn.
            using (var pen = new Pen(BorderColor, BorderWidth))
            {
                if (Brush == null) Brush = new SolidBrush(FillColor);
                g.FillPolygon(Brush, new Point[] { p1, p2, p3 });
                g.DrawPolygon(pen, new Point[] { p1, p2, p3 });
            }

            DrawSelection(g);
        }

        public override bool Contains(Point p)
        {
            var rect = GetBoundingRectangle();
            Point p1 = new Point(rect.Left + rect.Width / 2, rect.Top);
            Point p2 = new Point(rect.Left, rect.Bottom);
            Point p3 = new Point(rect.Right, rect.Bottom);
            // [Khoa] Sử dụng GraphicsPath để kiểm tra point-in-polygon chính xác hơn
            // so với bounding rectangle. Phục vụ cho tương tác chọn hình khi click.
            var path = new GraphicsPath();
            path.AddPolygon(new Point[] { p1, p2, p3 });
            bool contains = path.IsVisible(p);
            path.Dispose();
            return contains;
        }
    }
}
