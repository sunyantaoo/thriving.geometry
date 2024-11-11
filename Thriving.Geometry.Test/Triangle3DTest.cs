namespace Thriving.Geometry.Test
{
    public class Triangle3DTest
    {
        [Fact]
        public void TestMethod1()
        {
            var v1 = new Point3D(-1, 0, 0);
            var v2 = new Point3D(1, 0, 0);
            var v3 = new Point3D(0, 1, 0);
            var triangle = new Triangle3D(v1, v2, v3);

            Assert.True(triangle.IsCounterClockwise == true);
            Assert.True(triangle.Normal.AngleTo(Vector3D.BasisX) == 0.5 * Math.PI);

            Assert.True(triangle.IsProjectionInner(new Point3D(0.5, 0.2, 0.2)) == true);

            Assert.True(triangle.IsProjectionInner(new Point3D(-1, 0, 5)) == true);

        }

        [Fact]
        public void TestMethod2()
        {
            var v1 = new Point3D(1, 0, 0);
            var v2 = new Point3D(0, 1, 0);
            var v3 = new Point3D(0, 0, 1);
            var triangle = new Triangle3D(v1, v2, v3);

            Assert.True(triangle.IsCounterClockwise == true);
            var normal = triangle.Normal;
            var angle = normal.AngleTo(new Vector3D(1, 1, 1));
            Assert.True(angle == 0);

            Assert.True(triangle.IsProjectionInner(new Point3D(0.5, 0.2, 0.2)) == true);

            Assert.True(triangle.IsProjectionInner(new Point3D(-1, 0, 5)) == false);
        }
    }
}
