using System.Runtime.InteropServices;

namespace Thriving.Geometry
{
    /// <summary>
    /// 球体
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Sphere
    {
        private readonly Point3D _center;
        private readonly double _radius;

        public Sphere(Point3D center, double radius)
        {
            this._center = center;
            this._radius = radius;
        }
    }
}