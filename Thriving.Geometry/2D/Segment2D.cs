namespace Thriving.Geometry
{
    /// <summary>
    /// 直线段
    /// </summary>
    public class Segment2D : BoundedCurve2D
    {
        private readonly Point2D _startPoint;
        private readonly Point2D _endPoint;

        public Segment2D(Point2D startPoint, Point2D endPoint)
        {
            this._startPoint = startPoint;
            this._endPoint = endPoint;
        }

        public override Point2D StartPoint { get => _transform.OfPoint(_startPoint); }
        public override Point2D EndPoint { get => _transform.OfPoint(_endPoint); }

        /// <summary>
        /// 长度
        /// </summary>
        public double Length { get => _startPoint.DistanceTo(_endPoint); }

        /// <summary>
        /// 方向
        /// </summary>
        public Vector2D Direction { get => _transform.OfVector(new Vector2D(_startPoint, _endPoint)).Normalize(); }

        /// <summary>
        /// 延长线段
        /// </summary>
        /// <param name="start">起点延长比例</param>
        /// <param name="end">终点延长比例</param>
        /// <returns></returns>
        public Segment2D Extend(double start, double end)
        {
            var startPoint = _startPoint.Subtract(start * Length * Direction);
            var endPoint = _endPoint.Add(end * Length * Direction);

            return new Segment2D(startPoint, endPoint);
        }

        public IntersectResult<Point2D>? Intersect(Line2D line)
        {
            var es = line.Intersect(new Line2D(StartPoint, Direction));
            if (es != null)
            {
                var d1 = es.Value.DistanceTo(StartPoint);
                var d2 = es.Value.DistanceTo(EndPoint);
                if (Math.Abs(d1 + d2) - Length < GeometryUtility.Tolerance)
                {
                    return new IntersectResult<Point2D>()
                    {
                        Point = es.Value,
                        UParameter = (d1) / (d1 + d2)
                    };
                }
            }
            return null;
        }

        /// <summary>
        /// 获取线段上的点
        /// </summary>
        /// <param name="ratio">[0,1]之间</param>
        /// <returns></returns>
        public Point2D GetPoint(double ratio)
        {
            var x = _startPoint.X + ratio * (_endPoint.X - _startPoint.X);
            var y = _startPoint.Y + ratio * (_endPoint.Y - _startPoint.Y);
            return _transform.OfPoint(new Point2D(x, y));
        }

        /// <summary>
        /// 获取直线段上的垂线
        /// </summary>
        /// <param name="ratio">[0,1]之间</param>
        /// <returns></returns>
        public Line2D GetVerticalLine(double ratio = 0.5)
        {
            var origin = GetPoint(ratio);
            var k = (_startPoint.X - _endPoint.X) / (_endPoint.Y - _startPoint.Y);

            return new Line2D(k, origin);
        }

        public override BoundedCurve2D CreateTransformed(Transform2D transform)
        {
            var line = new Segment2D(_startPoint, _endPoint)
            {
                _transform = transform.Multiply(this._transform)
            };
            return line;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="offsetDist">偏移距离</param>
        /// <returns></returns>
        public override BoundedCurve2D CreateOffset(double offsetDist)
        {
            var offsetN = new Vector2D(_startPoint, _endPoint).Vertical().Normalize();
            var startPoint = _startPoint.Add(offsetDist * offsetN);
            var endPoint = _endPoint.Add(offsetDist * offsetN);
            return new Segment2D(startPoint, endPoint);
        }

        public override BoundedCurve2D CreateReversed()
        {
            return new Segment2D(_transform.OfPoint(_endPoint), _transform.OfPoint(_startPoint));
        }
    }
}