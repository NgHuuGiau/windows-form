using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nhom_03_Paint.Shapes
{
    internal class RectangleShape : Shape
    {
        public RectangleShape()
        {
        }

        public override void Draw(Graphics g)
        {
            var r = GetBoundingRectangle();
            // [Khoa] Vẽ hình chữ nhật: tạo Brush mặc định nếu cần, vẽ phần tô và vẽ viền.
            // Gọi DrawSelection để hiển thị khung chọn khi IsSelected = true.
            using (var pen = new Pen(BorderColor, BorderWidth))
            {
                if (Brush == null) Brush = new SolidBrush(FillColor);
                g.FillRectangle(Brush, r);
                g.DrawRectangle(pen, r);
            }

            DrawSelection(g);
        }

        public override bool Contains(Point p)
        {
            // [Khoa] Với hình chữ nhật, việc kiểm tra điểm nằm trong hình đơn giản
            // là kiểm tra bounding rectangle.
            return GetBoundingRectangle().Contains(p);
        }
    }
}
