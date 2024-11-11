using System.Runtime.InteropServices;

namespace Thriving.Geometry.Test
{
    public class SizeTest
    {
        [Fact]
        public void TestMethod1()
        {
            Assert.Equal(Marshal.SizeOf<byte>(), 1); // 8位
            Assert.Equal(Marshal.SizeOf<bool>(), 4); // 32位
            Assert.Equal(Marshal.SizeOf<short>(), 2); // 16位
            Assert.Equal(Marshal.SizeOf<int>(), 4); // 32位
            Assert.Equal(Marshal.SizeOf<long>(), 8); // 64位
            Assert.Equal(Marshal.SizeOf<float>(), 4); // 32位
            Assert.Equal(Marshal.SizeOf<double>(), 8); // 64位

            Assert.Equal(Marshal.SizeOf<IntPtr>(), 8);

            // 占用内存
            Assert.Equal(Marshal.SizeOf<Point2D>(), 16); // double 8*2=16
            Assert.Equal(Marshal.SizeOf<Point3D>(), 24); // double 8*3=24

            Assert.Equal(Marshal.SizeOf<Vector2D>(), 16); // double 8*2=16
            Assert.Equal(Marshal.SizeOf<Vector3D>(), 24); // double 8*3=24

            Assert.Equal(Marshal.SizeOf<Circle2D>(), 24); // Point2D double 16+8=24
            Assert.Equal(Marshal.SizeOf<Circle3D>(), 56); // Point3D Vector3D double 24+24+8=56

            Assert.Equal(Marshal.SizeOf<BBox2D>(), 32); // Point2D Point2D 16+16=32
            Assert.Equal(Marshal.SizeOf<BBox3D>(), 48); // Point3D Point3D 24+24=48

            Assert.Equal(Marshal.SizeOf<Line2D>(), 24); // double 8*3=24
            Assert.Equal(Marshal.SizeOf<Plane3D>(), 32); // double 8*4=32

            Assert.Equal(Marshal.SizeOf<Triangle2D>(), 48); // Point2D 16*3=48
            Assert.Equal(Marshal.SizeOf<Triangle3D>(), 72); // Point3D 24*3=72

            Assert.Equal(Marshal.SizeOf<Transform2D>(), 72);  // double 8*9=72
            Assert.Equal(Marshal.SizeOf<Transform3D>(), 128);  // double 8*16=128
        }

        [Fact]
        public void TestMethod2()
        {
            Assert.Equal(Marshal.SizeOf<TimeSpan>(), 8);
            Assert.Equal(Marshal.SizeOf<DateTime>(), 8);

            Assert.Equal(Marshal.SizeOf<Vec>(), 32);
        }

        public readonly struct Vec
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            private readonly double[] data;
        }
    }
}
