namespace Thriving.Geometry
{
    public class Ray3D 
    {
        private readonly Point3D _origin;
        private readonly Vector3D _direction;

        public Point3D Origin
        {
            get { return _origin; }
        }

        public Vector3D Direction
        {
            get { return _direction; }
        }

        public Ray3D(Point3D origin, Vector3D dir)
        {
            this._origin = origin;
            this._direction = dir;
        }
    }
}