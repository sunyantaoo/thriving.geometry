namespace Thriving.Geometry.Test
{
    public class SlopeTest
    {
        [Fact]
        public void TestMethod1()
        {
            var slope = new Slope(1);

            var dir = slope.GetDirection(Vector3D.BasisX, Vector3D.BasisY);
            Assert.True(dir.IsAlmostEqualTo(new Vector3D(1, 1, 0).Normalize()));
            var nor = slope.GetNormal(Vector3D.BasisX, Vector3D.BasisY);
            Assert.True(nor.IsAlmostEqualTo(new Vector3D(-1, 1, 0).Normalize()));

            var h1 = slope.GetWidthByHeight(1);
            Assert.Equal(h1, 1.0);

            var h3 = slope.GetWidthByLength(Math.Sqrt(2));
            Assert.Equal(h3, 1.0);

            var h2 = slope.GetHeightByWidth(1);
            Assert.Equal(h2, 1.0);

            var h4 = slope.GetHeightByLength(Math.Sqrt(2));
            Assert.Equal(h4, 1.0);

            var h5 = slope.GetLengthByHeight(1);
            Assert.Equal(h5, Math.Sqrt(2));

            var h6 = slope.GetLengthByWidth(1);
            Assert.Equal(h6, Math.Sqrt(2));
        }

        [Fact]
        public void TestMethod2()
        {
            var heigth = 3;
            var length = 1.5;
            var tt = Math.Sqrt(length * length + heigth * heigth);

            var slope=new Slope(heigth,length);

            var dir = slope.GetDirection(Vector3D.BasisX, Vector3D.BasisY);
            Assert.True(dir.IsAlmostEqualTo(new Vector3D(length, heigth, 0).Normalize()));
            var nor = slope.GetNormal(Vector3D.BasisX, Vector3D.BasisY);
            Assert.True(nor.IsAlmostEqualTo(new Vector3D(-heigth, length, 0).Normalize()));

            var h1 = slope.GetWidthByHeight(heigth);
            Assert.Equal(h1, length);

            var h3 = slope.GetWidthByLength(tt);
            Assert.Equal(h3, length);

            var h2 = slope.GetHeightByWidth(length);
            Assert.Equal(h2, heigth);

            var h4 = slope.GetHeightByLength(tt);
            Assert.Equal(h4, heigth);

            var h5 = slope.GetLengthByHeight(heigth);
            Assert.Equal(h5, tt);

            var h6 = slope.GetLengthByWidth(length);
            Assert.Equal(h6, tt);
        }


        [Fact]
        public void TestMethod3()
        {
            var heigth = 15;
            var length = 50;
            var tt = Math.Sqrt(length * length + heigth * heigth);

            var slope =  Slope.CreatePercent(30);

            var dir = slope.GetDirection(Vector3D.BasisX, Vector3D.BasisY);
            Assert.True(dir.IsAlmostEqualTo(new Vector3D(length, heigth, 0).Normalize()));
            var nor = slope.GetNormal(Vector3D.BasisX, Vector3D.BasisY);
            Assert.True(nor.IsAlmostEqualTo(new Vector3D(-heigth,length, 0).Normalize()));

            var h1 = slope.GetWidthByHeight(heigth);
            Assert.Equal(h1, length,GeometryUtility.Tolerance);

            var h3 = slope.GetWidthByLength(tt);
            Assert.Equal(h3, length, GeometryUtility.Tolerance);

            var h2 = slope.GetHeightByWidth(length);
            Assert.Equal(h2, heigth, GeometryUtility.Tolerance);

            var h4 = slope.GetHeightByLength(tt);
            Assert.Equal(h4, heigth, GeometryUtility.Tolerance);

            var h5 = slope.GetLengthByHeight(heigth);
            Assert.Equal(h5, tt, GeometryUtility.Tolerance);

            var h6 = slope.GetLengthByWidth(length);
            Assert.Equal(h6, tt, GeometryUtility.Tolerance);
        }
    }
}
