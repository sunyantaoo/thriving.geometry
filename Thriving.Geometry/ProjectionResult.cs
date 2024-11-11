namespace Thriving.Geometry
{
    /// <summary>
    /// 点到线或面的投影结果
    /// </summary>
    public class ProjectionResult<T> where T : struct
    {
        /// <summary>
        /// 点到曲线的最近点
        /// </summary>
        public T Point { get; set; }

        /// <summary>
        /// 最近点在有界曲线上的比例[0,1]
        /// </summary>
        public double Parameter { get; set; }

        /// <summary>
        /// 点到曲线的距离，即点到最近点的距离
        /// </summary>
        public double Distance { get; set; }
    }
}