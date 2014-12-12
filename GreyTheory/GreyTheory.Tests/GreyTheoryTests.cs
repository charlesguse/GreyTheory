using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GreyTheory.Tests
{
    [TestClass]
    public class GreyTheoryTests
    {
        [TestMethod]
        public void GaapOriginalData()
        {
            var data = new[] { 256.3, 291.6, 255, 279.2 };
            var expected = new[] { 281.939, 264.068, 280.132, 264.074, 236.932 };


            var gt = new GreyTheory(data);
            var actual = gt.Compute(expected.Length);
            var residualError = gt.ResidualError(expected.Length);

            //Assert.AreEqual(expected.Length, actual.Count);
            for (int i = 0; i < actual.Length; i++)
            {
                Console.WriteLine("{0:##.000}\t{1:##.000}", expected[i], actual[i]);
                //Assert.AreEqual(expected[i], Math.Round(actual[i], 3));
            }
            Console.WriteLine("Residual Error: {0:##.000}", residualError);
        }

        [TestMethod]
        public void Table3_UseAllInputData()
        {
            var inputData = new double[] { 83, 85, 82, 73, 83, 82, 69, 81 };
            var expected = new[] { 82.315, 78.617, 76.919, 83.232, 78.571, 76.945, 81.368 };


            var gt = new GreyTheory(inputData);
            var actual = gt.Compute(expected.Length);
            var residualError = gt.ResidualError(expected.Length);

            for (int i = 0; i < actual.Length; i++)
            {
                Console.WriteLine("{0:##.000}\t{1:##.000}", expected[i], Math.Round(actual[i], 3));
                //Assert.AreEqual(expected[i], Math.Round(actual[i], 3));
            }
            Console.WriteLine("Residual Error: {0:##.000}", residualError);
        }

        [TestMethod]
        public void Table3_Use4Inputs_CompareToAllData()
        {
            var inputData = new double[] { 83, 85, 82, 73 };
            var comparisonData = new double[] { 85, 82, 73, 83, 82, 69, 81 };
            var expected = new[] { 82.315, 78.617, 76.919, 83.232, 78.571, 76.945, 81.368 };


            var gt = new GreyTheory(inputData);
            var actual = gt.Compute(expected.Length);
            var residualError = gt.ResidualError(expected.Length, comparisonData);

            for (int i = 0; i < actual.Length; i++)
            {
                Console.WriteLine("{0:##.000}\t{1:##.000}", expected[i], Math.Round(actual[i], 3));
                //Assert.AreEqual(expected[i], Math.Round(actual[i], 3));
            }
            Console.WriteLine("Residual Error: {0:##.000}", residualError);
        }
    }
}
