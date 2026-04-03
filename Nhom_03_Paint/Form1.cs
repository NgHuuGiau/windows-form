using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nhom_03_Paint.Shapes;

namespace Nhom_03_Paint
{
    public partial class Form1 : Form
    {
        Color colorBorder; //BIẾN LƯU MÀU VIỀN
        Color colorFill; //BIẾN LƯU MÀU TÔ

        private DrawingManager drawingManager;
        private bool isDrawing = false;
        private Point startPoint = Point.Empty;
        private bool isMoving = false;
        private Shape movingShape = null;
        private Point moveOffset = Point.Empty;
        private Point originalDelta = Point.Empty;
        private Point lastEndPoint = Point.Empty;

        // Resize handles
        private enum ResizeHandle { None, TopLeft, TopCenter, TopRight, MiddleLeft, MiddleRight, BottomLeft, BottomCenter, BottomRight }
        private ResizeHandle currentResizeHandle = ResizeHandle.None;
        private bool isResizing = false;
        private Point resizeStartPoint = Point.Empty;
        private Rectangle originalRect = Rectangle.Empty;

        private Shape SelectedShape
        {
            get { return drawingManager.GetShapes().FirstOrDefault(s => s.IsSelected); }
        }

        private Point[] GetResizeHandles(Shape shape)
        {
            if (shape == null) return new Point[0];

            Rectangle rect = shape.GetBoundingRectangle();
            return new Point[]
            {
                new Point(rect.Left, rect.Top),          // TopLeft
                new Point(rect.Left + rect.Width / 2, rect.Top),    // TopCenter
                new Point(rect.Right, rect.Top),         // TopRight
                new Point(rect.Left, rect.Top + rect.Height / 2),   // MiddleLeft
                new Point(rect.Right, rect.Top + rect.Height / 2),  // MiddleRight
                new Point(rect.Left, rect.Bottom),       // BottomLeft
                new Point(rect.Left + rect.Width / 2, rect.Bottom), // BottomCenter
                new Point(rect.Right, rect.Bottom)       // BottomRight
            };
        }

        private void SetCursorForHandle(ResizeHandle handle)
        {
            switch (handle)
            {
                case ResizeHandle.TopLeft:
                case ResizeHandle.BottomRight:
                    panel1.Cursor = Cursors.SizeNWSE;
                    break;
                case ResizeHandle.TopRight:
                case ResizeHandle.BottomLeft:
                    panel1.Cursor = Cursors.SizeNESW;
                    break;
                case ResizeHandle.TopCenter:
                case ResizeHandle.BottomCenter:
                    panel1.Cursor = Cursors.SizeNS;
                    break;
                case ResizeHandle.MiddleLeft:
                case ResizeHandle.MiddleRight:
                    panel1.Cursor = Cursors.SizeWE;
                    break;
                default:
                    panel1.Cursor = Cursors.Default;
                    break;
            }
        }

        private void ResizeShape(Point currentPoint)
        {
            if (movingShape == null) return;

            Rectangle rect = originalRect;
            Point delta = new Point(currentPoint.X - resizeStartPoint.X, currentPoint.Y - resizeStartPoint.Y);

            switch (currentResizeHandle)
            {
                case ResizeHandle.TopLeft:
                    movingShape.StartPoint = new Point(rect.Left + delta.X, rect.Top + delta.Y);
                    break;
                case ResizeHandle.TopCenter:
                    movingShape.StartPoint = new Point(movingShape.StartPoint.X, rect.Top + delta.Y);
                    break;
                case ResizeHandle.TopRight:
                    movingShape.StartPoint = new Point(movingShape.StartPoint.X, rect.Top + delta.Y);
                    movingShape.EndPoint = new Point(rect.Right + delta.X, movingShape.EndPoint.Y);
                    break;
                case ResizeHandle.MiddleLeft:
                    movingShape.StartPoint = new Point(rect.Left + delta.X, movingShape.StartPoint.Y);
                    break;
                case ResizeHandle.MiddleRight:
                    movingShape.EndPoint = new Point(rect.Right + delta.X, movingShape.EndPoint.Y);
                    break;
                case ResizeHandle.BottomLeft:
                    movingShape.StartPoint = new Point(rect.Left + delta.X, movingShape.StartPoint.Y);
                    movingShape.EndPoint = new Point(movingShape.EndPoint.X, rect.Bottom + delta.Y);
                    break;
                case ResizeHandle.BottomCenter:
                    movingShape.EndPoint = new Point(movingShape.EndPoint.X, rect.Bottom + delta.Y);
                    break;
                case ResizeHandle.BottomRight:
                    movingShape.EndPoint = new Point(rect.Right + delta.X, rect.Bottom + delta.Y);
                    break;
            }

            // Ensure StartPoint is top-left, EndPoint is bottom-right
            Rectangle newRect = movingShape.GetBoundingRectangle();
            movingShape.StartPoint = new Point(Math.Min(newRect.Left, newRect.Right), Math.Min(newRect.Top, newRect.Bottom));
            movingShape.EndPoint = new Point(Math.Max(newRect.Left, newRect.Right), Math.Max(newRect.Top, newRect.Bottom));
        }

