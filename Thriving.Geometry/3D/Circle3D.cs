namespace Thriving.Geometry
{
    public struct Circle3D 
    {
        private  Point3D _center;
        private  double _radius;
        private  Vector3D _normal;

        public Circle3D()
        {
            
        }

        public Circle3D(Point3D center, double radius, Vector3D normal)
        {
            this._center = center;
            this._radius = radius;
            this._normal = normal;
        }

        public Point3D Center
        {
            readonly get => _center;
            set { _center = value; }
        }
        public Vector3D Normal
        {
            readonly get => _normal;
            set { _normal = value; }
        }
        public double Radius
        {
            readonly get => _radius;
            set { _radius = value; }
        }


        public readonly Plane3D Plane { get => new Plane3D(_normal, _center); }
       

        /// <summary>
        /// 面积
        /// </summary>
        public readonly double Area { get => Math.PI * Math.Pow(_radius, 2); }

        /// <summary>
        /// 空间圆与直线相交，
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public readonly IList<IntersectResult<Point3D>>? Intersect(Line3D line)
        {
            if (Plane.IsCoplanar(line))
            {
                //1.共面时，求圆心在直线上的投影，后判断投影点与圆心的交点
                var pRes = line.Projection(_center);
                if (pRes.Distance == _radius)
                {
                    var iRes = new IntersectResult<Point3D>() { Point = pRes.Point };
                    return new List<IntersectResult<Point3D>> { iRes };
                }
                if (pRes.Distance < _radius)
                {
                    var half = Math.Sqrt(_radius * _radius - pRes.Distance * pRes.Distance);
                    var iRes1 = new IntersectResult<Point3D>()
                    {
                        Point = pRes.Point.Subtract(half * line.Direction),
                    };
                    var iRes2 = new IntersectResult<Point3D>()
                    {
                        Point = pRes.Point.Add(half * line.Direction),
                    };
                    return new List<IntersectResult<Point3D>> { iRes1, iRes2 };
                }
            }
            else
            {
                //2.非共面时，求直线与平面的交点，交点与圆心的距离相等则相交
                var iPoint = Plane.Intersect(line.Origin, line.Direction);
                if (iPoint != null)
                {
                    if (_center.DistanceTo(iPoint.Value) == _radius)
                    {
                        var iRes = new IntersectResult<Point3D>() { Point = iPoint.Value };
                        return new List<IntersectResult<Point3D>> { iRes };
                    }
                }
            }
            return null;
        }
    }
}