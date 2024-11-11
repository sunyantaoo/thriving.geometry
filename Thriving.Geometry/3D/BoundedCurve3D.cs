namespace Thriving.Geometry
{
    /// <summary>
    /// 有边界曲线
    /// </summary>
    public abstract class BoundedCurve3D
    {
        public abstract Point3D StartPoint { get; }

        public abstract Point3D EndPoint { get; }

        protected Transform3D _transform = Transform3D.Identity;

        public Transform3D Transform { get => _transform; }

        public abstract BoundedCurve3D CreateTransformed(Transform3D transform);
    }
}