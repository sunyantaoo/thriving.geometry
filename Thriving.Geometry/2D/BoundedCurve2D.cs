namespace Thriving.Geometry
{
    /// <summary>
    /// 有边界曲线
    /// </summary>
    public abstract class BoundedCurve2D
    {
        protected Transform2D _transform = Transform2D.Identity;

        public Transform2D Transform { get => _transform; }

        public abstract Point2D StartPoint { get; }

        public abstract Point2D EndPoint { get; }

        public abstract BoundedCurve2D CreateTransformed(Transform2D transform);

        public abstract BoundedCurve2D CreateOffset(double offsetDist);

        public abstract BoundedCurve2D CreateReversed();
    }
}