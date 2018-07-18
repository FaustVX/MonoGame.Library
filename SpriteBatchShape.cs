using Microsoft.Xna.Framework;
using System;

namespace MonoGame.Library
{
    public class SpriteBatchShape : IDisposable
    {
        public SpriteBatchShape(SpriteBatchDrawer spriteBatch, int width, Color color, bool close = false)
        {
            Width = width;
            Color = color;
            CloseToFirst = close;

            _drawer = spriteBatch;
            _started = _closed = false;
            _start = _last = default;
        }

        public SpriteBatchShape(SpriteBatchDrawer spriteBatch, Point start, int width, Color color, bool close = false)
            : this(spriteBatch, width, color, close)
        {
            _started = true;
            _start = _last = start;
        }

        public int Width { get; set; }
        public Color Color { get; set; }
        public bool CloseToFirst { get; set; }

        private readonly SpriteBatchDrawer _drawer;
        private bool _started, _closed;
        private Point _start;
        private Point _last;

        public SpriteBatchShape Next(Point point)
            => Draw(_last, _last = point);

        public SpriteBatchShape MoveTo(Point point)
        {
            _last = point;
            return this;
        }

        public void DrawCircle(Point center, int radius, int nbFaces)
        {
            var angleRad = (Math.PI * 2) / nbFaces;
            for (int i = 0; i < nbFaces; i++)
            {
                var sum = angleRad * i;
                var polar = new PolarCoordinate(radius, sum);
                var (x, y) = polar.ToCartesian();
                Next(new Point(x, y) + center);
            }
        }

        public void DrawRectangle(Rectangle rectangle)
        {
            Next(new Point(rectangle.Left, rectangle.Top));
            Next(new Point(rectangle.Right, rectangle.Top));
            Next(new Point(rectangle.Right, rectangle.Bottom));
            Next(new Point(rectangle.Left, rectangle.Bottom));
        }

        public void DrawRectangle(Point center, Point relativCorner)
        {
            var rectangle = new Rectangle(center.X - Math.Abs(relativCorner.X), center.Y - Math.Abs(relativCorner.Y), Math.Abs(relativCorner.X) * 2, Math.Abs(relativCorner.Y) * 2);
            DrawRectangle(rectangle);
        }

        public void Close()
        {
            if (!_closed)
                Draw(_last, _start);
            _closed = true;
        }

        private SpriteBatchShape Draw(Point previous, Point current)
        {
            if (_closed)
                return null;
            if (_started)
                _drawer.DrawLine(previous, current, Width, Color);
            else
                _start = current;
            _started = true;
            return this;
        }

        void IDisposable.Dispose()
        {
            if (CloseToFirst)
                Close();
            _closed = true;
        }
    }
}