        public Form1()
        {
            InitializeComponent();
            drawingManager = new DrawingManager();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.DoubleBuffered = true; // Kích hoạt Double Buffering cho Form
            panel1.DoubleClick += panel1_DoubleClick;
            gradientDirectionSelect.SelectedIndexChanged += gradientDirectionSelect_SelectedIndexChanged;
            sizeBorder.ValueChanged += sizeBorder_ValueChanged;
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
                // Update selected shape if any
                if (SelectedShape != null && !(SelectedShape is TextShape))
                {
                    SelectedShape.BorderColor = colorBorder;
                    panel1.Invalidate();
                }
            }
        }

        private void colorFillSelect_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                colorFill = colorDialog.Color;
                colorFillSelect.BackColor = colorFill;
                // Update selected shape if any
                if (SelectedShape != null && !(SelectedShape is TextShape))
                {
                    SelectedShape.FillColor = colorFill;
                    SetShapeBrush(SelectedShape);
                    panel1.Invalidate();
                }
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
            // Update selected shape if any
            if (SelectedShape != null && !(SelectedShape is TextShape))
            {
                SetShapeBrush(SelectedShape);
                panel1.Invalidate();
            }
        }

        private void gradientDirectionSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update selected shape if any and fill style is gradient
            if (SelectedShape != null && !(SelectedShape is TextShape) && fillStyleSelect.SelectedItem?.ToString() == "LinearGradientMode")
            {
                SetShapeBrush(SelectedShape);
                panel1.Invalidate();
            }
        }

        private void sizeBorder_ValueChanged(object sender, EventArgs e)
        {
            // Update selected shape if any
            if (SelectedShape != null && !(SelectedShape is TextShape))
            {
                SelectedShape.BorderWidth = (int)sizeBorder.Value;
                panel1.Invalidate();
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

            // Draw resize handles if shape is selected
            var selectedShape = SelectedShape;
            if (selectedShape != null && !(selectedShape is TextShape))
            {
                Point[] handles = GetResizeHandles(selectedShape);
                foreach (var handle in handles)
                {
                    Rectangle handleRect = new Rectangle(handle.X - 5, handle.Y - 5, 10, 10);
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), handleRect);
                    e.Graphics.DrawRectangle(new Pen(Color.Black), handleRect);
                }
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Check if currently editing (resizing or moving)
                bool wasEditing = isResizing || isMoving;

                // First, check for shape selection
                var shapes = drawingManager.GetShapes();
                Shape selectedShape = null;
                for (int i = shapes.Count - 1; i >= 0; i--)
                {
                    if (shapes[i].Contains(e.Location))
                    {
                        selectedShape = shapes[i];
                        break;
                    }
                }
                // Set selection
                foreach (var s in shapes)
                {
                    s.IsSelected = (s == selectedShape);
                }
                // Update controls if shape selected and not text
                if (selectedShape != null && !(selectedShape is TextShape))
                {
                    colorBorder = selectedShape.BorderColor;
                    colorBorderSelect.BackColor = colorBorder;
                    colorFill = selectedShape.FillColor;
                    colorFillSelect.BackColor = colorFill;
                    sizeBorder.Value = selectedShape.BorderWidth;
                    // Note: Fill style not updated for simplicity
                }
                panel1.Invalidate(); // Refresh to show selection

                if (selectedShape != null && !(selectedShape is TextShape))
                {
                    // Check if clicked on resize handle
                    Point[] handles = GetResizeHandles(selectedShape);
                    bool onHandle = false;
                    for (int i = 0; i < handles.Length; i++)
                    {
                        Rectangle handleRect = new Rectangle(handles[i].X - 6, handles[i].Y - 6, 12, 12);
                        if (handleRect.Contains(e.Location))
                        {
                            onHandle = true;
                            currentResizeHandle = (ResizeHandle)(i + 1);
                            isResizing = true;
                            movingShape = selectedShape;
                            resizeStartPoint = e.Location;
                            originalRect = selectedShape.GetBoundingRectangle();
                            SetCursorForHandle(currentResizeHandle);
                            break;
                        }
                    }
                    if (!onHandle)
                    {
                        // Start moving the selected shape
                        isMoving = true;
                        movingShape = selectedShape;
                        moveOffset = new Point(e.X - selectedShape.StartPoint.X, e.Y - selectedShape.StartPoint.Y);
                        originalDelta = new Point(selectedShape.EndPoint.X - selectedShape.StartPoint.X, selectedShape.EndPoint.Y - selectedShape.StartPoint.Y);
                    }
                }
                else if (selectedShape != null && selectedShape is TextShape)
                {
                    // For text, allow moving
                    isMoving = true;
                    movingShape = selectedShape;
                    moveOffset = new Point(e.X - selectedShape.StartPoint.X, e.Y - selectedShape.StartPoint.Y);
                    originalDelta = new Point(selectedShape.EndPoint.X - selectedShape.StartPoint.X, selectedShape.EndPoint.Y - selectedShape.StartPoint.Y);
                }
                else
                {
                    // No shape selected, proceed to drawing only if not currently editing
                    if (!wasEditing)
                    {
                        string shapeType = shapeSelect.SelectedItem?.ToString();
                        if (shapeType == "Text")
                        {
                            // Show text input dialog
                            TextInputDialog dialog = new TextInputDialog();
                            if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(dialog.InputText))
                            {
                                TextShape textShape = new TextShape();
                                textShape.StartPoint = e.Location;
                                textShape.EndPoint = e.Location; // Same point for text
                                textShape.Text = dialog.InputText;
                                textShape.Font = dialog.SelectedFont;
                                textShape.TextColor = dialog.SelectedColor;
                                // Note: Border and fill not used for text, but set anyway for consistency
                                textShape.BorderColor = colorBorder;
                                textShape.BorderWidth = (int)sizeBorder.Value;
                                textShape.FillColor = colorFill;
                                SetShapeBrush(textShape); // Though not used

                                drawingManager.AddShape(textShape);
                                // Auto-select the newly drawn text
                                var allShapes = drawingManager.GetShapes();
                                foreach (var s in allShapes)
                                {
                                    s.IsSelected = (s == textShape);
                                }
                                panel1.Invalidate();
                            }
                        }
                        else
                        {
                            isDrawing = true;
                            startPoint = e.Location;
                        }
                    }
                }
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizing && movingShape != null)
            {
                ResizeShape(e.Location);
                SetShapeBrush(movingShape);
                SetCursorForHandle(currentResizeHandle);
                panel1.Invalidate();
            }
            else if (isMoving && movingShape != null)
            {
                Point newStart = new Point(e.X - moveOffset.X, e.Y - moveOffset.Y);
                movingShape.StartPoint = newStart;
                movingShape.EndPoint = new Point(newStart.X + originalDelta.X, newStart.Y + originalDelta.Y);
                panel1.Cursor = Cursors.SizeAll;
                panel1.Invalidate();
            }
            else if (isDrawing)
            {
                Point endPoint = e.Location;
                lastEndPoint = endPoint;
                Shape previewShape = CreateShape(startPoint, endPoint);
                if (previewShape != null)
                {
                    drawingManager.SetPreviewShape(previewShape);
                }
                panel1.Cursor = Cursors.Cross;
                panel1.Invalidate();
            }

            // Check for resize handles
            var selectedShape = SelectedShape;
            if (selectedShape != null && !(selectedShape is TextShape))
            {
                Point[] handles = GetResizeHandles(selectedShape);
                bool overHandle = false;
                for (int i = 0; i < handles.Length; i++)
                {
                    Rectangle handleRect = new Rectangle(handles[i].X - 6, handles[i].Y - 6, 12, 12);
                    if (handleRect.Contains(e.Location))
                    {
                        overHandle = true;
                        currentResizeHandle = (ResizeHandle)(i + 1); // Enum starts from 1
                        SetCursorForHandle(currentResizeHandle);
                        break;
                    }
                }
                if (!overHandle)
                {
                    currentResizeHandle = ResizeHandle.None;
                    panel1.Cursor = Cursors.Default;
                }
            }
            else
            {
                currentResizeHandle = ResizeHandle.None;
                panel1.Cursor = Cursors.Default;
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (isResizing)
                {
                    isResizing = false;
                    movingShape = null;
                    currentResizeHandle = ResizeHandle.None;
                }
                else if (isMoving)
                {
                    isMoving = false;
                    movingShape = null;
                }
                else if (isDrawing)
                {
                    isDrawing = false;
                    Shape finalShape = CreateShape(startPoint, lastEndPoint);
                    if (finalShape != null)
                    {
                        drawingManager.AddShape(finalShape);
                        // Auto-select the newly drawn shape
                        var allShapes = drawingManager.GetShapes();
                        foreach (var s in allShapes)
                        {
                            s.IsSelected = (s == finalShape);
                        }
                    }
                    drawingManager.ClearPreviewShape();
                    panel1.Invalidate();
                }
            }
        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            var allShapes = drawingManager.GetShapes();
            foreach (var shape in allShapes)
            {
                if (shape.IsSelected && shape is TextShape textShape)
                {
                    // Open dialog with current values
                    TextInputDialog dialog = new TextInputDialog();
                    dialog.InputText = textShape.Text;
                    dialog.SelectedFont = textShape.Font;
                    dialog.SelectedColor = textShape.TextColor;
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        textShape.Text = dialog.InputText;
                        textShape.Font = dialog.SelectedFont;
                        textShape.TextColor = dialog.SelectedColor;
                        panel1.Invalidate();
                    }
                    break;
                }
            }
        }

        private Shape CreateShape(Point start, Point end)
        {
            if (shapeSelect.SelectedIndex < 0)
                return null;

            string shapeType = shapeSelect.SelectedItem.ToString();
            Shape shape = null;

            switch (shapeType)
            {
                case "Line":
                    shape = new LineShape();
                    break;
                case "Rectangle":
                    shape = new RectangleShape();
                    break;
                case "Ellipse":
                    shape = new EllipseShape();
                    break;
                case "Triangle":
                    shape = new TriangleShape();
                    break;
                case "Parallelogram":
                    shape = new ParallelogramShape();
                    break;
                case "Text":
                    shape = new TextShape();
                    break;
            }

            if (shape != null)
            {
                shape.StartPoint = start;
                shape.EndPoint = end;
                shape.BorderColor = colorBorder;
                shape.BorderWidth = (int)sizeBorder.Value;
                shape.FillColor = colorFill;
                
                // Set brush based on fill style
                SetShapeBrush(shape);
            }

            return shape;
        }

        private void SetShapeBrush(Shape shape)
        {
            string fillStyle = fillStyleSelect.SelectedItem?.ToString() ?? "Solid";
            
            switch (fillStyle)
            {
                case "Solid":
                    shape.Brush = new SolidBrush(colorFill);
                    break;
                case "LinearGradientMode":
                    var rect = shape.GetBoundingRectangle();
                    if (rect.Width > 0 && rect.Height > 0)
                    {
                        LinearGradientMode mode = GetGradientMode();
                        shape.Brush = new LinearGradientBrush(rect, colorBorder, colorFill, mode);
                    }
                    else
                    {
                        shape.Brush = new SolidBrush(colorFill);
                    }
                    break;
                case "PathGradientBrush":
                    var rect2 = shape.GetBoundingRectangle();
                    if (rect2.Width > 0 && rect2.Height > 0)
                    {
                        Point centerPoint = new Point(rect2.X + rect2.Width / 2, rect2.Y + rect2.Height / 2);
                        Point[] points = new Point[] {
                            new Point(rect2.X, rect2.Y),
                            new Point(rect2.Right, rect2.Y),
                            new Point(rect2.Right, rect2.Bottom),
                            new Point(rect2.X, rect2.Bottom)
                        };
                        shape.Brush = new PathGradientBrush(points);
                        ((PathGradientBrush)shape.Brush).CenterColor = colorBorder;
                        ((PathGradientBrush)shape.Brush).SurroundColors = new Color[] { colorFill };
                    }
                    else
                    {
                        shape.Brush = new SolidBrush(colorFill);
                    }
                    break;
                case "HatchBrush":
                    shape.Brush = new HatchBrush(HatchStyle.Cross, colorBorder, colorFill);
                    break;
                default:
                    shape.Brush = new SolidBrush(colorFill);
                    break;
            }
        }

        private LinearGradientMode GetGradientMode()
        {
            string direction = gradientDirectionSelect.SelectedItem?.ToString() ?? "Horizontal";
            
            switch (direction)
            {
                case "BackwardDiagonal":
                    return LinearGradientMode.BackwardDiagonal;
                case "ForwardDiagonal":
                    return LinearGradientMode.ForwardDiagonal;
                case "Vertical":
                    return LinearGradientMode.Vertical;
                default:
                    return LinearGradientMode.Horizontal;
            }
        }
    }
}
