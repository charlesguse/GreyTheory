using System;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace GreyTheory
{
    public class GreyTheory
    {
        private readonly double[] _observationData;

        public GreyTheory(double[] observationData)
        {
            _observationData = observationData;
        }

        public Vector<double> Compute()
        {
            var ab = Compute_ab();
            var eago = EstimateAGO(ab);
            var iago = EstimateIAGO(eago);

            return iago;
        }

        private Vector<double> FSkipFirst()
        {
            return Vector<double>.Build.DenseOfEnumerable(_observationData.Skip(1));
        }

        private double F0(int t)
        {
            return _observationData[t];
        }

        private double F1(int i)
        {
            return _observationData.Take(i).Sum();
        }

        private Vector<double> Compute_b_Row(int t)
        {
            return Vector<double>.Build.DenseOfArray(new[] {
                 Cell(t), 1
            });
        }

        //private Vector<double> Compute_ab_Row(int t)
        //{
        //    var A = Matrix<double>.Build.DenseOfArray(new[,] {
        //        { Cell(t), 1 },
        //        { Cell(t + 1), 1 },
        //    });

        //    var G = Vector<double>.Build.Dense(new[] { F0(t), F0(t+1) });

        //    return A.Solve(G);
        //}

        private double Cell(int t)
        {
            return -.5 * (F1(t) + F1(t - 1));
        }

        private Matrix<double> ComputeB()
        {
            var B = Matrix<double>.Build.Dense(_observationData.Length - 1, 2);
            for (int i = 1; i < _observationData.Length; i++)
            {
                var row = Compute_b_Row(i);
                B[i - 1, 0] = row[0];
                B[i - 1, 1] = row[1];
            }

            return B;
        }

        private Vector<double> Compute_ab()
        {
            var B = ComputeB();
            var Bt = B.Transpose();
            var F = FSkipFirst();

            return (Bt*B).Inverse()*Bt*F;
        }

        private Vector<double> EstimateIAGO(Vector<double> eago)
        {
            var n = _observationData.Length;
            var iago = Vector<double>.Build.Dense(n);

            iago[0] = F0(0);
            for (int i = 1; i < n - 1; i++)
            {
                iago[i] = eago[i] - eago[i - 1];
            }

            return iago;
        }

        private Vector<double> EstimateAGO(Vector<double> ab)
        {
            //var ab = Compute_ab();
            var a = ab[0];
            var b = ab[1];
            var n = _observationData.Length;

            //return (F0(0) - b/a) * Math.Exp(-a * ) 
            var eago = Vector<double>.Build.Dense(n - 1);
            for (int i = 0; i < _observationData.Length - 1; i++)
            {
                eago[i] = (F0(0) - b/a)*Math.Exp(-a*i) + b/a;
                //eago.Append(Compute_ab_Row(i).ToRowMatrix());
            }

            return eago;
        }
    }
}
