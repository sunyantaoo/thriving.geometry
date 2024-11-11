using System.Drawing;

namespace Thriving.Geometry
{
    public struct Circle2D
    {
        private Point2D _center;
        private double _radius;

        public Circle2D(Point2D center, double radius)
        {
            this._center = center;
            this._radius = radius;
        }
        public Point2D Center
        {
            readonly get => _center;
            set { _center = value; }
        }
        public double Radius
        {
            readonly get => _radius;
            set { _radius = value; }
        }


        /// <summary>
        /// 面积
        /// </summary>
        public readonly double Area { get => Math.PI * Math.Pow(_radius, 2); }

        /// <summary>
        /// 周长
        /// </summary>
        public readonly double Perimeter { get => 2 * Math.PI * _radius; }

        /// <summary>
        /// 圆与直线相交，不相交为空
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public readonly IList<IntersectResult<Point2D>>? Intersect(Line2D line)
        {
            // 圆心到直线的距离判单是否相交
            var pRes = line.Projection(_center);
            if (Math.Abs(_radius - Math.Abs(pRes.Distance)) <= GeometryUtility.Tolerance)
            {
                var iRes = new IntersectResult<Point2D>()
                {
                    Point = pRes.Point,
                };
                return new List<IntersectResult<Point2D>>() { iRes };
            }
            else if (_radius - Math.Abs(pRes.Distance) > GeometryUtility.Tolerance)
            {
                var half = Math.Sqrt(_radius * _radius - pRes.Distance * pRes.Distance);
                var iRes1 = new IntersectResult<Point2D>()
                {
                    Point = pRes.Point.Subtract(half * line.Direction),
                };
                var iRes2 = new IntersectResult<Point2D>()
                {
                    Point = pRes.Point.Add(half * line.Direction),
                };
                return new List<IntersectResult<Point2D>> { iRes1, iRes2 };
            }
            return null;
        }

        /// <summary>
        /// 圆与圆相交，不相交为空
        /// </summary>
        /// <param name="circle"></param>
        /// <returns></returns>
        public readonly IList<IntersectResult<Point2D>>? Intersect(Circle2D other)
        {
            var distance = this.Center.DistanceTo(other.Center);
            if (distance == this.Radius + other.Radius)
            {
                var dir = new Vector2D(this.Center, other.Center).Normalize();
                var point = this.Center.Add(dir * this.Radius);
                return new List<IntersectResult<Point2D>>()
                {
                    new IntersectResult<Point2D>()
                    {
                        Point= point,
                    }
                };
            }
            else if (distance < this.Radius + other.Radius && distance > Math.Abs(this.Radius - other.Radius))
            {
                var angle = Math.Acos((this.Radius * this.Radius + distance * distance - other.Radius * other.Radius) / (2 * this.Radius * distance));

                var tranA = Transform2D.CreateRotation(angle);
                var tranB = Transform2D.CreateRotation(-angle);

                var dir = new Vector2D(this.Center, other.Center).Normalize();

                var dirA = tranA.OfVector(dir);
                var dirB = tranB.OfVector(dir);

                var pointA = this.Center.Add(dirA * this.Radius);
                var pointB = this.Center.Add(dirB * this.Radius);

                return new List<IntersectResult<Point2D>>()
                {
                    new IntersectResult<Point2D>()
                    {
                        Point= pointA,
                    },
                     new IntersectResult<Point2D>()
                    {
                        Point= pointB,
                    }
                };
            }
            return null;
        }

    }
}