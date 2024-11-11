namespace Thriving.Geometry.Test
{
    public class Plane3DTest
    {
        [Fact]
        public void TestMethod1()
        {
            var plane = new Plane3D(2 * Vector3D.BasisZ, 5);

            var d1 = plane.DistanceTo(new Point3D(1, 1, 2.5));
            Assert.Equal(d1, 0);

            var d3 = plane.DistanceTo(new Point3D(1, 1, 15));
            Assert.True(d3 > 0);

            var d4 = plane.DistanceTo(new Point3D(1, 1, 1));
            Assert.True(d4 < 0);

            var iPoint1 = plane.Intersect(new Point3D(1, 0, 3), Vector3D.BasisX);
            Assert.Null(iPoint1);

            var iPoint2 = plane.Intersect(new Point3D(1, 0, 0), 2 * Vector3D.BasisZ);
            var d2 = iPoint2.Value.DistanceTo(new Point3D(1, 0, 2.5));
            Assert.Equal(d2, 0);

            var iPoint3 = plane.Intersect(new Point3D(1, 0, 10), Vector3D.BasisZ);
            var d5 = iPoint3.Value.DistanceTo(new Point3D(1, 0, 2.5));
            Assert.Equal(d5, 0);
        }


        [Fact]
        public void TestMethod2()
        {
            var plane = new Plane3D(new Vector3D(1,0,1),  new Point3D(1,0,0));

            var d1 = plane.DistanceTo(Point3D.Zero);
            Assert.Equal(d1, -1/Math.Sqrt(2));

            var d3 = plane.DistanceTo(new Point3D(1, 0, 1));
            Assert.True(  d3 == 1 / Math.Sqrt(2));

            var d4 = plane.DistanceTo(new Point3D(-1, 0, -1));
            Assert.True(Math.Abs(d4+3* Math.Sqrt(2)/2)<0.0001);

            var iPoint1 = plane.Intersect(new Point3D(1, 0, 1), new Vector3D(1, 0, -1));
            Assert.Null(iPoint1);

            var iPoint2 = plane.Intersect(new Point3D(1, 0, 1), Vector3D.BasisZ);
            Assert.True(iPoint2.Value.IsAlmostEqualTo(new Point3D(1,0,0)));

            var iPoint3 = plane.Intersect(new Point3D(-1, 0, -1), Vector3D.BasisZ);
            Assert.True(iPoint3.Value.IsAlmostEqualTo(new Point3D(-1, 0, 2)));
        }

    }
}
