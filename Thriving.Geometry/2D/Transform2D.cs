using System.Runtime.InteropServices;

namespace Thriving.Geometry
{
    /// <summary>
    /// 二维坐标变换矩阵
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Transform2D
    {
        private readonly double _basisXX, _basisYX, _originX;
        private readonly double _basisXY, _basisYY, _originY;
        private readonly double _basisXH, _basisYH, _originH;

        public Vector2D BasisX { get => new Vector2D(_basisXX, _basisXY); }
        public Vector2D BasisY { get => new Vector2D(_basisYX, _basisYY); }
        public Point2D Origin { get => new Point2D(_originX, _originY); }

        public Transform2D(Point2D origin, Vector2D basisX, Vector2D basisY)
        {
            _basisXX = basisX.X;
            _basisXY = basisX.Y;
            _basisXH = 0;

            _basisYX = basisY.X;
            _basisYY = basisY.Y;
            _basisYH = 0;

            _originX = origin.X;
            _originY = origin.Y;
            _originH = 1;
        }

        internal Transform2D(Matrix matrix)
        {
            if (matrix.RowCount != 3 || matrix.ColCount != 3) throw new ArgumentException("矩阵大小不符合二维局部坐标系要求");

            _basisXX = matrix[0, 0];
            _basisXY = matrix[1, 0];
            _basisXH = matrix[2, 0];

            _basisYX = matrix[0, 1];
            _basisYY = matrix[1, 1];
            _basisYH = matrix[2, 1];

            _originX = matrix[0, 2];
            _originY = matrix[1, 2];
            _originH = matrix[2, 2];
        }


        public static Transform2D Identity { get => new Transform2D(Point2D.Zero, Vector2D.BasisX, Vector2D.BasisY); }

        public Matrix Matrix
        {
            get
            {
                var data = new double[3, 3]
                {
                    { _basisXX, _basisYX, _originX },
                    { _basisXY, _basisYY, _originY },
                    { _basisXH, _basisYH, _originH },
                };
                return new Matrix(data);
            }
        }


        /// <summary>
        /// 是否为右手坐标系
        /// </summary>
        /// <returns></returns>
        public bool IsRightHand()
        {
            return BasisX.X * BasisY.Y > BasisX.Y * BasisY.X;
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Transform2D CreateTranslation(Vector2D vector)
        {
            return new Transform2D(Point2D.Zero.Add(vector), Vector2D.BasisX, Vector2D.BasisY);
        }

        /// <summary>
        /// 绕中心旋转矩阵
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Transform2D CreateRotation(double angle)
        {
            var basisX = new Vector2D(Math.Cos(angle), Math.Sin(angle));
            var basisY = new Vector2D(-Math.Sin(angle), Math.Cos(angle));

            return new Transform2D(Point2D.Zero, basisX, basisY);
        }

        /// <summary>
        /// 绕指定点的旋转矩阵
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Transform2D CreateRotationAtPoint(double angle, Point2D origin)
        {
            var transA = CreateTranslation(origin.Vector);
            var transB = CreateRotation(angle);
            var transC = CreateTranslation(origin.Vector.Negate());

            return transA.Multiply(transB).Multiply(transC);
        }

        /// <summary>
        /// 缩放矩阵
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Transform2D CreateScale(double x, double y)
        {
            var basisX = x * Vector2D.BasisX;
            var basisY = y * Vector2D.BasisY;

            return new Transform2D(Point2D.Zero, basisX, basisY);
        }

        /// <summary>
        /// 在指定点处的缩放矩阵
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Transform2D CreateScaleAtPoint(double x, double y, Point2D origin)
        {
            var transA = CreateTranslation(origin.Vector);
            var transB = CreateScale(x, y);
            var transC = CreateTranslation(origin.Vector.Negate());

            return transA.Multiply(transB).Multiply(transC);
        }

        /// <summary>
        /// 以指定向量为镜像轴的镜像矩阵
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Transform2D CreateReflection(Vector2D vector)
        {
            var v = vector.Normalize();
            if (v.IsEqualTo(Vector2D.BasisX))
            {
                return new Transform2D(Point2D.Zero, Vector2D.BasisX, Vector2D.BasisY.Negate());
            }
            else if (v.IsEqualTo(Vector2D.BasisY))
            {
                return new Transform2D(Point2D.Zero, Vector2D.BasisX.Negate(), Vector2D.BasisY);
            }
            else
            {
                var basisX = (vector - Vector2D.BasisX).Normalize();
                var basisY = (vector - Vector2D.BasisY).Normalize();

                return new Transform2D(Point2D.Zero, basisX, basisY);
            }
        }

        /// <summary>
        /// 以过指定点的向量为镜像轴的镜像矩阵
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static Transform2D CreateReflectionAtPoint(Vector2D vector, Point2D origin)
        {
            var transA = CreateTranslation(origin.Vector);
            var transB = CreateReflection(vector);
            var transC = CreateTranslation(origin.Vector.Negate());

            return transA.Multiply(transB).Multiply(transC);
        }

        public Point2D OfPoint(Point2D point)
        {
            var result = Matrix * point.Matrix;
            return new Point2D(result[0, 0], result[1, 0]);
        }

        public Vector2D OfVector(Vector2D vector)
        {
            var result = Matrix * vector.Matrix;
            return new Vector2D(result[0, 0] - _originX, result[1, 0] - _originY);
        }

        public Transform2D Multiply(Transform2D other)
        {
            var result = Matrix * other.Matrix;
            return new Transform2D(result);
        }

        /// <summary>
        /// 逆矩阵
        /// </summary>
        public Transform2D Inverse
        {
            get
            {
                return new Transform2D(Matrix.Inverse);
            }
        }
    }
}