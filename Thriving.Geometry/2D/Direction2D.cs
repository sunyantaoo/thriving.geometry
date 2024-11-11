namespace Thriving.Geometry
{
    /// <summary>
    /// 平面方向，即平面单位向量
    /// </summary>
    public readonly struct Direction2D 
    {
        private readonly double _x, _y;

        public Direction2D(Vector2D vector)
        {
            var nor = vector.Normalize();

            this._x = nor.X;
            this._y = nor.Y;
        }

        public Direction2D(double x,double y):this(new Vector2D(x,y))
        {
            
        }

    }
}