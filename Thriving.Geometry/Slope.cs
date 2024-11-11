namespace Thriving.Geometry
{
    /// <summary>
    /// 坡度，坡面的垂直高度和水平方向宽度的比，单位无关
    /// </summary>
    public class Slope
    {
        private readonly double _value;
        /// <summary>
        /// 定义表示方式
        /// </summary>
        /// <param name="height">垂直高度</param>
        /// <param name="width">水平方向的宽度</param>
        public Slope(double height, double width)
        {
            this._value = width / height;
        }
        /// <summary>
        /// 1：value的表示方法
        /// </summary>
        /// <param name="value"></param>
        public Slope(double value)
        {
            this._value = value;
        }
        /// <summary>
        /// 百分数的表示方法
        /// </summary>
        /// <param name="value">百分数</param>
        /// <returns></returns>
        public static Slope CreatePercent(double value)
        {
            return new Slope(value, 100);
        }
        /// <summary>
        /// 角度表示方法
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Slope CreateAngle(double angle)
        {
            return new Slope(Math.Tan(angle));
        }


        /// <summary>
        /// 垂直高度为单位高度时的水平方向长度
        /// </summary>
        public double Value { get => _value; }
        /// <summary>
        /// 垂直高度为单位高度时的坡长，单位无关
        /// </summary>
        public double Length { get => Math.Sqrt(Value * Value + 1); }
        /// <summary>
        /// 坡线的竖向夹角
        /// </summary>
        public double Angle { get => Math.Atan(Value); }

        /// <summary>
        /// 坡度在X轴和Y轴平面的坡线方向
        /// </summary>
        /// <param name="xAxis"></param>
        /// <param name="yAxis"></param>
        /// <returns></returns>
        public Vector3D GetDirection(Vector3D xAxis, Vector3D yAxis)
        {
            return (Value * xAxis + yAxis).Normalize();
        }

        /// <summary>
        /// 坡度在X轴和Y轴平面的坡线的法方向
        /// </summary>
        /// <param name="xAxis"></param>
        /// <param name="yAxis"></param>
        /// <returns></returns>
        public Vector3D GetNormal(Vector3D xAxis, Vector3D yAxis)
        {
            return (Value * yAxis - xAxis).Normalize();
        }

        /// <summary>
        /// 已知坡高求水平长度
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        public double GetWidthByHeight(double height)
        {
            return height * _value;
        }

        /// <summary>
        /// 已知坡线总长求水平长度
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public double GetWidthByLength(double length)
        {
            return length * _value / Math.Sqrt(1 + _value * _value);
        }

        /// <summary>
        /// 已知坡高求坡长
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        public double GetLengthByHeight(double height)
        {
            return Math.Sqrt(Math.Pow(height * Value, 2) + Math.Pow(height, 2));
        }

        /// <summary>
        /// 已知水平长度求坡长
        /// </summary>
        /// <param name="width"></param>
        /// <returns></returns>
        public double GetLengthByWidth(double width)
        {
            return Math.Sqrt(Math.Pow(width / Value, 2) + Math.Pow(width, 2));
        }


        /// <summary>
        /// 已知水平长度求坡高
        /// </summary>
        /// <param name="width"></param>
        /// <returns></returns>
        public double GetHeightByWidth(double width)
        {
            return width / _value;
        }

        /// <summary>
        /// 已知坡线总长求高度
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public double GetHeightByLength(double length)
        {
            return length / Math.Sqrt(1 + _value * _value);
        }
    }
}