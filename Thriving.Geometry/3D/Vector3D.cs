using System.Runtime.InteropServices;

namespace Thriving.Geometry
{
    /// <summary>
    /// 三维向量
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3D
    {
        private double _x, _y, _z;

        public Vector3D(double x, double y, double z)
        {
            this._x = x;
            this._y = y;
            this._z = z;
        }

        public Vector3D(Point3D pA, Point3D pB)
        {
            this._x = pB.X - pA.X;
            this._y = pB.Y - pA.Y;
            this._z = pB.Z - pA.Z;
        }

        public Vector3D(Vector2D vector, double z = 0)
        {
            this._x = vector.X;
            this._y = vector.Y;
            this._z = z;
        }

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

        public static Vector3D BasisX { get => new Vector3D(1, 0, 0); }
        public static Vector3D BasisY { get => new Vector3D(0, 1, 0); }
        public static Vector3D BasisZ { get => new Vector3D(0, 0, 1); }

        internal readonly Matrix Matrix
        {
            get => new Matrix(new double[,] { { _x }, { _y }, { _z }, { 1 } });
        }

        public readonly double Length
        {
            get => Math.Sqrt(_x * _x + _y * _y + _z * _z);
        }

        /// <summary>
        /// 长度平方
        /// </summary>
        public readonly double SquareLength
        {
            get => _x * _x + _y * _y + _z * _z;
        }

        public readonly Vector3D Negate()
        {
            return new Vector3D(-_x, -_y, -_z);
        }

        public readonly Vector3D Normalize()
        {
            return new Vector3D(_x / Length, _y / Length, _z / Length);
        }

        /// <summary>
        /// 点积
        /// </summary>
        /// <param name="other"></param>
        /// <returns>
        /// <para>大于0，向量夹角在0-90度之间</para>
        /// <para>等于0，两向量垂直</para>
        /// <para>大于0，向量夹角在90-180度之间</para>
        /// </returns>
        public readonly double DotProduct(Vector3D other)
        {
            return _x * other._x + _y * other._y + _z * other._z;
        }

        /// <summary>
        /// 叉积
        /// </summary>
        /// <param name="other"></param>
        /// <returns>
        /// <para>方向为：两向量构成平面的法向量</para>
        /// <para>长度为：|a|*|b|*(sin)</para>
        /// </returns>
        public readonly Vector3D CrossProduct(Vector3D other)
        {
            return new Vector3D(
                _y * other._z - _z * other._y,
                _z * other._x - _x * other._z,
                _x * other._y - _y * other._x);
        }

        /// <summary>
        /// 同向且长度相同
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly bool IsAlmostEqualTo(Vector3D other)
        {
            var v1 = DotProduct(other);
            var v2 = CrossProduct(other);
            // 点积大于0且叉积等于0
            return v1 > 0 && v2.Length < GeometryUtility.Tolerance;
        }

        /// <summary>
        /// 余弦定理求夹角
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly double AngleTo(Vector3D other)
        {
            var temp = DotProduct(other) / (Length * other.Length);
            if (temp >= 1) { return 0; }
            if (temp <= -1) { return Math.PI; }
            return Math.Acos(temp);
        }

        /// <summary>
        /// 逆时针方向夹角
        /// </summary>
        /// <param name="other"></param>
        /// <param name="normal">两向量平面的正方向</param>
        /// <returns></returns>
        public readonly double CcwAngleTo(Vector3D other, Vector3D normal)
        {
            var angle = AngleTo(other);
            var nor = CrossProduct(other).Normalize();
            if (nor.IsAlmostEqualTo(normal))
            {
                return 2 * Math.PI - angle;
            }
            else
            {
                return angle;
            }
        }

        public static Vector3D operator +(Vector3D vectorA, Vector3D vectorB)
        {
            return new Vector3D(vectorA.X + vectorB.X, vectorA.Y + vectorB.Y, vectorA.Z + vectorB.Z);
        }

        public static Vector3D operator -(Vector3D vectorA, Vector3D vectorB)
        {
            return new Vector3D(vectorA.X - vectorB.X, vectorA.Y - vectorB.Y, vectorA.Z - vectorB.Z);
        }

        public static Vector3D operator *(double value, Vector3D vector)
        {
            return new Vector3D(value * vector.X, value * vector.Y, value * vector.Z);
        }

        public static Vector3D operator *(Vector3D vector, double value)
        {
            return new Vector3D(value * vector.X, value * vector.Y, value * vector.Z);
        }
    }
}