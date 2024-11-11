using System.Runtime.InteropServices;

namespace Thriving.Geometry
{
    /// <summary>
    /// 二维点
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Point2D
    {
        private double _x, _y;

        public Point2D()
        {
            _x = 0;
            _y = 0;
        }

        public Point2D(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public static Point2D Zero { get => new Point2D(); }

        public readonly Vector2D Vector { get => new Vector2D(_x, _y); }

        public double X
        {
            readonly get => _x;
            set { _x = value; }
        }

        public double Y
        {
            readonly get => _y;
            set { _y = value; }
        }

        internal readonly Matrix Matrix
        {
            get => new Matrix(new double[,] { { _x }, { _y }, { 1 } });
        }

        /// <summary>
        /// 点到点的距离
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public readonly double DistanceTo(Point2D point)
        {
            return Math.Sqrt((point._y - this._y) * (point._y - this._y) + (point.X - this._x) * (point.X - this._x));
        }

        public readonly bool IsAlmostEqualTo(Point2D point, double tolerance = 0.01)
        {
            var d = DistanceTo(point);
            return d <= tolerance;
        }

        public readonly Point2D Add(Vector2D vector)
        {
            return new Point2D(_x + vector.X, _y + vector.Y);
        }

        public readonly Point2D Subtract(Vector2D vector)
        {
            return new Point2D(this._x - vector.X, this._y - vector.Y);
        }

        public readonly Point2D Scale(double factor)
        {
            return new Point2D(factor * _x, factor * _y);
        }
    }
}