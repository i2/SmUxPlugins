using System.Drawing;

namespace Diagnostic
{
    public static class RectangeHelper
    {
        public static Point GetMiddlePoint(this Rectangle rectangle)
        {
            return new Point(rectangle.Location.X + rectangle.Width/2, rectangle.Location.Y + rectangle.Height/2);
        }
    }
}