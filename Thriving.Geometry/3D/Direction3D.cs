namespace Thriving.Geometry
{
    /// <summary>
    /// 三维方向，即单位向量
    /// </summary>
    public readonly struct Direction3D 
    {
        private readonly double _x, _y, _z;

        public Direction3D(Vector3D vector)
        {
            var nor = vector.Normalize();

            this._x = nor.X;
            this._y = nor.Y;
            this._z = nor.Z;
        }

        public Direction3D(double x, double y, double z) : this(new Vector3D(x, y, z))
        {

        }

        public Vector3D Direction
        {
            get { return new Vector3D(_x, _y, _z); }
        }
    }
}