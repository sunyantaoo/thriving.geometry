using System.Runtime.InteropServices;

namespace Thriving.Geometry.Test
{
    public class VectorTest
    {
        [Fact]
        public void TestMethod1()
        {
            Vector2D v1 = Vector2D.BasisX;
            Vector2D v2 = Vector2D.BasisY;
            Vector2D v3 = new Vector2D(1, 1);
            Vector2D v4 = new Vector2D(-1, 1);

            Vector2D v5 = new Vector2D(0.234, 1.111);
            Vector2D v6 = new Vector2D(0.234, 1.111);
            Vector2D v7 = new Vector2D(-0.234, -1.111);

            Assert.True(v5.IsEqualTo(v6));
            Assert.False(v6.IsEqualTo(v7));

            var angle1 = v1.AngleTo(v2);
            Assert.Equal(angle1, Math.PI * 0.5, 0.000001);

            var angle2 = v1.AngleTo(v3);
            Assert.Equal(angle2, Math.PI * 0.25, 0.000001);

            var angle3 = v3.AngleTo(v4);
            Assert.Equal(angle3, Math.PI * 0.5, 0.000001);
        }

        [Fact]
        public void TestMethod2()
        {
            Vector3D v1 = Vector3D.BasisX;
            Vector3D v2 = Vector3D.BasisY;
            Vector3D v3 = new Vector3D(1, 0, 1);
            Vector3D v4 = new Vector3D(-1, 0, -1);

            var angle1 = v1.AngleTo(v2);
            Assert.Equal(angle1, Math.PI * 0.5, 0.000001);

            var angle2 = v1.AngleTo(v3);
            Assert.Equal(angle2, Math.PI * 0.25, 0.000001);

            var angle3 = v3.AngleTo(v4);
            Assert.Equal(angle3, Math.PI, 0.000001);

            Vector3D v5 = new Vector3D(1.2, 0.663, 0.052);
            Vector3D v6 = new Vector3D(1.2, 0.663, 0.052);
            Vector3D v7 = new Vector3D(-1.2, -0.663, -0.052);

            Assert.True(v5.IsAlmostEqualTo(v6));
            Assert.False(v6.IsAlmostEqualTo(v7));
        }

        [Fact]
        public void TestMethod3()
        {
            var ds = Marshal.SizeOf<double>();

            int size1 = Marshal.SizeOf(typeof(Vector3D));
            Assert.Equal(size1, 3 * ds);

            int size2 = Marshal.SizeOf(typeof(Point3D));
            Assert.Equal(size2, 3 * ds);

            int size3 = Marshal.SizeOf(typeof(Vector2D));
            Assert.Equal(size3, 2 * ds);

            int size4 = Marshal.SizeOf(typeof(Point2D));
            Assert.Equal(size4, 2 * ds);
        }
    }
}
