namespace Thriving.Geometry
{
    /// <summary>
    /// 四面体
    /// </summary>
    public class Tetrahedron
    {
        private readonly Point3D _v0, _v1, _v2, _v3;

        public Tetrahedron(Point3D v0, Point3D v1, Point3D v2, Point3D v3)
        {
            this._v0 = v0;
            this._v1 = v1;
            this._v2 = v2;
            this._v3 = v3;
        }

        /// <summary>
        /// 四面体上指定索引的三角面[0,3]，法线朝外
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Triangle3D? this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return new Triangle3D(_v2, _v1, _v0);
                    case 1: return new Triangle3D(_v0, _v1, _v3);
                    case 2: return new Triangle3D(_v1, _v2, _v3);
                    case 3: return new Triangle3D(_v2, _v0, _v3);
                    default: return null;
                }
            }
        }
    }
}