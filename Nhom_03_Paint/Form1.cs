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
        private Color colorBorder = Color.Black; //BIẾN LƯU MÀU VIỀN
        private Color colorFill = Color.Black; //BIẾN LƯU MÀU TÔ

        private DrawingManager drawingManager;
        private bool isDrawing = false;
        private Point startPoint = Point.Empty;

        private string _word = "";
        private int _fontSize = 24;
        private Color _color = Color.DodgerBlue;
        private bool _painting = false;
        private Bitmap _bitmap;

        public Form1()
        {
            InitializeComponent();
            drawingManager = new DrawingManager();

            Text = "WordPaint";
            DoubleBuffered = true;

            // Tạo canvas trên panel1 từ Designer
            var canvas = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
           
                _bitmap = new Bitmap(panel1.Width > 0 ? panel1.Width : 650, panel1.Height > 0 ? panel1.Height : 385);
                canvas.Image = _bitmap;

                canvas.MouseDown += (s, e) => { _painting = true; PaintOnBmp(_bitmap, e.X, e.Y); canvas.Refresh(); };
                //canvas.MouseMove += (s, e) =>
                //{
                //    if (_painting) { PaintOnBmp(_bitmap, e.X, e.Y); canvas.Refresh(); }
                //};
                canvas.MouseUp += (_, __) => _painting = false;

                panel1.Controls.Add(canvas);
            
        }

        // Đổi tên hàm để tránh trùng với sự kiện Paint của Form
        private void PaintOnBmp(Bitmap bmp, int x, int y)
        {
            // Chỉ vẽ chữ nếu shapeSelect là "Text"
            if (shapeSelect.SelectedItem == null || shapeSelect.SelectedItem.ToString() != "Text")
            {
                return;
            }

            using (Graphics g = Graphics.FromImage(bmp))
            using (Font font = new Font("Consolas", _fontSize))
            using (SolidBrush brush = new SolidBrush(_color))
            {
                g.DrawString(_word, font, brush, x, y);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.DoubleBuffered = true; // Kích hoạt Double Buffering cho Form
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _bitmap?.Dispose();
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

        private void shapeSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Khi người dùng chuyển đổi shape, xóa canvas để bắt đầu vẽ cái mới
            if (_bitmap != null)
            {
                using (Graphics g = Graphics.FromImage(_bitmap))
                {
                    g.Clear(Color.White);
                }

                // Tìm PictureBox trong panel1 và refresh
                foreach (Control control in panel1.Controls)
                {
                    if (control is PictureBox picBox)
                    {
                        picBox.Refresh();
                        break;
                    }
                }
            }

            // Nếu chuyển sang "Text", cho phép nhập text
            if (shapeSelect.SelectedItem != null && shapeSelect.SelectedItem.ToString() == "Text")
            {
                Boxtext.Enabled = true;
                Boxtext.Focus();
            }
            else
            {
                Boxtext.Enabled = false;
                _word = ""; // Xóa từ khi chuyển shape
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

        private void Boxtext_TextChanged(object sender, EventArgs e)
        {
            // Chỉ cập nhật _word nếu shapeSelect là "Text"
            if (shapeSelect.SelectedItem != null && shapeSelect.SelectedItem.ToString() == "Text")
            {
                _word = Boxtext.Text;
            }
        }
    }

}
