namespace Thriving.Geometry
{
    /// <summary>
    /// 曲线与曲线某一个相交结果
    /// </summary>
    public class IntersectResult<T> where T : struct
    {
        /// <summary>
        /// 交点
        /// </summary>
        public T Point { get; set; }

        /// <summary>
        /// 交点在有界曲线上的比例[0,1]
        /// </summary>
        public double UParameter { get; set; }

        /// <summary>
        /// 交点在有界曲线上的比例[0,1]
        /// </summary>
        public double VParameter { get; set; }
    }
}