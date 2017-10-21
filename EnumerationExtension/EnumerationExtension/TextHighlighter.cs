using System.Windows.Controls;
using System.Windows.Media;

namespace EnumerationExtension
{
    public class TextHighlighter
    {
        private readonly Brush _backgroundBrush;        
        private readonly Pen _borderPen;

        public TextHighlighter()
        {
            _backgroundBrush = new SolidColorBrush(Color.FromArgb(0x20, 0x00, 0xee, 0x22));
            _backgroundBrush.Freeze();

            var _penBrush = new SolidColorBrush(Colors.Red);
            _penBrush.Freeze();
            _borderPen = new Pen(_penBrush, 0.5);
            _borderPen.Freeze();
        }

        public Image Highlight(Geometry geometry)
        {
            var drawing = new GeometryDrawing(_backgroundBrush, _borderPen, geometry);
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
