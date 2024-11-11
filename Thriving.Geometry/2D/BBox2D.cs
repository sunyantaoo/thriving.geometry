using System.Runtime.InteropServices;

namespace Thriving.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BBox2D
    {
        private Point2D _min;
        private Point2D _max;

        public Point2D Min
        {
            readonly get { return _min; }
            set { _min = value; }
        }

        public Point2D Max
        {
            readonly get { return _max; }
            set { _max = value; }
        }
    }
}