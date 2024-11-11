namespace Thriving.Geometry
{
    public static class GeometryUtility
    {
        private static double _tolerance = 1e-9;
        public static double Tolerance { get => _tolerance; }

        /// <summary>
        /// 配置全局计算精度
        /// </summary>
        /// <param name="tolerance"></param>
        public static void UseTolerance(double tolerance)
        {
            _tolerance = tolerance;
        }
    }
}