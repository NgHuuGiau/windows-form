using Nhom_03_Paint.Brushes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nhom_03_Paint
{
    public partial class Form1 : Form
    {
        Color colorBorder; //BIẾN LƯU MÀU VIỀN
        Color colorFill; //BIẾN LƯU MÀU TÔ

        private DrawingManager drawingManager;
        private bool isDrawing = false;
        private Point startPoint = Point.Empty;

        public Form1()
        {
            InitializeComponent();
            drawingManager = new DrawingManager();
            // Hook panel events
            panel1.Paint += Panel1_Paint;
            panel1.MouseDown += Panel1_MouseDown;
            panel1.MouseMove += Panel1_MouseMove;
            panel1.MouseUp += Panel1_MouseUp;
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.DoubleBuffered = true; // Kích hoạt Double Buffering cho Form
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            drawingManager.DrawAll(e.Graphics, panel1.Width, panel1.Height);
        }

        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Check selection first
                var shapes = drawingManager.GetShapes();
                bool found = false;
                for (int i = shapes.Count - 1; i >= 0; i--)
                {
                    if (shapes[i].Contains(e.Location))
                    {
                        // toggle selection
                        shapes[i].IsSelected = !shapes[i].IsSelected;
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    panel1.Invalidate();
                    return;
                }

                // [Khoa] Click vào vùng trống -> bỏ chọn tất cả hình (clear selection)
                // Việc này giúp người dùng có thể hủy lựa chọn hiện tại trước khi bắt đầu vẽ hình mới
                shapes.ForEach(s => s.IsSelected = false);

                isDrawing = true;
                startPoint = e.Location;
            }
        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                // [Khoa] Tạo hình preview để hiển thị mượt khi kéo chuột (không lưu vào danh sách shapes)
                Shape preview = null;
                string type = shapeSelect.SelectedItem?.ToString() ?? "Line";
                switch (type)
                {
                    case "Triangle": preview = new Shapes.TriangleShape(); break;
                    case "Parallelogram": preview = new Shapes.ParallelogramShape(); break;
                    case "Ellipse": preview = new Shapes.EllipseShape(); break;
                    case "Rectangle": preview = new Shapes.RectangleShape(); break;
                    case "Line":
                    default: preview = new Shapes.LineShape(); break;
                }

                if (preview != null)
                {
                    preview.StartPoint = startPoint;
                    preview.EndPoint = e.Location;
                    preview.BorderColor = colorBorder;
                    preview.BorderWidth = (int)sizeBorder.Value;
                    preview.FillColor = colorFill;

                    // create brush for preview similarly to final shape
                    var brushKind = GradientBrushes.BrushKind.Solid;
                    switch (fillStyleSelect.SelectedIndex)
                    {
                        case 0: brushKind = GradientBrushes.BrushKind.Solid; break;
                        case 1: brushKind = GradientBrushes.BrushKind.LinearGradient; break;
                        case 2: brushKind = GradientBrushes.BrushKind.PathGradient; break;
                        case 3: brushKind = GradientBrushes.BrushKind.Texture; break;
                        case 4: brushKind = GradientBrushes.BrushKind.Hatch; break;
                    }

                    var bounds = preview.GetBoundingRectangle();
                    var mode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
                    if (gradientDirectionSelect.SelectedItem != null)
                    {
                        Enum.TryParse(gradientDirectionSelect.SelectedItem.ToString(), out mode);
                    }

                    preview.Brush = GradientBrushes.CreateBrush(brushKind, bounds, preview.FillColor, Color.White, null, mode);
                }

                drawingManager.SetPreviewShape(preview);
                panel1.Invalidate();
            }
            else
            {
                // Nếu không đang vẽ thì đảm bảo không còn preview
                drawingManager.ClearPreviewShape();
            }
        }

        private void Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return;

            isDrawing = false;
            var endPoint = e.Location;

            Shape shape = null;
            string type = shapeSelect.SelectedItem?.ToString() ?? "Line";
            switch (type)
            {
                case "Triangle":
                    shape = new Shapes.TriangleShape();
                    break;
                case "Parallelogram":
                    shape = new Shapes.ParallelogramShape();
                    break;
                case "Ellipse":
                    shape = new Shapes.EllipseShape();
                    break;
                case "Rectangle":
                    shape = new Shapes.RectangleShape();
                    break;
                case "Line":
                default:
                    shape = new Shapes.LineShape();
                    break;
            }

            if (shape != null)
            {
                shape.StartPoint = startPoint;
                shape.EndPoint = endPoint;
                shape.BorderColor = colorBorder;
                shape.BorderWidth = (int)sizeBorder.Value;
                shape.FillColor = colorFill;

                // create brush based on selection
                var brushKind = GradientBrushes.BrushKind.Solid;
                switch (fillStyleSelect.SelectedIndex)
                {
                    case 0: brushKind = GradientBrushes.BrushKind.Solid; break;
                    case 1: brushKind = GradientBrushes.BrushKind.LinearGradient; break;
                    case 2: brushKind = GradientBrushes.BrushKind.PathGradient; break;
                    case 3: brushKind = GradientBrushes.BrushKind.Texture; break;
                    case 4: brushKind = GradientBrushes.BrushKind.Hatch; break;
                }

                var bounds = shape.GetBoundingRectangle();
                var mode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
                if (gradientDirectionSelect.SelectedItem != null)
                {
                    Enum.TryParse(gradientDirectionSelect.SelectedItem.ToString(), out mode);
                }

                shape.Brush = GradientBrushes.CreateBrush(brushKind, bounds, shape.FillColor, Color.White, null, mode);

                drawingManager.AddShape(shape);
            }

            // [Khoa] Vẽ xong -> xóa preview và refresh
            drawingManager.ClearPreviewShape();
            panel1.Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                // remove selected shapes
                var shapes = drawingManager.GetShapes();
                shapes.RemoveAll(s => s.IsSelected);
                panel1.Invalidate();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            drawingManager?.Dispose();
            base.OnFormClosing(e);
        }

        private void colorBorder_Click(object sender, EventArgs e)
        {
            // [Khoa] Mở hộp chọn màu viền. Nếu người dùng nhấn OK thì áp màu viền mới cho
            // tất cả các hình đang được chọn (IsSelected == true) và gọi Invalidate để
            // vẽ lại panel với màu viền cập nhật.
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() != DialogResult.OK) return;
                colorBorder = colorDialog.Color;
                colorBorderSelect.BackColor = colorBorder;

                // [Khoa] Áp màu viền mới cho mọi hình đang chọn
                var shapes = drawingManager.GetShapes();
                foreach (var s in shapes)
                {
                    if (s.IsSelected)
                    {
                        s.BorderColor = colorBorder;
                    }
                }

                // Yêu cầu vẽ lại panel để hiển thị thay đổi
                panel1.Invalidate();
            }
        }

        private void colorFillSelect_Click(object sender, EventArgs e)
        {
            // [Khoa] Mở hộp chọn màu tô. Nếu người dùng nhấn OK thì áp màu tô mới cho
            // mọi hình đang chọn. Đồng thời tái tạo đối tượng Brush tương ứng dựa trên
            // lựa chọn kiểu tô (fillStyleSelect) và hướng gradient (gradientDirectionSelect).
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() != DialogResult.OK) return;
                colorFill = colorDialog.Color;
                colorFillSelect.BackColor = colorFill;

                // [Khoa] Xác định kiểu brush hiện tại từ UI
                var shapes = drawingManager.GetShapes();
                var brushKind = GradientBrushes.BrushKind.Solid;
                switch (fillStyleSelect.SelectedIndex)
                {
                    case 0: brushKind = GradientBrushes.BrushKind.Solid; break;
                    case 1: brushKind = GradientBrushes.BrushKind.LinearGradient; break;
                    case 2: brushKind = GradientBrushes.BrushKind.PathGradient; break;
                    case 3: brushKind = GradientBrushes.BrushKind.Texture; break;
                    case 4: brushKind = GradientBrushes.BrushKind.Hatch; break;
                }

                var mode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
                if (gradientDirectionSelect.SelectedItem != null)
                {
                    Enum.TryParse(gradientDirectionSelect.SelectedItem.ToString(), out mode);
                }

                // [Khoa] Cập nhật FillColor và tạo lại Brush cho từng hình đang chọn
                foreach (var s in shapes)
                {
                    if (s.IsSelected)
                    {
                        // [Khoa] Giải phóng brush cũ trước khi tạo brush mới để tránh rò rỉ
                        try { s.Brush?.Dispose(); } catch { }
                        s.FillColor = colorFill;
                        var bounds = s.GetBoundingRectangle();
                        // [Khoa] Tái tạo Brush dựa trên kiểu và kích thước vùng vẽ của hình
                        s.Brush = GradientBrushes.CreateBrush(brushKind, bounds, s.FillColor, Color.White, null, mode);
                    }
                }

                // Yêu cầu vẽ lại panel để hiển thị màu tô mới
                panel1.Invalidate();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            shapeSelect.SelectedIndex = 0;
            fillStyleSelect.SelectedIndex = 0;
            gradientDirectionSelect.SelectedIndex = 0;
            colorBorder = Color.Black;
            colorBorderSelect.BackColor = colorBorder;
            colorFill = Color.Black;
            colorFillSelect.BackColor = colorFill;
        }

        private void fillStyleSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fillStyleSelect.SelectedIndex != 1)
            {
                labelGrad.Visible = false;
                gradientDirectionSelect.Visible = false;
            }
            else
            {
                labelGrad.Visible = true;
                gradientDirectionSelect.Visible = true;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ứng dụng vẽ đơn giản được phát triển bởi Nhóm 03.\n\n" +
                "Các thành viên:\n" +
                "- Nguyễn Lê Văn Dũng\n" +
                "- Nguyễn Lê Khánh Hoàng\n" +
                "- Nguyễn Đăng Khoa\n" +
                "- Đỗ Văn Hiệp\n" +
                "- Nguyễn Hữu Giàu\n" +
                "\nCảm ơn bạn đã sử dụng ứng dụng!",
                "Thông tin về ứng dụng", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            drawingManager.DrawAll(e.Graphics, panel1.Width, panel1.Height);
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDrawing = true;
                startPoint = e.Location;
            }
        }
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                // Vẽ hình tạm thời khi di chuột
                panel1.Invalidate();
            }
        }
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                isDrawing = false;
                Point endPoint = e.Location;
                // Tạo hình mới dựa trên lựa chọn
                Shape newShape = null;
                switch (shapeSelect.SelectedItem.ToString())
                {
                    case "Line":
                        newShape = new Shapes.LineShape();
                        break;
                    case "Rectangle":
                        newShape = new Shapes.RectangleShape();
                        break;
                    case "Square":
                        newShape = new Shapes.SquareShape();
                        break;
                    case "Ellipse":
                        newShape = new Shapes.EllipseShape();
                        break;
                }
                if (newShape != null)
                {
                    newShape.StartPoint = startPoint;
                    newShape.EndPoint = endPoint;
                    newShape.BorderColor = colorBorder;
                    newShape.FillColor = colorFill;
                    
                    drawingManager.AddShape(newShape);
                    panel1.Invalidate(); // Vẽ lại panel
                }
            }
        }
    }
}
