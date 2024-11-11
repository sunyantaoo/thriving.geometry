using System.Runtime.InteropServices;

namespace Thriving.Geometry
{
    /// <summary>
    /// 直线
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Line2D
    {
        /// <summary>
        /// 方程式Ax+By=C;
        /// </summary>
        private readonly double _a, _b, _c;

        /// <summary>
        /// Ax+By=C
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        public Line2D(double a, double b, double c)
        {
            this._a = a;
            this._b = b;
            this._c = c;
        }

        /// <summary>
        /// y=kx+b
        /// </summary>
        /// <param name="k"></param>
        /// <param name="b"></param>
        public Line2D(double k, double b)
        {
            this._a = k;
            this._b = -1;
            this._c = -b;
        }

        /// <summary>
        /// y-v=k(x-u)
        /// </summary>
        /// <param name="k"></param>
        /// <param name="point"></param>
        public Line2D(double k, Point2D point)
        {
            this._a = k;
            this._b = -1;
            this._c = k * point.X - point.Y;
        }

        public Line2D(Point2D point, Vector2D dir)
        {
            this._a = dir.Y;
            this._b = -dir.X;
            this._c = dir.Y * point.X - dir.X * point.Y;
        }

        public Line2D(Point2D pointA, Point2D pointB)
        {
            if (Math.Abs(pointA.X - pointB.X) < GeometryUtility.Tolerance)
            {
                this._a = 1;
                this._b = 0;
                this._c = 0.5 * (pointA.X + pointB.X);
            }
            else if (Math.Abs(pointA.Y - pointB.Y) < GeometryUtility.Tolerance)
            {
                this._a = 0;
                this._b = 1;
                this._c = 0.5 * (pointA.Y + pointB.Y);
            }
            else
            {
                this._a = 1 / (pointB.X - pointA.X);
                this._b = -1 / (pointB.Y - pointA.Y);
                this._c = (pointA.X / (pointB.X - pointA.X)) - (pointA.Y / (pointB.Y - pointA.Y));
            }
        }


        public Point2D? GetPointByX(double x)
        {
            if (_b != 0)
            {
                return new Point2D(x, (_c - _a * x) / _b);
            }
            else
            {
                return null;
            }
        }

        public Point2D? GetPointByY(double y)
        {
            if (_a != 0)
            {
                return new Point2D((_c - _b * y) / _a, y);
            }
            else
            {
                return null;
            }
        }

        public Vector2D Direction
        {
            get
            {
                return (new Vector2D(-_b, _a)).Normalize();
            }
        }

        /// <summary>
        /// 点到线的距离
        /// </summary>
        /// <param name="point"></param>
        /// <returns>正值则点位于直线前进方向右侧，负值为左侧</returns>
        public double DistanceTo(Point2D point)
        {
            return (_a * point.X + _b * point.Y - _c) / Math.Sqrt(_a * _a + _b * _b);
        }

        public ProjectionResult<Point2D> Projection(Point2D point)
        {
            Point2D oP; double d;
            if (Math.Abs(_a) < GeometryUtility.Tolerance)
            {
                d = point.Y - (_c / _b);
                oP = new Point2D(point.X, _c / _b);
            }
            else if (Math.Abs(_b) < GeometryUtility.Tolerance)
            {
                d = point.X - (_c / _a);
                oP = new Point2D(_c / _a, point.Y);
            }
            else
            {
                var pA = GetPointByX(point.X);
                d = DistanceTo(point);

                var lA = _a * d / _b;
                oP = pA.Value.Add(lA * Direction);
            }

            return new ProjectionResult<Point2D>()
            {
                Point = oP,
                Distance = d
            };
        }

        /// <summary>
        /// 点是否在线上
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsInner(Point2D point)
        {
            return this._c == this._a * point.X + this._b * point.Y;
        }

        /// <summary>
        /// 是否平行
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsParallel(Line2D other)
        {
            return this._a * other._b == other._a * this._b;
        }

        /// <summary>
        /// 求交点，直线平行时为空
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Point2D? Intersect(Line2D other)
        {
            if (IsParallel(other)) return null;

            var x = (other._c * this._b - other._b * this._c) / (other._a * this._b - other._b * this._a);

            double y;
            if (Math.Abs(this._b) < GeometryUtility.Tolerance)
            {
                y = (other._c - other._a * x) / other._b;
            }
            else
            {
                y = (this._c - this._a * x) / this._b;
            }

            return new Point2D(x, y);
        }
    }
}