namespace Thriving.Geometry.Test
{
    public class MatrixTest
    {
        [Fact]
        public void TestMethod1()
        {
            var data = new double[,]
            {
                { 1, 0, 0 },
                { 0, 1, 0 }
            };

            var matrix = new Matrix(data);

            Assert.Equal(2, matrix.RowCount);
            Assert.Equal(3, matrix.ColCount);
        }


        [Fact]
        public void TestMethod2()
        {
            var data = new double[,] { { 1, 0, 0 }, { 0, 1, 0 } };
            var matrix = new Matrix(data);

            var transpose = matrix.Transpose;

            Assert.Equal(3, transpose.RowCount);
            Assert.Equal(2, transpose.ColCount);
            Assert.Equal(1, transpose[0, 0]);
            Assert.Equal(1, transpose[1, 1]);
        }


        [Fact]
        public void TestMethod3()
        {
            var data1 = new double[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };
            var matrix1 = new Matrix(data1);

            var data2 = new double[,] { { 1 }, { 1 }, { 1 } };
            var matrix2 = new Matrix(data2);

            var result = matrix1 * matrix2;

            Assert.Equal(3, result.RowCount);
            Assert.Equal(1, result.ColCount);
            Assert.Equal(1, result[0, 0]);
            Assert.Equal(1, result[1, 0]);
            Assert.Equal(1, result[2, 0]);
        }

        [Fact]
        public void TestMethod4()
        {
            var data1 = new double[,] { { 1, 0, 3, -1 }, { 2, 1, 0, 2 } };
            var matrix1 = new Matrix(data1);

            var data2 = new double[,] { { 4, 1, 0 }, { -1, 1, 3 }, { 2, 0, 1 }, { 1, 3, 4 } };
            var matrix2 = new Matrix(data2);

            var result = matrix1 * matrix2;

            Assert.Equal(9, result[0, 0]);
            Assert.Equal(-2, result[0, 1]);
            Assert.Equal(-1, result[0, 2]);
            Assert.Equal(9, result[1, 0]);
            Assert.Equal(9, result[1, 1]);
            Assert.Equal(11, result[1, 2]);
        }


        [Fact]
        public void TestMethod5()
        {
            var data = new double[,] { { 1, 0, 1 }, { 0, 1, 1 }, { 0, 0, 1 } };
            var matrix = new Matrix(data);

            var result = matrix.Inverse;

            Assert.Equal(3, result.RowCount);
            Assert.Equal(3, result.ColCount);
            Assert.Equal(-1, result[0, 2]);
            Assert.Equal(-1, result[1, 2]);
            Assert.Equal(1, result[2, 2]);
        }
    }
}
