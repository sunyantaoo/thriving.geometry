namespace Thriving.Geometry.Test
{
    public class TransfromTest
    {
        [Fact]
        public void TestMethod1()
        {
            Transform2D t1 = Transform2D.CreateTranslation(new Vector2D(0.5, 0));
            var res1 = t1.OfPoint(new Point2D(0.5, 0.5));
            Assert.Equal(res1.X, 1);
            Assert.Equal(res1.Y, 0.5);

            Transform2D transform = Transform2D.CreateRotation(0.5 * Math.PI);
            var res = transform.OfVector(new Vector2D(1, 1));

            Assert.True(res.AngleTo(Vector2D.BasisY) == 0.25 * Math.PI);
        }

        [Fact]
        public void TestMethod2()
        {
            Transform3D transform = Transform3D.CreateRotation(Vector3D.BasisZ, 0.5 * Math.PI);

            var res = transform.OfVector(new Vector3D(1, 1, 0));

            Assert.Equal(res.AngleTo(Vector3D.BasisY), 0.25 * Math.PI);
        }

        [Fact]
        public void TestMethod3()
        {
            Transform2D transform = Transform2D.CreateScale(0.1, 0.5);
            var res = transform.OfPoint(new Point2D(1, 1));

            Assert.Equal(res.X, 0.1);
            Assert.Equal(res.Y, 0.5);

            Transform2D transform2 = Transform2D.CreateScaleAtPoint(0.1, 0.5, new Point2D(1, 1));
            var res2 = transform2.OfPoint(new Point2D(2, 2));

            Assert.Equal(res2.X, 1.1);
            Assert.Equal(res2.Y, 1.5);
        }

        [Fact]
        public void TestMethod4()
        {
            Transform3D transform = Transform3D.CreateScale(0.1, 0.5, 0.2);
            var res = transform.OfPoint(new Point3D(1, 1, 1));

            Assert.Equal(res.X, 0.1);
            Assert.Equal(res.Y, 0.5);
            Assert.Equal(res.Z, 0.2);

            Transform3D transform2 = Transform3D.CreateScaleAtPoint(0.1, 0.5, 0.2, new Point3D(1, 1, 1));
            var res2 = transform2.OfPoint(new Point3D(1, 1, 1));

            Assert.Equal(res2.X, 1);
            Assert.Equal(res2.Y, 1);
            Assert.Equal(res2.Z, 1);

            Transform3D transform3 = Transform3D.CreateScaleAtPoint(0.1, 0.5, 0.2, new Point3D(1, 1, 1));
            var res3 = transform3.OfPoint(new Point3D(2, 2, 2));

            Assert.Equal(res3.X, 1.1);
            Assert.Equal(res3.Y, 1.5);
            Assert.Equal(res3.Z, 1.2, 0.001);
        }


        [Fact]
        public void TestMethod5()
        {
            Transform2D transform = Transform2D.CreateReflection(new Vector2D(0, 1));
            var res = transform.OfPoint(new Point2D(1, 1));

            Assert.Equal(res.X, -1);
            Assert.Equal(res.Y, 1);

            Transform2D transform2 = Transform2D.CreateReflectionAtPoint(new Vector2D(0, 1), new Point2D(1, 1));
            var res2 = transform2.OfPoint(new Point2D(2, 2));

            Assert.Equal(res2.X, 0);
            Assert.Equal(res2.Y, 2);

            Transform2D transform3 = Transform2D.CreateReflection(new Vector2D(1, 1));
            var res3 = transform3.OfPoint(new Point2D(1, 0));

            Assert.Equal(res3.X, 0);
            Assert.Equal(res3.Y, 1);
        }

        [Fact]
        public void TestMethod6()
        {
            Transform3D transform1 = Transform3D.CreateReflection(new Plane3D(new Vector3D(1, 0, 0), Point3D.Zero));
            var res1 = transform1.OfPoint(new Point3D(1, 1, 0));

            Assert.Equal(res1.X, -1);
            Assert.Equal(res1.Y, 1);

            Transform3D transform2 = Transform3D.CreateReflection(new Plane3D(new Vector3D(1, 1, 0), Point3D.Zero));
            var res2 = transform2.OfPoint(new Point3D(1, 0, 0));

            Assert.Equal(res2.X, 0, 0.0001);
            Assert.Equal(res2.Y, -1, 0.00001);

            Transform3D transform3 = Transform3D.CreateReflection(new Plane3D(new Vector3D(1, 1, 0), new Point3D(1, 0, 0)));
            var res3 = transform3.OfPoint(Point3D.Zero);

            Assert.Equal(res3.X, 1, 0.0001);
            Assert.Equal(res3.Y, 1, 0.00001);

            Transform3D transform4 = Transform3D.CreateReflection(new Plane3D(new Vector3D(1, -1, 0), new Point3D(1, 0, 0)));
            var res4 = transform4.OfPoint(Point3D.Zero);

            Assert.Equal(res4.X, 1, 0.0001);
            Assert.Equal(res4.Y, -1, 0.00001);
        }

    }
}
