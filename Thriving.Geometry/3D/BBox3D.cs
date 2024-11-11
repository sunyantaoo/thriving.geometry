using System.Runtime.InteropServices;

namespace Thriving.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BBox3D
    {
        private  Point3D _min;
        private  Point3D _max;

        public  Point3D Min
        {
            readonly get { return _min; }
            set { _min = value; }
        }

        public Point3D Max
        {
            readonly get { return _max; }
            set { _max = value; }
        }

        public BBox3D()
        {
            
        }

        public BBox3D(Point3D min, Point3D max)
        {
            this._min = min;
            this._max = max;
        }
    }
}