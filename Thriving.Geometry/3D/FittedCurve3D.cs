namespace Thriving.Geometry
{
    public class FittedCurve3D : BoundedCurve3D
    {
        private readonly IList<Point3D> _points;

        public FittedCurve3D(IList<Point3D> points)
        {
            _points = points;
        }

        public IList<Point3D> ControlPoints { get => _points.Select(x => _transform.OfPoint(x)).ToList(); }

        public override Point3D StartPoint { get => _points.FirstOrDefault(); }

        public override Point3D EndPoint { get => _points.LastOrDefault(); }

        public override BoundedCurve3D CreateTransformed(Transform3D transform)
        {
            var points = _points.Select(x => transform.OfPoint(x)).ToList();
            return new FittedCurve3D(points);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BezierCurve
    {
        public BezierCurve(IList<Point3D> points)
        {
        }
    }
}