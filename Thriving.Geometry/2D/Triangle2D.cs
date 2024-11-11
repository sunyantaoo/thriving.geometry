using System.Runtime.InteropServices;

namespace Thriving.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Triangle2D
    {
        private readonly Point2D _v1;
        private readonly Point2D _v2;
        private readonly Point2D _v3;

        public Triangle2D(Point2D v1, Point2D v2, Point2D v3)
        {
            _v1 = v1;
            _v2 = v2;
            _v3 = v3;
        }

        /// <summary>
        /// 逆时针方向
        /// </summary>
        public bool IsCounterClockwise
        {
            get
            {
                var n1 = new Vector2D(_v1, _v2);
                var n2 = new Vector2D(_v1, _v3);

                return n1.X * n2.Y > n1.Y * n2.X;
            }
        }

        /// <summary>
        /// 外接圆圆心
        /// </summary>
        /// <returns></returns>
        public Point2D CircumCenter()
        {
            var a = _v1.X - _v2.X;
            var b = _v1.Y - _v2.Y;
            var c = _v1.X - _v3.X;
            var d = _v1.Y - _v3.Y;
            var e = 0.5 * ((_v1.X * _v1.X - _v2.X * _v2.X) - (_v2.Y * _v2.Y - _v1.Y * _v1.Y));
            var f = 0.5 * ((_v1.X * _v1.X - _v3.X * _v3.X) - (_v3.Y * _v3.Y - _v1.Y * _v1.Y));

            var x = (e * d - b * f) / (a * d - b * c);
            var y = (a * f - e * c) / (a * d - b * c);

            return new Point2D(x, y);
        }

        /// <summary>
        /// 点是否在三角形内
        /// </summary>
        /// <param name="point"></param>
        /// <param name="edge"></param>
        /// <returns></returns>
        public bool IsInner(Point2D point, bool edge = true)
        {
            Vector2D op = new Vector2D(point.X - _v1.X, point.Y - _v1.Y);
            Vector2D oa = new Vector2D(_v2.X - _v1.X, _v2.Y - _v1.Y);
            Vector2D ob = new Vector2D(_v3.X - _v1.X, _v3.Y - _v1.Y);
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