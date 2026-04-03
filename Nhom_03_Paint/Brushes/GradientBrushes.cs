using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

namespace Nhom_03_Paint.Brushes
{
    internal static class GradientBrushes
    {
        public enum BrushKind
        {
            Solid,
            LinearGradient,
            PathGradient,
            Texture,
            Hatch
        }

        // [Khoa] Tạo Brush dựa trên loại được chọn. Các tham số:
        // - kind: loại brush (Solid, LinearGradient, PathGradient, Texture, Hatch)
        // - bounds: vùng giới hạn dùng cho các brush dạng gradient/path
        // - fillColor: màu chính dùng để tô
        // - backColor: màu phụ (ví dụ màu nền cho gradient)
        // - texturePath: đường dẫn tới file ảnh nếu dùng TextureBrush
        // - mode: hướng LinearGradient
        public static Brush CreateBrush(BrushKind kind, Rectangle bounds, Color fillColor, Color backColor, string texturePath = null, LinearGradientMode mode = LinearGradientMode.Horizontal)
        {
            // [Khoa] Một số Brush (LinearGradientBrush, PathGradientBrush) không cho phép
            // rectangle có chiều rộng hoặc chiều cao bằng 0. Chuẩn hóa bounds để đảm bảo
            // ít nhất là 1x1 để tránh ArgumentException khi tạo Brush.
            var safeWidth = Math.Max(1, bounds.Width);
            var safeHeight = Math.Max(1, bounds.Height);
            var safeRect = new Rectangle(bounds.X, bounds.Y, safeWidth, safeHeight);
            switch (kind)
            {
                case BrushKind.Solid:
                    // [Khoa] Brush đơn màu
                    return new SolidBrush(fillColor);
                case BrushKind.LinearGradient:
                    // [Khoa] LinearGradientBrush dùng 2 màu và hướng được cung cấp.
                    // Sử dụng safeRect để tránh width/height = 0
                    return new LinearGradientBrush(safeRect, fillColor, backColor, mode);
                case BrushKind.PathGradient:
                    {
                        // [Khoa] PathGradientBrush với đường path (ellipse theo bounds)
                        var path = new GraphicsPath();
                        path.AddEllipse(safeRect);
                        var pgb = new PathGradientBrush(path)
                        {
                            CenterColor = fillColor,
                            SurroundColors = new Color[] { backColor }
                        };
                        return pgb;
                    }
                case BrushKind.Texture:
                    {
                        // [Khoa] TextureBrush: ưu tiên load từ texturePath nếu có,
                        // nếu không có thì cố gắng load resource nội bộ làm fallback.
                        try
                        {
                            Image img = null;
                            if (!string.IsNullOrEmpty(texturePath) && File.Exists(texturePath))
                                img = Image.FromFile(texturePath);
                            else
                            {
                                // [Khoa] Thử load ảnh nhúng trong assembly nếu không có file
                                var asm = Assembly.GetExecutingAssembly();
                                using (var s = asm.GetManifestResourceStream("Nhom_03_Paint.Resources.texture.png"))
                                {
                                    if (s != null) img = Image.FromStream(s);
                                }
                            }

                            if (img != null)
                                return new TextureBrush(img, WrapMode.Tile);
                        }
                        catch
                        {
                            // [Khoa] Nếu có lỗi khi load ảnh, fallback sang solid brush
                        }

                        // [Khoa] Fallback: sử dụng SolidBrush nếu không thể tạo TextureBrush
                        return new SolidBrush(fillColor);
                    }
                case BrushKind.Hatch:
                    // [Khoa] HatchBrush với style mặc định (có thể thay đổi theo UI sau này)
                    return new HatchBrush(HatchStyle.DashedHorizontal, fillColor, backColor);
                default:
                    // [Khoa] Mặc định trả về SolidBrush
                    return new SolidBrush(fillColor);
            }
        }
    }
}
