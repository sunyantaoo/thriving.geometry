using System.Collections;
using System.Collections.ObjectModel;

namespace Thriving.Geometry
{
    /// <summary>
    /// 二维连续曲线
    /// </summary>
    public class CurvePath2D : IEnumerable<BoundedCurve2D>
    {
        private readonly ICollection<BoundedCurve2D> _curves;

        public CurvePath2D()
        {
            _curves = new Collection<BoundedCurve2D>();
        }

        public CurvePath2D(IEnumerable<BoundedCurve2D> curve2Ds)
        {
            _curves = new Collection<BoundedCurve2D>();
            foreach (var item in curve2Ds)
            {
                Append(item);
            }
        }

        public void Append(BoundedCurve2D curve2D)
        {
            if (_curves.Any())
            {
                var last = _curves.Last();
                if (last.EndPoint.IsAlmostEqualTo(curve2D.StartPoint))
                {
                    _curves.Add(curve2D);
                }
                else
                {
                    throw new InvalidOperationException("曲线不连续，无法追加到曲线路径");
                }
            }
            else
            {
                _curves.Add(curve2D);
            }
        }

        /// <summary>
        /// 是否闭合
        /// </summary>
        /// <returns></returns>
        public bool IsClosed()
        {
            if (_curves.Count <= 1)
            {
                return false;
            }
            var first = _curves.First();
            var last = _curves.Last();
            return first.StartPoint.IsAlmostEqualTo(last.EndPoint);
        }

        public CurvePath2D CreateTransform(Transform2D transform)
        {
            var newCurves = new Collection<BoundedCurve2D>();
            foreach (var item in _curves)
            {
                var transCurve = item.CreateTransformed(transform);
                newCurves.Add(transCurve);
            }
            return new CurvePath2D(newCurves);
        }

        IEnumerator<BoundedCurve2D> IEnumerable<BoundedCurve2D>.GetEnumerator()
        {
            return _curves.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _curves.GetEnumerator();
        }
    }

}