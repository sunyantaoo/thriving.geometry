namespace Thriving.Geometry
{
    public class Arc3D : BoundedCurve3D
    {
        private readonly double _radius;
        private readonly double _startAngle;
        private readonly double _endAngle;

        public Point3D Center { get => _transform.Origin; }
        public double Radius { get => _radius; }
        public double StartAngle { get => _startAngle; }
        public double EndAngle { get => _endAngle; }

        public double Length { get => _radius * (_endAngle - _startAngle); }

        public Vector3D Normal
        {
            get
            {
                var v1 = new Vector3D(Center, GetPoint(0));
                var v2 = new Vector3D(Center, GetPoint(1));

                return v1.CrossProduct(v2).Normalize();
            }
        }

        public override Point3D StartPoint { get => GetPoint(0); }

        public override Point3D EndPoint { get => GetPoint(1); }

        public Arc3D(Point3D center, double radius, double startAngle, double endAngle, Vector3D basisX, Vector3D basisY)
        {
            this._radius = radius;
            this._startAngle = startAngle;
            this._endAngle = endAngle;

            var basisZ = basisX.CrossProduct(basisY).Normalize();
            this._transform = new Transform3D(center, basisX, basisY, basisZ);
        }

        public Arc3D(Point3D startPoint, Point3D endPoint, Point3D midPoint)
        {
            var triangle = new Triangle3D(startPoint, endPoint, midPoint);
            var center = triangle.CircumCenter();

            this._radius = center.DistanceTo(startPoint);

            var basisX = new Vector3D(center, startPoint).Normalize();
            var basisZ = new Vector3D(midPoint, endPoint).CrossProduct(new Vector3D(midPoint, startPoint).Normalize()); // 圆弧上任一点与起点及终点的夹角始终小于180度
            var basisY = basisZ.CrossProduct(basisX).Normalize();

            this._startAngle = 0;
            var angle = new Vector3D(center, startPoint).AngleTo(new Vector3D(center, endPoint));
            if (new Vector3D(center, startPoint).CrossProduct(new Vector3D(center, endPoint).Normalize()).IsAlmostEqualTo(basisZ))
            {
                this._endAngle = angle;
            }
            else
            {
                this._endAngle = 2 * Math.PI - angle;
            }

            this._transform = new Transform3D(center, basisX, basisY, basisZ);
        }

        public Point3D GetPoint(double ratio)
        {
            var angle = _startAngle + ratio * (_endAngle - _startAngle);
            var result = _transform.Origin.Add(_radius * Math.Cos(angle) * _transform.BasisX + _radius * Math.Sin(angle) * _transform.BasisY);

            return result;
        }

        /// <summary>
        /// 延长圆弧
        /// </summary>
        /// <param name="start">起点延长比例</param>
        /// <param name="end">终点延长比例</param>
        /// <returns></returns>
        public Arc3D Extend(double start, double end)
        {
            var startAngle = _startAngle - start * (_endAngle - _startAngle);
            var endAngle = _endAngle + (end * (_endAngle - _startAngle));

            if (endAngle - startAngle > 2 * Math.PI)
            {
                throw new ArgumentException("圆弧总角度已超出360°");
            }

            return new Arc3D(_transform.Origin, _radius, startAngle, endAngle, _transform.BasisX, _transform.BasisY);
        }

        public override BoundedCurve3D CreateTransformed(Transform3D transform)
        {
            var resTrans = transform.Multiply(this._transform);
            var arc = new Arc3D(resTrans.Origin, _radius, _startAngle, _endAngle, resTrans.BasisX, resTrans.BasisY);
            return arc;
        }

        //public Arc3D CreateReversed()
        //{
        //    return new Segment3D(_endPoint, _startPoint);
        //}
    }
}