using System.Runtime.InteropServices;

namespace Thriving.Geometry
{
    /// <summary>
    /// 三角形
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Triangle3D 
    {
        private readonly Point3D _v1;
        private readonly Point3D _v2;
        private readonly Point3D _v3;

        public Triangle3D(Point3D v1, Point3D v2, Point3D v3)
        {
            _v1 = v1;
            _v2 = v2;
            _v3 = v3;
        }

        /// <summary>
        /// 逆时针方向（参照Z轴正方向）
        /// </summary>
        public bool IsCounterClockwise
        {
            get
            {
                var n1 = new Vector3D(_v1, _v2);
                var n2 = new Vector3D(_v1, _v3);
                return n1.X * n2.Y > n1.Y * n2.X;
            }
        }

        public Point3D Vertex1 { get => _v1; }
        public Point3D Vertex2 { get => _v2; }
        public Point3D Vertex3 { get => _v3; }

        /// <summary>
        /// 法向量
        /// </summary>
        /// <returns></returns>
        public Vector3D Normal
        {
            get
            {
                var n1 = new Vector3D(_v1, _v2);
                var n2 = new Vector3D(_v1, _v3);

                return n1.CrossProduct(n2).Normalize();
            }
        }

        /// <summary>
        /// 三角形所在平面
        /// </summary>
        /// <returns></returns>
        public Plane3D Plane()
        {
            return new Plane3D(Normal, _v1);
        }
        /// <summary>
        /// 外接圆圆心
        /// </summary>
        /// <returns></returns>
        public Point3D CircumCenter()
        {
            double x1 = _v1.X;
            double x2 = _v2.X;
            double x3 = _v3.X;
            double y1 = _v1.Y;
            double y2 = _v2.Y;
            double y3 = _v3.Y;
            double z1 = _v1.Z;
            double z2 = _v2.Z;
            double z3 = _v3.Z;

            double a1 = (y1 * z2 - y2 * z1 - y1 * z3 + y3 * z1 + y2 * z3 - y3 * z2),
                   b1 = -(x1 * z2 - x2 * z1 - x1 * z3 + x3 * z1 + x2 * z3 - x3 * z2),
                   c1 = (x1 * y2 - x2 * y1 - x1 * y3 + x3 * y1 + x2 * y3 - x3 * y2),
                   d1 = -(x1 * y2 * z3 - x1 * y3 * z2 - x2 * y1 * z3 + x2 * y3 * z1 + x3 * y1 * z2 - x3 * y2 * z1);

            double a2 = 2 * (x2 - x1),
                   b2 = 2 * (y2 - y1),
                   c2 = 2 * (z2 - z1),
                   d2 = x1 * x1 + y1 * y1 + z1 * z1 - x2 * x2 - y2 * y2 - z2 * z2;

            double a3 = 2 * (x3 - x1),
                   b3 = 2 * (y3 - y1),
                   c3 = 2 * (z3 - z1),
                   d3 = x1 * x1 + y1 * y1 + z1 * z1 - x3 * x3 - y3 * y3 - z3 * z3;

            double cx = -(b1 * c2 * d3 - b1 * c3 * d2 - b2 * c1 * d3 + b2 * c3 * d1 + b3 * c1 * d2 - b3 * c2 * d1)
                    / (a1 * b2 * c3 - a1 * b3 * c2 - a2 * b1 * c3 + a2 * b3 * c1 + a3 * b1 * c2 - a3 * b2 * c1);
            double cy = (a1 * c2 * d3 - a1 * c3 * d2 - a2 * c1 * d3 + a2 * c3 * d1 + a3 * c1 * d2 - a3 * c2 * d1)
                    / (a1 * b2 * c3 - a1 * b3 * c2 - a2 * b1 * c3 + a2 * b3 * c1 + a3 * b1 * c2 - a3 * b2 * c1);
            double cz = -(a1 * b2 * d3 - a1 * b3 * d2 - a2 * b1 * d3 + a2 * b3 * d1 + a3 * b1 * d2 - a3 * b2 * d1)
                    / (a1 * b2 * c3 - a1 * b3 * c2 - a2 * b1 * c3 + a2 * b3 * c1 + a3 * b1 * c2 - a3 * b2 * c1);

            return new Point3D(cx, cy, cz);
        }

        /// <summary>
        /// 是否与直线段相交且在三角形范围内
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="intersectPoint"></param>
        /// <returns></returns>
        public bool Intersect(Segment3D segment, bool edge, out Point3D? intersectPoint)
        {
            // 求：三角形平面与线段所在向量的交点
            var plane = new Plane3D(Normal, _v1);
            if (plane.IsIntersect(segment, out Point3D? iPoint))
            {
                if (IsProjectionInner(iPoint.Value, edge))
                {
                    intersectPoint = iPoint;
                    return true;
                }
                else
                {
                    intersectPoint = null;
                    return false;
                }
            }
            else
            {
                intersectPoint = null;
                return false;
            }
        }

        /// <summary>
        /// 点是否在三角形内
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsInner(Point3D point)
        {
            if (IsCoplanar(point))
            {
                var va = new Vector3D(point, _v1);
                var vb = new Vector3D(point, _v2);
                var vc = new Vector3D(point, _v3);

                var a1 = va.AngleTo(vb);
                var a2 = vb.AngleTo(vc);
                var a3 = vc.AngleTo(va);

                var sum = a1 + a2 + a3;
                // 内角和为360度则在三角形内
                return Math.Abs(sum - 2 * Math.PI) < GeometryUtility.Tolerance;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否共面
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsCoplanar(Point3D point)
        {
            var va = new Vector3D(point, _v1);
            var vb = new Vector3D(point, _v2);
            var vc = new Vector3D(point, _v3);

            var mat = new Matrix(new double[,]
            {
                { va.X,vb.X,vc.X },
                { va.Y,vb.Y,vc.Y },
                { va.Z,vb.Z,vc.Z }
            });
            // 行列式值为0，则共面
            return Math.Abs(mat.DetValue) < GeometryUtility.Tolerance;
        }

        /// <summary>
        /// 点是否在三角形平面投影范围内
        /// </summary>
        /// <param name="point"></param>
        /// <param name="edge"></param>
        /// <returns></returns>
        public bool IsProjectionInner(Point3D point, bool edge = true)
        {
            var op = new Vector3D(point.X - _v1.X, point.Y - _v1.Y, 0);
            var oa = new Vector3D(_v2.X - _v1.X, _v2.Y - _v1.Y, 0);
            var ob = new Vector3D(_v3.X - _v1.X, _v3.Y - _v1.Y, 0);
            double u = (op.DotProduct(oa) * ob.DotProduct(ob) - op.DotProduct(ob) * ob.DotProduct(oa)) / (oa.DotProduct(oa) * ob.DotProduct(ob) - ob.DotProduct(oa) * oa.DotProduct(ob));
            double v = (op.DotProduct(ob) * oa.DotProduct(oa) - op.DotProduct(oa) * oa.DotProduct(ob)) / (ob.DotProduct(ob) * oa.DotProduct(oa) - oa.DotProduct(ob) * ob.DotProduct(oa));
            if (u >= 0 && v >= 0 && u + v <= 1)//u+v=1时，在三角形边上
            {
                if (u == 0 || v == 0 || u + v == 1)
                {
                    return edge;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }
}