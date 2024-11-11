using System.Runtime.InteropServices;

namespace Thriving.Geometry
{
    /// <summary>
    /// 三维点
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Point3D
    {
        private double _x, _y, _z;

        public double X
        {
            readonly get { return _x; }
            set { _x = value; }
        }

        public double Y
        {
            readonly get { return _y; }
            set { _y = value; }
        }

        public double Z
        {
            readonly get { return _z; }
            set { _z = value; }
        }

        public Point3D()
        {
            this._x = 0;
            this._y = 0;
            this._z = 0;
        }

        public Point3D(double x, double y, double z)
        {
            this._x = x;
            this._y = y;
            this._z = z;
        }

        public Point3D(Point2D point, double z = 0)
        {
            this._x = point.X;
            this._y = point.Y;
            this._z = z;
        }

        internal readonly Matrix Matrix
        {
            get => new Matrix(new double[,] { { _x }, { _y }, { _z }, { 1 } });
        }

        public readonly double DistanceTo(Point3D other)
        {
            return Math.Sqrt((other._x - this._x) * (other._x - this._x) + (other._y - this._y) * (other._y - this._y) + (other._z - this._z) * (other._z - this._z));
        }

        /// <summary>
        /// 距离平方
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly double SquareDistanceTo(Point3D other)
        {
            return (other._x - this._x) * (other._x - this._x) + (other._y - this._y) * (other._y - this._y) + (other._z - this._z) * (other._z - this._z);
        }

        /// <summary>
        /// XY平面投影距离
        /// </summary>
        public readonly double ProjectDistanceTo(Point3D p2)
        {
            return Math.Sqrt((p2.X - this._x) * (p2.X - this._x) + (p2.Y - this._y) * (p2.Y - this._y));
        }

        public static Point3D Zero { get { return new Point3D(); } }

        public readonly Vector3D Vector { get { return new Vector3D(_x, _y, _z); } }

        public readonly Point3D Add(Vector3D vector)
        {
            return new Point3D(this._x + vector.X, this._y + vector.Y, this._z + vector.Z);
        }

        public readonly Point3D Subtract(Vector3D vector)
        {
            return new Point3D(this._x - vector.X, this._y - vector.Y, this._z - vector.Z);
        }

        public readonly Point3D Scale(double factor)
        {
            return new Point3D(factor * _x, factor * _y, factor * _z);
        }

        public readonly bool IsAlmostEqualTo(Point3D point, double tolerance = 0.01)
        {
            var d = DistanceTo(point);
            return d < tolerance;
        }

        public override readonly string ToString()
        {
            return $"({_x},{_y},{_z})";
        }
    }
}