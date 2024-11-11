using System.Runtime.InteropServices;

namespace Thriving.Geometry
{
    /// <summary>
    /// 二维向量
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2D
    {
        private double _x;
        private double _y;

        public Vector2D(double x = 0, double y = 0)
        {
            this._x = x;
            this._y = y;
        }

        public Vector2D(Point2D pA, Point2D pB)
        {
            this._x = pB.X - pA.X;
            this._y = pB.Y - pA.Y;
        }

        public static Vector2D BasisX { get => new Vector2D(1, 0); }
        public static Vector2D BasisY { get => new Vector2D(0, 1); }

        /// <summary>
        /// 逆向量
        /// </summary>
        public readonly Vector2D Negate()
        {
            return new Vector2D(-_x, -_y);
        }

        /// <summary>
        /// 逆时针方向垂直向量
        /// </summary>
        public readonly Vector2D Vertical()
        {
            return new Vector2D(-_y, _x);
        }

        public readonly Vector2D Normalize()
        {
            return new Vector2D(_x / Length, _y / Length);
        }

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

        public readonly double Length { get => Math.Sqrt(_x * _x + _y * _y); }

        /// <summary>
        /// 长度平方
        /// </summary>
        public readonly double SquareLength
        {
            get => _x * _x + _y * _y;
        }

        internal readonly Matrix Matrix
        {
            get => new Matrix(new double[,] { { _x }, { _y }, { 1 } });
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
        public readonly double DotProduct(Vector2D other)
        {
            return _x * other._x + _y * other._y;
        }

        /// <summary>
        /// 余弦定理求夹角，[0,Pi]
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly double AngleTo(Vector2D other)
        {
            var temp = DotProduct(other) / (other.Length * this.Length);
            if (temp >= 1) { return 0; }
            if (temp <= -1) { return Math.PI; }
            return Math.Acos(temp);
        }

        /// <summary>
        /// 逆时针方向角度[0,2*Pi]
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly double CcwAngleTo(Vector2D other)
        {
            var angle = AngleTo(other);
            if (IsCounterClockwise(other))
            {
                return angle;
            }
            else
            {
                return 2 * Math.PI - angle;
            }
        }

        /// <summary>
        /// 同向且长度相同
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly bool IsEqualTo(Vector2D other)
        {
            var v1 = DotProduct(other);
            var v2 = this._x * other._y - this._y * other._x;
            // 点积大于0且叉积等于0
            return v1 > 0 && v2 == 0;
        }

        /// <summary>
        /// 是否在逆时针180度范围内
        /// </summary>
        public readonly bool IsCounterClockwise(Vector2D other)
        {
            // 叉积的Z轴大小
            return this._x * other.Y >= this._y * other.X;
        }

        public static Vector2D operator +(Vector2D vectorA, Vector2D vectorB)
        {
            return new Vector2D(vectorA.X + vectorB.X, vectorA.Y + vectorB.Y);
        }

        public static Vector2D operator -(Vector2D vectorA, Vector2D vectorB)
        {
            return new Vector2D(vectorA.X - vectorB.X, vectorA.Y - vectorB.Y);
        }

        public static Vector2D operator *(double value, Vector2D vector)
        {
            return new Vector2D(value * vector.X, value * vector.Y);
        }

        public static Vector2D operator *(Vector2D vector, double value)
        {
            return new Vector2D(value * vector.X, value * vector.Y);
        }
    }
}