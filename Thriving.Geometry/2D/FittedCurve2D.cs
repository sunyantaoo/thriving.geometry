namespace Thriving.Geometry
{
    /// <summary>
    /// 过点的拟合曲线
    /// </summary>
    public class FittedCurve2D : BoundedCurve2D
    {
        private readonly IList<Point2D> _points;

        public FittedCurve2D(IList<Point2D> points)
        {
            _points = points;
        }

        public IList<Point2D> ControlPoints { get => _points.Select(x => _transform.OfPoint(x)).ToList(); }

        public override Point2D StartPoint { get => _transform.OfPoint(_points.ElementAt(0)); }

        public override Point2D EndPoint { get => _transform.OfPoint(_points.ElementAt(1)); }

        public override BoundedCurve2D CreateTransformed(Transform2D transform)
        {
            var sp = new FittedCurve2D(_points)
            {
                _transform = transform.Multiply(this._transform)
            };
            return sp;
        }

        public override BoundedCurve2D CreateReversed()
        {
            var sp = new FittedCurve2D(_points.Reverse().ToList())
            {
                _transform = _transform
            };
            return sp;
        }

        public override BoundedCurve2D CreateOffset(double offsetDist)
        {
            throw new System.NotImplementedException();
        }
    }
}