namespace Thriving.Geometry
{
    public class Line3D 
    {
        private readonly Point3D _origin;
        private readonly Vector3D _direction;

        public Line3D(Point3D origin, Vector3D direction)
        {
            _origin = origin;
            _direction = direction.Normalize();
        }

        public Point3D Origin { get => _origin; }

        public Vector3D Direction { get => _direction; }

        /// <summary>
        /// 是否在线上
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsInner(Point3D point)
        {
            var v1 = new Vector3D(_origin, point);
            var c = v1.CrossProduct(_direction);

            return c.Length < GeometryUtility.Tolerance;
        }

        /// <summary>
        /// 点在线上的投影点
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public ProjectionResult<Point3D> Projection(Point3D point)
        {
            var vector = new Vector3D(_origin, point);
            var angle = _direction.AngleTo(vector);

            var d = vector.Length * Math.Sin(angle);
            var p_point = point.Subtract(d * _direction);
            return new ProjectionResult<Point3D>()
            {
                Point = p_point,
                Distance = d,
            };
        }
    }
}