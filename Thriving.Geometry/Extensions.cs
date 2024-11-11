namespace Thriving.Geometry
{
    public static class Extensions
    {
        public static bool IsContinuity(this IList<Segment3D> segments)
        {
            var result = true;
            for (int i = 0; i < segments.Count - 1; i++)
            {
                var current = segments.ElementAt(i);
                var next = segments.ElementAt(i + 1);

                if (!current.EndPoint.IsAlmostEqualTo(next.StartPoint))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        public static IList<Segment3D> CreateOffset(this IList<Segment3D> segments, double offset, Vector3D normal)
        {
            if (!segments.IsContinuity())
            {
                throw new ArgumentException("线段不连续，不能创建集体偏移");
            }
            IList<Segment3D> result = new List<Segment3D>();
            Point3D? midPoint = null;
            for (int i = 0; i < segments.Count - 1; i++)
            {
                var current = segments.ElementAt(i);
                var next = segments.ElementAt(i + 1);

                var dir = current.Direction.CrossProduct(normal).Normalize();

                Point3D startPoint = i == 0 ? current.StartPoint.Add(offset * dir) : midPoint.Value;
                Point3D endPoint;

                var mdir = (next.Direction + current.Direction.Negate()).Normalize();
                var angle = 0.5 * current.Direction.AngleTo(next.Direction);
                var length = offset / Math.Cos(angle);
                if (mdir.DotProduct(dir) > 0)
                {
                    endPoint = current.EndPoint.Add(length * mdir);
                }
                else
                {
                    endPoint = current.EndPoint.Subtract(length * mdir);
                }
                result.Add(new Segment3D(startPoint, endPoint));
                midPoint = endPoint;
            }

            var last = segments.Last();
            var ldir = last.Direction.CrossProduct(normal).Normalize();
            result.Add(new Segment3D(midPoint.Value, last.EndPoint.Add(offset * ldir)));

            return result;
        }
    }
}