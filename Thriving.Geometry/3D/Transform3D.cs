using System.Runtime.InteropServices;

namespace Thriving.Geometry
{
    /// <summary>
    /// 三维坐标转换
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Transform3D
    {
        private readonly double _basisXX, _basisYX, _basisZX, _originX;
        private readonly double _basisXY, _basisYY, _basisZY, _originY;
        private readonly double _basisXZ, _basisYZ, _basisZZ, _originZ;
        private readonly double _basisXH, _basisYH, _basisZH,_originH;

        public Vector3D BasisX { get => new Vector3D(_basisXX, _basisXY, _basisXZ); }
        public Vector3D BasisY { get => new Vector3D(_basisYX, _basisYY, _basisYZ); }
        public Vector3D BasisZ { get => new Vector3D(_basisZX, _basisZY, _basisZZ); }
        public Point3D Origin { get => new Point3D(_originX, _originY, _originZ); }

        internal Transform3D(Matrix matrix)
        {
            if (matrix.RowCount != 4 || matrix.ColCount != 4) throw new ArgumentException("矩阵大小不符合三维局部坐标系要求");

            _basisXX = matrix[0, 0];
            _basisXY = matrix[1, 0];
            _basisXZ = matrix[2, 0];
            _basisXH = matrix[3, 0];

            _basisYX = matrix[0, 1];
            _basisYY = matrix[1, 1];
            _basisYZ = matrix[2, 1];
            _basisYH = matrix[3, 1];

            _basisZX = matrix[0, 2];
            _basisZY = matrix[1, 2];
            _basisZZ = matrix[2, 2];
            _basisZH = matrix[3, 2];

            _originX = matrix[0, 3];
            _originY = matrix[1, 3];
            _originZ = matrix[2, 3];
            _originH = matrix[3, 3];
        }

        public Transform3D(Point3D origin, Vector3D basisX, Vector3D basisY, Vector3D basisZ)
        {
            _basisXX = basisX.X;
            _basisXY = basisX.Y;
            _basisXZ = basisX.Z;
            _basisXH = 0;

            _basisYX = basisY.X;
            _basisYY = basisY.Y;
            _basisYZ = basisY.Z;
            _basisYH = 0;

            _basisZX = basisZ.X;
            _basisZY = basisZ.Y;
            _basisZZ = basisZ.Z;
            _basisZH = 0;

            _originX = origin.X;
            _originY = origin.Y;
            _originZ = origin.Z;
            _originH = 1;
        }

        public static Transform3D Identity { get => new Transform3D(Point3D.Zero, Vector3D.BasisX, Vector3D.BasisY, Vector3D.BasisZ); }

        public Matrix Matrix
        {
            get
            {
                var data = new double[4, 4]
                {
                    { _basisXX, _basisYX, _basisZX, _originX },
                    { _basisXY, _basisYY, _basisZY, _originY },
                    { _basisXZ, _basisYZ, _basisZZ, _originZ },
                    { _basisXH, _basisYH, _basisZH, _originH }
                };
                return new Matrix(data);
            }
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Transform3D CreateTranslation(Vector3D vector)
        {
            return new Transform3D(Point3D.Zero.Add(vector), Vector3D.BasisX, Vector3D.BasisY, Vector3D.BasisZ);
        }

        /// <summary>
        /// 绕中心旋转矩阵
        /// </summary>
        /// <param name="axis">旋转轴</param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Transform3D CreateRotation(Vector3D axis, double angle)
        {
            // 罗德里格斯旋转矩阵
            var basisX = new Vector3D(
                axis.X * axis.X * (1 - Math.Cos(angle)) + Math.Cos(angle),
                axis.Y * axis.X * (1 - Math.Cos(angle)) + axis.Z * Math.Sin(angle),
                axis.Z * axis.X * (1 - Math.Cos(angle)) - axis.Y * Math.Sin(angle)
                );
            var basisY = new Vector3D(
                axis.X * axis.Y * (1 - Math.Cos(angle)) - axis.Z * Math.Sin(angle),
                axis.Y * axis.Y * (1 - Math.Cos(angle)) + Math.Cos(angle),
                axis.Z * axis.Y * (1 - Math.Cos(angle)) + axis.X * Math.Sin(angle)
                );
            var basisZ = new Vector3D(
                axis.X * axis.Z * (1 - Math.Cos(angle)) + axis.Y * Math.Sin(angle),
                axis.Y * axis.Z * (1 - Math.Cos(angle)) - axis.X * Math.Sin(angle),
                axis.Z * axis.Z * (1 - Math.Cos(angle)) + Math.Cos(angle)
                );
            return new Transform3D(Point3D.Zero, basisX.Normalize(), basisY.Normalize(), basisZ.Normalize());
        }

        /// <summary>
        /// 绕指定点旋转矩阵
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="angle"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static Transform3D CreateRotationAtPoint(Vector3D axis, double angle, Point3D origin)
        {
            var transA = CreateTranslation(origin.Vector);
            var transB = CreateRotation(axis, angle);
            var transC = CreateTranslation(origin.Vector.Negate());

            return transA.Multiply(transB).Multiply(transC);
        }

        /// <summary>
        /// 缩放矩阵
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Transform3D CreateScale(double x, double y, double z)
        {
            var basisX = x * Vector3D.BasisX;
            var basisY = y * Vector3D.BasisY;
            var basisZ = z * Vector3D.BasisZ;

            return new Transform3D(Point3D.Zero, basisX, basisY, basisZ);
        }

        /// <summary>
        /// 在指定点处的缩放矩阵
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Transform3D CreateScaleAtPoint(double x, double y, double z, Point3D origin)
        {
            var transA = CreateTranslation(origin.Vector);
            var transB = CreateScale(x, y, z);
            var transC = CreateTranslation(origin.Vector.Negate());

            return transA.Multiply(transB).Multiply(transC);
        }

        /// <summary>
        /// 指定镜像平面法向量的镜像矩阵
        /// </summary>
        /// <param name="basisX"></param>
        /// <returns></returns>
        public static Transform3D CreateReflection(Plane3D plane)
        {
            var normal = plane.Normal;
            var d = plane.DistanceTo(Point3D.Zero);

            var basisX = (new Vector3D(1 - 2 * normal.X * normal.X, -2 * normal.X * normal.Y, -2 * normal.X * normal.Z)).Normalize();
            var basisY = (new Vector3D(-2 * normal.Y * normal.X, 1 - 2 * normal.Y * normal.Y, -2 * normal.Y * normal.Z)).Normalize();
            var basisZ = (new Vector3D(-2 * normal.Z * normal.X, -2 * normal.Z * normal.Y, 1 - 2 * normal.Z * normal.Z)).Normalize();
            var origin = new Point3D(-2 * d * normal.X, -2 * d * normal.Y, -2 * d * normal.Z);

            return new Transform3D(origin, basisX, basisY, basisZ);
        }

        public Point3D OfPoint(Point3D point)
        {
            var result = Matrix * point.Matrix;
            return new Point3D(result[0, 0], result[1, 0], result[2, 0]);
        }

        public Vector3D OfVector(Vector3D vector)
        {
            var result = Matrix * vector.Matrix;
            return new Vector3D(result[0, 0] - _originX, result[1, 0] - _originY, result[2, 0] - _originZ);
        }

        public Transform3D Multiply(Transform3D other)
        {
            var result = Matrix * other.Matrix;
            return new Transform3D(result);
        }

        /// <summary>
        /// 逆矩阵
        /// </summary>
        public Transform3D Inverse
        {
            get
            {
                return new Transform3D(Matrix.Inverse);
            }
        }
    }
}