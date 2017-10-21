using System.Windows.Controls;
using System.Windows.Media;

namespace EnumerationExtension
{
    public class TextHighlighter
    {
        /// <summary>
        /// Adornment brush.
        /// </summary>
        private readonly Brush _brush;

        /// <summary>
        /// Adornment pen.
        /// </summary>
        private readonly Pen _pen;

        public TextHighlighter()
        {
            // Create the pen and brush to color the box behind the a's
            _brush = new SolidColorBrush(Color.FromArgb(0x20, 0x00, 0x00, 0xee));
            _brush.Freeze();

            var _penBrush = new SolidColorBrush(Colors.Red);
            _penBrush.Freeze();
            _pen = new Pen(_penBrush, 0.5);
            _pen.Freeze();
        }

        public Image Highlight(Geometry geometry)
        {
            var drawing = new GeometryDrawing(_brush, _pen, geometry);
            drawing.Freeze();

            var drawingImage = new DrawingImage(drawing);
            drawingImage.Freeze();

            var image = new Image
            {
                Source = drawingImage,
            };

            // Align the image with the top of the bounds of the text geometry
            Canvas.SetLeft(image, geometry.Bounds.Left);
            Canvas.SetTop(image, geometry.Bounds.Top);
            return image;
        }
    }
}
