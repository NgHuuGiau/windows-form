using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nhom_03_Paint.Shapes
{
    internal class EllipseShape : Shape
    {
        public EllipseShape()
        {
        }

        public override void Draw(Graphics g)
        {
            var r = GetBoundingRectangle();
            // [Khoa] Vẽ ellipse: tạo Brush mặc định nếu cần, vẽ phần tô và viền.
            // Gọi DrawSelection để hiển thị khung khi hình được chọn.
            using (var pen = new Pen(BorderColor, BorderWidth))
            {
                if (Brush == null) Brush = new SolidBrush(FillColor);
                g.FillEllipse(Brush, r);
                g.DrawEllipse(pen, r);
            }

            DrawSelection(g);
        }

        public override bool Contains(Point p)
        {
            // [Khoa] Dùng GraphicsPath.AddEllipse để kiểm tra hit-test chính xác
            var r = GetBoundingRectangle();
            var path = new GraphicsPath();
            path.AddEllipse(r);
            bool contains = path.IsVisible(p);
            path.Dispose();
            return contains;
        }
    }
}
