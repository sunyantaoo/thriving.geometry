namespace Thriving.Geometry
{
    /// <summary>
    /// 直线段
    /// </summary>
    public class Segment3D : BoundedCurve3D
    {
        private readonly Point3D _startPoint;
        private readonly Point3D _endPoint;

        public override Point3D StartPoint { get => _startPoint; }
        public override Point3D EndPoint { get => _endPoint; }

        public Segment3D(Point3D startPoint, Point3D endPoint)
        {
            this._startPoint = startPoint;
            this._endPoint = endPoint;
        }

        /// <summary>
        /// 长度
        /// </summary>
        public double Length { get => _startPoint.DistanceTo(_endPoint); }

        /// <summary>
        /// 方向
        /// </summary>
        public Vector3D Direction { get => new Vector3D(_startPoint, _endPoint).Normalize(); }

        /// <summary>
        /// 获取线段上的点
        /// </summary>
        /// <param name="ratio">[0,1]之间</param>
        /// <returns></returns>
        public Point3D GetPoint(double ratio)
        {
            var x = _startPoint.X + ratio * (_endPoint.X - _startPoint.X);
            var y = _startPoint.Y + ratio * (_endPoint.Y - _startPoint.Y);
            var z = _startPoint.Z + ratio * (_endPoint.Z - _startPoint.Z);
            return new Point3D(x, y, z);
        }

        /// <summary>
        /// 延长线段
        /// </summary>
        /// <param name="start">起点延长比例</param>
        /// <param name="end">终点延长比例</param>
        /// <returns></returns>
        public Segment3D Extend(double start, double end)
        {
            var startPoint = _startPoint.Subtract(start * Length * Direction);
            var endPoint = _endPoint.Add(end * Length * Direction);

            return new Segment3D(startPoint, endPoint);
        }

        public override BoundedCurve3D CreateTransformed(Transform3D transform)
        {
            var segment = new Segment3D(transform.OfPoint(_startPoint), transform.OfPoint(_endPoint));
            return segment;
        }

        public Segment3D CreateReversed()
        {
            return new Segment3D(_endPoint, _startPoint);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="offsetDist">偏移距离</param>
        /// <returns></returns>
        public Segment3D CreateOffset(double offsetDist, Vector3D normal)
        {
            var transfrom = Transform3D.CreateTranslation(offsetDist * Direction.CrossProduct(normal).Normalize());
            return new Segment3D(transfrom.OfPoint(_startPoint), transfrom.OfPoint(_endPoint));
        }
    }
}