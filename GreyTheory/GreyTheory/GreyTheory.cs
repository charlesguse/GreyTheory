using System;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace GreyTheory
{
    public class GreyTheory
    {
        private readonly double[] _observationData;
        private readonly Vector<double> _ab;

        public GreyTheory(double[] observationData)
        {
            _observationData = observationData;
            _ab = Compute_ab();
        }

        public double[] Compute(int length)
        {
            var eago = EstimateAGO(_ab, length + 1);
            var iago = EstimateIAGO(eago);

            return iago.ToArray();
        }

        public double ResidualError(int length)
        {
            var f = FSkipFirst().ToArray();
            return ResidualError(length, f);
        }

        public double ResidualError(int length, double[] comparisonData)
        {
            double sum = 0;
            var iago = Compute(length);

            var n = (new[] { length, iago.Length, comparisonData.Length }).Min();

            for (var i = 0; i < n; i++)
            {
                sum += Math.Abs(comparisonData[i] - iago[i]) / comparisonData[i];
            }

            return sum / length;
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

        private Vector<double> Compute_B_Row(int t)
        {
            return Vector<double>.Build.DenseOfArray(new[] {
                 Cell(t), 1
            });
        }

        private double Cell(int t)
        {
            return -.5 * (F1(t) + F1(t - 1));
        }

        private Matrix<double> ComputeB()
        {
            // ReSharper disable once InconsistentNaming
            var B = Matrix<double>.Build.Dense(_observationData.Length - 1, 2);
            for (int i = 1; i < _observationData.Length; i++)
            {
                var row = Compute_B_Row(i);
                B[i - 1, 0] = row[0];
                B[i - 1, 1] = row[1];
            }

            return B;
        }

        private Vector<double> Compute_ab()
        {
            // ReSharper disable InconsistentNaming
            var B = ComputeB();
            var Bt = B.Transpose();
            var F = FSkipFirst();
            // ReSharper restore InconsistentNaming

            return (Bt * B).Inverse() * Bt * F;
        }

        // ReSharper disable once InconsistentNaming
        private Vector<double> EstimateAGO(Vector<double> ab, int n)
        {
            var a = ab[0];
            var b = ab[1];

            var eago = Vector<double>.Build.Dense(n);
            for (int i = 0; i < n; i++)
            {
                eago[i] = (F0(0) - b / a) * Math.Exp(-a * i) + b / a;
            }

            return eago;
        }

        // ReSharper disable once InconsistentNaming
        private Vector<double> EstimateIAGO(Vector<double> eago)
        {
            var n = eago.Count - 1;
            var iago = Vector<double>.Build.Dense(n);

            for (int i = 1; i < eago.Count; i++)
            {
                iago[i - 1] = eago[i] - eago[i - 1];
            }

            return iago;
        }
    }
}
