namespace Thriving.Geometry.Test
{
    public class LineTest
    {
        [Fact]
        public void TestMethod1()
        {
            var line = new Line2D(new Point2D(), new Vector2D(2, 2));

            var d1 = line.DistanceTo(new Point2D(1, 0));
            Assert.Equal(0.5, d1 * d1, 1e-9);

            var d2 = line.DistanceTo(new Point2D(0, 1));
            Assert.Equal(0.5, d2 * d2, 1e-9);
        }

        [Fact]
        public void TestMethod2()
        {
            var line = new Line2D(1, 0);

            var d1 = line.DistanceTo(new Point2D(1, 0));
            Assert.Equal(0.5, d1 * d1, 1e-9);

            var d2 = line.DistanceTo(new Point2D(0, 1));
            Assert.Equal(0.5, d2 * d2, 1e-9);
        }
    }
}
