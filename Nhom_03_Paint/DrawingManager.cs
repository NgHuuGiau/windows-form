using Nhom_03_Paint.Shapes;
using System.Drawing.Drawing2D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Nhom_03_Paint
{
    internal class DrawingManager
    {
        private List<Shape> shapes = new List<Shape>();

        private Stack<List<Shape>> undoStack = new Stack<List<Shape>>();
        private Stack<List<Shape>> redoStack = new Stack<List<Shape>>();
        private const int MaxHistorySize = 50;

        private Shape previewShape;

        private Bitmap backBuffer;
        private Graphics backGraphics;

        private Image backgroundImage = null;

        public void SetPreviewShape(Shape s)
        {
            if (previewShape != null)
            {
                try { previewShape.Brush?.Dispose(); } catch { }
                previewShape.IsSelected = false;
            }
            previewShape = s;
            if (previewShape != null)
            {
                previewShape.IsSelected = true;
            }
        }

        public void ClearPreviewShape()
        {
            if (previewShape != null)
            {
                try { previewShape.Brush?.Dispose(); } catch { }
                previewShape.IsSelected = false;
                previewShape = null;
            }
        }

        public void AddShape(Shape shape)
        {
            if (shape != null)
            {
                SaveState();
                shapes.Add(shape);
                redoStack.Clear();
            }
        }

        public void RemoveLastShape()
        {
            if (shapes.Count > 0)
            {
                var idx = shapes.Count - 1;
                try { shapes[idx].Brush?.Dispose(); } catch { }
                shapes.RemoveAt(idx);
            }
        }

        public void ClearAll()
        {
            foreach (var s in shapes)
            {
                try { s.Brush?.Dispose(); } catch { }
            }
            shapes.Clear();
            
            // Xóa cả ảnh nền khi làm mới
            if (backgroundImage != null)
            {
                backgroundImage.Dispose();
                backgroundImage = null;
            }
        }

        public void SetBackgroundImage(Image image)
        {
            if (backgroundImage != null)
            {
                backgroundImage.Dispose();
            }
            backgroundImage = new Bitmap(image);
        }

        public bool DeleteSelectedShape()
        {
            for (int i = shapes.Count - 1; i >= 0; i--)
            {
                if (shapes[i].IsSelected)
                {
                    SaveState();
                    try { shapes[i].Brush?.Dispose(); } catch { }
                    shapes.RemoveAt(i);
                    redoStack.Clear();
                    return true;
                }
            }
            return false;
        }

        public bool Undo()
        {
            if (undoStack.Count == 0) return false;
            redoStack.Push(CloneShapes(shapes));
            shapes = CloneShapes(undoStack.Pop());
            return true;
        }

        public bool Redo()
        {
            if (redoStack.Count == 0) return false;
            undoStack.Push(CloneShapes(shapes));
            shapes = CloneShapes(redoStack.Pop());
            return true;
        }

        private void SaveState()
        {
            undoStack.Push(CloneShapes(shapes));
            if (undoStack.Count > MaxHistorySize)
            {
                // Convert stack to list (oldest at index 0), keep only the newest MaxHistorySize
                var temp = new List<List<Shape>>(undoStack);
                temp.Reverse(); // Now oldest at index 0
                temp = temp.Skip(temp.Count - MaxHistorySize).ToList();
                undoStack.Clear();
                // Push back in correct order (newest on top)
                foreach (var state in temp)
                {
                    undoStack.Push(state);
                }
            }
        }

        private List<Shape> CloneShapes(List<Shape> source)
        {
            var clone = new List<Shape>();
            foreach (var s in source)
            {
                var newShape = CloneShape(s);
                if (newShape != null) clone.Add(newShape);
            }
            return clone;
        }

        public Shape CloneShape(Shape source)
        {
            Shape clone = null;
            switch (source)
            {
                case LineShape _:
                    clone = new LineShape();
                    break;
                case RectangleShape _:
                    clone = new RectangleShape();
                    break;
                case EllipseShape _:
                    clone = new EllipseShape();
                    break;
                case TriangleShape _:
                    clone = new TriangleShape();
                    break;
                case ParallelogramShape _:
                    clone = new ParallelogramShape();
                    break;
                case SquareShape _:
                    clone = new SquareShape();
                    break;
                case TextShape _:
                    clone = new TextShape();
                    break;
                default:
                    return null;
            }

            clone.StartPoint = source.StartPoint;
            clone.EndPoint = source.EndPoint;
            clone.BorderColor = source.BorderColor;
            clone.FillColor = source.FillColor;
            clone.BorderWidth = source.BorderWidth;
            clone.RotationAngle = source.RotationAngle;
            clone.IsSelected = source.IsSelected;

            if (source.Brush is LinearGradientBrush lgb)
            {
                clone.Brush = lgb.Clone() as LinearGradientBrush;
            }
            else if (source.Brush is PathGradientBrush pgb)
            {
                clone.Brush = pgb.Clone() as PathGradientBrush;
            }
            else if (source.Brush is HatchBrush hb)
            {
                clone.Brush = hb.Clone() as HatchBrush;
            }
            else if (source.Brush is TextureBrush tb)
            {
                // Clone the underlying image to ensure the pasted shape retains the texture
                try
                {
                    Image imgClone = new Bitmap(tb.Image);
                    var newTb = new TextureBrush(imgClone);
                    newTb.WrapMode = tb.WrapMode;
                    clone.Brush = newTb;
                }
                catch
                {
                    // Fallback to solid brush if cloning the texture fails
                    clone.Brush = new SolidBrush(source.FillColor);
                }
            }
            else
            {
                clone.Brush = new SolidBrush(source.FillColor);
            }

            if (source is TextShape textSrc && clone is TextShape textClone)
            {
                textClone.Text = textSrc.Text;
                textClone.Font = textSrc.Font;
                textClone.TextColor = textSrc.TextColor;
            }

            return clone;
        }

        public List<Shape> GetShapes()
        {
            return shapes;
        }

        public void DrawAll(Graphics g, int width, int height)
        {
            if (backBuffer == null || backBuffer.Width != width || backBuffer.Height != height)
            {
                backBuffer?.Dispose();
                backBuffer = new Bitmap(width, height);
                backGraphics?.Dispose();
                backGraphics = Graphics.FromImage(backBuffer);
            }

            backGraphics.Clear(Color.White);

            // Vẽ ảnh nền trước (nếu có)
            if (backgroundImage != null)
            {
                backGraphics.DrawImage(backgroundImage, 0, 0, width, height);
            }

            foreach (var shape in shapes)
            {
                if (shape != null)
                {
                    var state = backGraphics.Save();

                    if (shape.RotationAngle != 0)
                    {
                        Rectangle bounds = shape.GetBoundingRectangle();
                        float centerX = bounds.X + bounds.Width / 2f;
                        float centerY = bounds.Y + bounds.Height / 2f;

                        backGraphics.TranslateTransform(centerX, centerY);
                        backGraphics.RotateTransform(shape.RotationAngle);
                        backGraphics.TranslateTransform(-centerX, -centerY);
                    }

                    shape.Draw(backGraphics);

                    backGraphics.Restore(state);
                }
            }

            foreach (var shape in shapes)
            {
                if (shape != null && shape.IsSelected)
                {
                    shape.DrawHandles(backGraphics);
                }
            }

            if (previewShape != null)
            {
                var state = backGraphics.Save();
                if (previewShape.RotationAngle != 0)
                {
                    Rectangle bounds = previewShape.GetBoundingRectangle();
                    float centerX = bounds.X + bounds.Width / 2f;
                    float centerY = bounds.Y + bounds.Height / 2f;

                    backGraphics.TranslateTransform(centerX, centerY);
                    backGraphics.RotateTransform(previewShape.RotationAngle);
                    backGraphics.TranslateTransform(-centerX, -centerY);
                }

                previewShape.Draw(backGraphics);
                backGraphics.Restore(state);
            }

            g.DrawImageUnscaled(backBuffer, 0, 0);
        }

        public bool SaveImage(string filePath, Image imageToSave)
        {
            try
            {
                if (imageToSave == null)
                    return false;

                string ext = Path.GetExtension(filePath).ToLower();

                switch (ext)
                {
                    case ".jpg":
                    case ".jpeg":
                        imageToSave.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case ".png":
                        imageToSave.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case ".bmp":
                        imageToSave.Save(filePath, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    default:
                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lưu file: {ex.Message}", "Error");
                return false;
            }
        }

        public void Dispose()
        {
            if (previewShape != null)
            {
                try { previewShape.Brush?.Dispose(); } catch { }
                previewShape = null;
            }

            foreach (var s in shapes)
            {
                try { s.Brush?.Dispose(); } catch { }
            }

            backGraphics?.Dispose();
            backBuffer?.Dispose();
        }

        public int GetShapeCount()
        {
            return shapes.Count;
        }
    }
}
