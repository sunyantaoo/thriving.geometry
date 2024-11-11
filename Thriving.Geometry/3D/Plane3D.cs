using System.Runtime.InteropServices;

namespace Thriving.Geometry
{
    /// <summary>
    /// 平面
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Plane3D
    {
        private readonly double _a, _b, _c, _d;

        /// <summary>
        /// 满足方程是Ax+By+Cz=D
        /// </summary>
        /// <param name="normal">平面法向量(A,B,C)</param>
        /// <param name="distance">如果法向量为单位向量，则D为原点至平面的距离</param>
        public Plane3D(Vector3D normal, double distance)
        {
            this._a = normal.X;
            this._b = normal.Y;
            this._c = normal.Z;

            this._d = distance;
        }

        /// <summary>
        /// 满足方程式A(x-U)+B(y-V)+C(z-W)=0
        /// </summary>
        /// <param name="normal">平面法向量(A,B,C)</param>
        /// <param name="arbiPoint">平面上任意一点(U,V,W)</param>
        public Plane3D(Vector3D normal, Point3D arbiPoint)
        {
            this._a = normal.X;
            this._b = normal.Y;
            this._c = normal.Z;

            this._d = normal.X * arbiPoint.X + normal.Y * arbiPoint.Y + normal.Z * arbiPoint.Z;
        }

        public Vector3D Normal { get => new Vector3D(_a, _b, _c).Normalize(); }

        public static Plane3D PlaneXY { get => new Plane3D(Vector3D.BasisZ, Point3D.Zero); }
        public static Plane3D PlaneYZ { get => new Plane3D(Vector3D.BasisX, Point3D.Zero); }
        public static Plane3D PlaneZX { get => new Plane3D(Vector3D.BasisY, Point3D.Zero); }

        /// <summary>
        ///
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsCoplanar(Point3D point)
        {
            return _a * point.X + _b * point.Y + _c * point.Z == _d;
        }

        public bool IsCoplanar(Line3D line)
        {
            var result = false;
            if (line.Direction.DotProduct(Normal) == 0)
            {
                result = IsCoplanar(line.Origin);
            }
            return result;
        }

        /// <summary>
        /// 到面的距离点
        /// </summary>
        /// <param name="point"></param>
        /// <returns>正值时点在平面正向，负值则为反向</returns>
        public double DistanceTo(Point3D point)
        {
            return (_a * point.X + _b * point.Y + _c * point.Z - _d) / Math.Sqrt(_a * _a + _b * _b + _c * _c);
        }

        /// <summary>
        /// 点在面上的投影
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public ProjectionResult<Point3D> Projection(Point3D point)
        {
            var distance = DistanceTo(point);
            var p_point = point.Subtract(distance * Normal);
            return new ProjectionResult<Point3D>()
            {
                Distance = distance,
                Point = p_point,
            };
        }

        /// <summary>
        /// 向量与平面的夹角
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public double AngleTo(Vector3D vector)
        {
            // 向量与平面法向量的角度
            return 0.5 * Math.PI - vector.AngleTo(Normal);
        }

        /// <summary>
        /// 过点的向量与平面的交点
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>向量与平面平行时返回空</returns>
        public Point3D? Intersect(Point3D point, Vector3D vector)
        {
            var d = DistanceTo(point);
            if (d == 0)
            {
                return point;
            }
            var cos = vector.DotProduct(Normal) / vector.Length;
            if (cos == 0) { return null; }
            if (cos >= 1)
            {
                return point.Subtract(d * Normal);
            }
            else
            {
                return point.Subtract((d / cos) * vector.Normalize());
            }
        }

        /// <summary>
        /// 平面是否与线段相交
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        public bool IsIntersect(Segment3D segment, out Point3D? intersectPoint)
        {
            var startPoint = segment.GetPoint(0);
            var endPoint = segment.GetPoint(1);
            var vector = new Vector3D(startPoint, endPoint);
            var iPoint = Intersect(startPoint, vector);

            if (iPoint == null)
            {
                intersectPoint = null;
                return false;
            }

            // 判断交点是否在线段范围内
            var lengthA = iPoint.Value.DistanceTo(startPoint);
            var lengthB = iPoint.Value.DistanceTo(endPoint);

            if (lengthA > segment.Length || lengthB > segment.Length)
            {
                intersectPoint = null;
                return false;
            }
            else
            {
                intersectPoint = iPoint;
                return true;
            }
        }
    }
}