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
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.DoubleBuffered = true; // Kích hoạt Double Buffering cho Form
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            drawingManager?.Dispose();
            base.OnFormClosing(e);
        }

        private void colorBorder_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                colorBorder = colorDialog.Color;
                colorBorderSelect.BackColor = colorBorder;
            }
        }

        private void colorFillSelect_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                colorFill = colorDialog.Color;
                colorFillSelect.BackColor = colorFill;
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
