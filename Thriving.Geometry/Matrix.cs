namespace Thriving.Geometry
{
    /// <summary>
    /// 矩阵
    /// </summary>
    public class  Matrix
    {
        private readonly double[,] _data;

        /// <summary>
        ///  行数
        /// </summary>
        public int RowCount { get => _data.GetLength(0); }

        /// <summary>
        /// 列数
        /// </summary>
        public int ColCount { get => _data.GetLength(1); }

        public double this[int i, int j]
        {
            get { return _data[i, j]; }
            set { _data[i, j] = value; }
        }

        public Matrix(int rowCount, int colCount)
        {
            this._data = new double[rowCount, colCount];
        }

        public Matrix(double[,] data)
        {
            this._data = data;
        }

        /// <summary>
        /// 行列式值
        /// </summary>
        public double DetValue
        {
            get
            {
                if (RowCount == 1)
                {
                    return _data[0, 0];
                }
                else
                {
                    double sum = 0;
                    for (int j = 0; j < ColCount; j++)
                    {
                        var cofactor = GetCofactor(0, j);
                        sum += _data[0, j] * Math.Pow(-1, 0 + j) * cofactor.DetValue;
                    }
                    return sum;
                }
            }
        }

        /// <summary>
        /// 逆矩阵
        /// </summary>
        public Matrix Inverse
        {
            get
            {
                var result = new Matrix(RowCount, ColCount);
                for (int i = 0; i < RowCount; i++)
                {
                    for (int j = 0; j < ColCount; j++)
                    {
                        var colfactor = GetCofactor(j, i);
                        result[i, j] = Math.Pow(-1, i + j) * colfactor.DetValue / DetValue;
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 转置矩阵
        /// </summary>
        public Matrix Transpose
        {
            get
            {
                var result = new Matrix(ColCount, RowCount);
                for (int i = 0; i < RowCount; i++)
                {
                    for (int j = 0; j < ColCount; j++)
                    {
                        result[j, i] = _data[i, j];
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 伴随矩阵
        /// </summary>
        public Matrix Adjoint
        {
            get
            {
                var result = new Matrix(RowCount, ColCount);
                for (int i = 0; i < RowCount; i++)
                {
                    for (int j = 0; j < ColCount; j++)
                    {
                        var colfacotr = GetCofactor(i, j);
                        result[i, j] = Math.Pow(-1, i + j) * colfacotr.DetValue;
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 去掉第m行和第n列形成新的矩阵
        /// </summary>
        /// <param name="m">第m行</param>
        /// <param name="n">第n列</param>
        /// <returns></returns>
        public Matrix GetCofactor(int m, int n)
        {
            var result = new Matrix(RowCount - 1, ColCount - 1);
            for (int i = 0; i < RowCount - 1; i++)
            {
                for (int j = 0; j < ColCount - 1; j++)
                {
                    result[i, j] = _data[i < m ? i : i + 1, j < n ? j : j + 1];
                }
            }
            return result;
        }

        public static Matrix operator +(Matrix m, Matrix n)
        {
            if (m.ColCount != n.ColCount || m.RowCount == n.RowCount)
            {
                throw new InvalidOperationException("矩阵加法，两矩阵的行、列数必须相同");
            }

            var rowCount = m.RowCount;
            var colCount = m.ColCount;

            var result = new Matrix(rowCount, colCount);
            for (int i = 0; i < m.RowCount; i++)
            {
                for (int j = 0; j < m.ColCount; j++)
                {
                    result[i, j] = m[i, j] + n[i, j];
                }
            }
            return result;
        }

        public static Matrix operator -(Matrix m, Matrix n)
        {
            if (m.ColCount != n.ColCount || m.RowCount == n.RowCount)
            {
                throw new InvalidOperationException("矩阵减法，两矩阵的行、列数必须相同");
            }

            var rowCount = m.RowCount;
            var colCount = m.ColCount;

            var result = new Matrix(rowCount, colCount);
            for (int i = 0; i < m.RowCount; i++)
            {
                for (int j = 0; j < m.ColCount; j++)
                {
                    result[i, j] = m[i, j] + n[i, j];
                }
            }
            return result;
        }

        public static Matrix operator *(double value, Matrix m)
        {
            var result = new Matrix(m.RowCount, m.ColCount);
            for (int i = 0; i < m.RowCount; i++)
            {
                for (int j = 0; j < m.ColCount; j++)
                {
                    result[i, j] = value * m[i, j];
                }
            }
            return result;
        }

        public static Matrix operator *(Matrix m, Matrix n)
        {
            if (m.ColCount != n.RowCount)
            {
                throw new InvalidOperationException("矩阵乘法，左值的列数与右值的行数必须相等");
            }

            var result = new Matrix(m.RowCount, n.ColCount);
            for (int i = 0; i < m.RowCount; i++)
            {
                for (int j = 0; j < n.ColCount; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < m.ColCount; k++)
                    {
                        sum += m[i, k] * n[k, j];
                    }
                    result[i, j] = sum;
                }
            }
            return result;
        }
    }
}