namespace Thriving.Geometry
{
    /// <summary>
    /// 圆弧段
    /// </summary>
    public class Arc2D : BoundedCurve2D
    {
        private readonly double _radius;
        private readonly double _startAngle;
        private readonly double _endAngle;

        public Point2D Center { get => _transform.Origin; }
        public double Radius { get => _radius; }
        public double StartAngle { get => _startAngle; }
        public double EndAngle { get => _endAngle; }

        public double Length { get => _radius * (_endAngle - _startAngle); }

        public override Point2D StartPoint { get => GetPoint(0); }

        public override Point2D EndPoint { get => GetPoint(1); }

        /// <summary>
        /// 逆时针
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="startAngle"></param>
        /// <param name="endAngle"></param>
        public Arc2D(Point2D center, double radius, double startAngle, double endAngle, Vector2D basisX, Vector2D basisY)
        {
            this._radius = radius;
            this._startAngle = startAngle;
            this._endAngle = endAngle;

            this._transform = new Transform2D(center, basisX, basisY);
        }

        public Arc2D(Point2D startPoint, Point2D endPoint, Point2D midPoint)
        {
            var triangle = new Triangle2D(startPoint, endPoint, midPoint);

            var center = triangle.CircumCenter();
            this._radius = center.DistanceTo(startPoint);

            this._startAngle = 0;

            var basisX = new Vector2D(center, startPoint).Normalize();
            var basisY = basisX.Vertical();

            var angle = new Vector2D(center, startPoint).AngleTo(new Vector2D(center, endPoint));
            if (new Vector2D(center, startPoint).IsCounterClockwise(new Vector2D(center, endPoint)))
            {
                if (new Vector2D(midPoint, endPoint).IsCounterClockwise(new Vector2D(midPoint, startPoint)))
                {
                    this._endAngle = angle;
                }
                else
                {
                    basisY = basisY.Negate();
                    this._endAngle = 2 * Math.PI - angle;
                }
            }
            else
            {
                if (new Vector2D(midPoint, endPoint).IsCounterClockwise(new Vector2D(midPoint, startPoint)))
                {
                    this._endAngle = 2 * Math.PI - angle;
                }
                else
                {
                    this._endAngle = angle;
                    basisY = basisY.Negate();
                }
            }

            this._transform = new Transform2D(center, basisX, basisY);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="center">圆心</param>
        /// <param name="startPoint">起点</param>
        /// <param name="endPoint">终点</param>
        /// <param name="ccw">是否为逆时针方向</param>
        public Arc2D(Point2D center, Point2D startPoint, Point2D endPoint, bool ccw)
        {
            var radis1 = center.DistanceTo(startPoint);
            var radis2 = center.DistanceTo(endPoint);

            if (Math.Abs(radis1 - radis2) > GeometryUtility.Tolerance) throw new ArgumentException("无效参数，不能构建圆弧");

            this._radius = 0.5 * (radis1 + radis2);

            var dir1 = new Vector2D(center, startPoint);
            var dir2 = new Vector2D(center, endPoint);

            this._startAngle = 0;
           
            var angle = dir1.AngleTo(dir2);

            var basisX=dir1.Normalize();
            var basisY = basisX.Vertical();
            if (dir1.IsCounterClockwise(dir2))
            {
                if (ccw)
                {
                    this._endAngle = angle;
                }
                else
                {
                    basisY = basisY.Negate();
                    this._endAngle = 2 * Math.PI - angle;
                }
            }
            else
            {
                if(ccw)
                {
                    this._endAngle= 2 * Math.PI - angle;
                }
                else
                {
                    basisY = basisY.Negate();
                    this._endAngle = angle;
                }
            }

            this._transform = new Transform2D(center, basisX, basisY);
        }

        public Arc2D(Point2D center, Point2D startPoint, double angle, bool ccw)
        {
            this._radius = center.DistanceTo(startPoint);
            this._endAngle = angle;

            this._startAngle = 0;

            var dir1 = new Vector2D(center, startPoint);

            var basisX = dir1.Normalize();
            var basisY = ccw ? basisX.Vertical() : basisX.Vertical().Negate();

            this._transform = new Transform2D(center, basisX, basisY);
        }

        /// <summary>
        /// 获取圆弧上的点
        /// </summary>
        /// <param name="ratio">[0,1]之间</param>
        /// <returns></returns>
        public Point2D GetPoint(double ratio)
        {
            var angle = _startAngle + ratio * (_endAngle - _startAngle);
            var result = new Point2D(_radius * Math.Cos(angle), _radius * Math.Sin(angle));

            return _transform.OfPoint(result);
        }

        /// <summary>
        /// 获取圆弧上点的切向量
        /// </summary>
        /// <param name="ratio">[0,1]之间</param>
        /// <returns></returns>
        public Vector2D GetTangentVector(double ratio)
        {
            var angle = _startAngle + ratio * (_endAngle - _startAngle);
            var result = new Vector2D(-Math.Sin(angle), Math.Cos(angle));

            return _transform.OfVector(result);
        }

        public IList<IntersectResult<Point2D>>? Intersect(Line2D line)
        {
            var circle = new Circle2D(Center, _radius);

            var temp = circle.Intersect(line);
            if (temp == null) return null;

            IList<IntersectResult<Point2D>> result = new List<IntersectResult<Point2D>>();
            var oA = new Vector2D(Center, StartPoint);
            var oB = new Vector2D(Center, EndPoint);
            foreach (var item in temp)
            {
                // 判断交点是否在圆弧范围内
                var op = new Vector2D(Center, item.Point);
                if (_endAngle - _startAngle > Math.PI)
                {
                    var angle = 2 * Math.PI - (_endAngle - _startAngle);
                    // 大于180度顺时针
                    if (oA.IsCounterClockwise(oB))
                    {
                        var angleA = oA.AngleTo(op);
                        var angleB = op.AngleTo(oB);
                        if (Math.Abs((angleA + angleB) - angle) < GeometryUtility.Tolerance)
                        {
                            item.UParameter = (angle) / (angleA + angleB);
                            result.Add(item);
                        }
                    }
                    // 大于180度逆时针
                    else
                    {
                        var angleA = oA.AngleTo(op);
                        var angleB = op.AngleTo(oB);
                        if (Math.Abs((angleA + angleB) - angle) < GeometryUtility.Tolerance)
                        {
                            item.UParameter = (angle) / (angleA + angleB);
                            result.Add(item);
                        }
                    }
                }
                else
                {
                    // 小于180度逆时针
                    if (oA.IsCounterClockwise(oB))
                    {
                        var angleA = oA.AngleTo(op);
                        var angleB = op.AngleTo(oB);
                        if (Math.Abs((angleA + angleB) - (_endAngle - _startAngle)) < GeometryUtility.Tolerance)
                        {
                            item.UParameter =  angleA / (_endAngle - _startAngle);
                            result.Add(item);
                        }
                    }
                    // 小于180度顺时针
                    else
                    {
                        var angleA = oA.AngleTo(op);
                        var angleB = op.AngleTo(oB);
                        if (Math.Abs((angleA + angleB) - (_endAngle - _startAngle)) < GeometryUtility.Tolerance)
                        {
                            item.UParameter = angleA  / (_endAngle - _startAngle);
                            result.Add(item);
                        }
                    }
                }
            };
            return result;
        }

        /// <summary>
        /// 延长圆弧
        /// </summary>
        /// <param name="start">起点延长比例</param>
        /// <param name="end">终点延长比例</param>
        /// <returns></returns>
        public Arc2D Extend(double start, double end)
        {
            var startAngle = _startAngle - start * (_endAngle - _startAngle);
            var endAngle = _endAngle + (end * (_endAngle - _startAngle));

            if (endAngle - startAngle > 2 * Math.PI)
            {
                throw new ArgumentException("圆弧总角度已超出360°");
            }

            return new Arc2D(_transform.Origin, _radius, startAngle, endAngle, _transform.BasisX, _transform.BasisY);
        }

        public override BoundedCurve2D CreateTransformed(Transform2D transform)
        {
            var trans = transform.Multiply(this._transform);

            var arc = new Arc2D(trans.Origin, _radius, _startAngle, _endAngle, trans.BasisX, trans.BasisY);
            return arc;
        }

        public override BoundedCurve2D CreateOffset(double offsetDist)
        {
            var radius = _radius + offsetDist;
            var arc = new Arc2D(_transform.Origin, radius, _startAngle, _endAngle, _transform.BasisX, _transform.BasisY);
            return arc;
        }

        public override BoundedCurve2D CreateReversed()
        {
            var startPoint = GetPoint(1);
            var midPoint = GetPoint(0.5);
            var endPoint = GetPoint(0);
            return new Arc2D(startPoint, endPoint, midPoint);
        }
    }
}