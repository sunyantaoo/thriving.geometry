namespace Thriving.Geometry.Test
{
    public class CommonTest
    {
        /// <summary>
        /// 点在三角形内部，点与三个顶点形成的内角和为360度
        /// </summary>
        [Fact]
        public void TestMethod1()
        {
            var v1 = new Point3D(-1, 0, 0);
            var v2 = new Point3D(1, 0, 0);
            var v3 = new Point3D(0, 1, 0);

            var p = new Point3D(0.5, 0.2, 0);

            var va =new Vector3D(p, v1  );
            var vb = new Vector3D(p,v2) ;
            var vc = new Vector3D(p,v3);

            var a1 = va.AngleTo(vb);
            var a2 = vb.AngleTo(vc);
            var a3 = vc.AngleTo(va);

            var angle = a1 + a2 + a3;
            Assert.True(angle == 2 * Math.PI);
        }

        /// <summary>
        ///  行列式值为0，则4个点共面
        /// </summary>
        [Fact]
        public void TestMethod2()
        {
            var v1 = new Point3D(-1, 0, 0);
            var v2 = new Point3D(1, 0, 0);
            var v3 = new Point3D(0, 1, 0);

            var p = new Point3D(2, 0.2, 0);

            var va = new Vector3D(v1, v2);
            var vb = new Vector3D(v1, v3);
            var vc = new Vector3D(v1, p);

            var mat = new Matrix(new double[,]
            {
                { va.X,vb.X,vc.X },
                { va.Y,vb.Y,vc.Y },
                { va.Z,vb.Z,vc.Z }
            });

            Assert.True(mat.DetValue == 0);
        }


        [Fact]
        public void TestMethod3()
        {
            var v1 = new Point3D(1, 0, 0);
            var v2 = new Point3D(0, 0, 1);
            var v3 = new Point3D(-1, 0, 0);

            var p = new Point3D(0, 2, 0.2);

            var triangle=new Triangle3D(v1,v2,v3);
          
            var result = triangle.Plane().Intersect(p, Vector3D.BasisY);

            var va = new Vector3D(result.Value, v1);
            var vb =new Vector3D(result.Value,v2)  ;
            var vc = new Vector3D(result.Value,v3) ;
 
            var mat = new Matrix(new double[,]
            {
                { va.X,vb.X,vc.X },
                { va.Y,vb.Y,vc.Y },
                { va.Z,vb.Z,vc.Z }
            });

            Assert.True(mat.DetValue == 0);

            var a1 = va.AngleTo(vb);
            var a2 = vb.AngleTo(vc);
            var a3 = vc.AngleTo(va);

            var angle = a1 + a2 + a3;
            Assert.True(angle == 2 * Math.PI);
        }

    }
}
