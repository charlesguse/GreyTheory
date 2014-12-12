using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GreyTheory.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var data = new[] { 256.3, 291.6, 255, 279.2, 284.3, 250, 280.9, 346.5};
            var expected = new [] {256.3, 281.939, 264.068, 280.132, 264.074, 236.932};


            var gt = new GreyTheory(data);
            var actual = gt.Compute();

            //Assert.AreEqual(expected.Length, actual.Count);
            for (int i = 0; i < expected.Length; i++)
            {
                Console.WriteLine("{0:##.000}\t{1:##.000}", expected[i], Math.Round(actual[i], 3));

                //Assert.AreEqual(expected[i], Math.Round(actual[i], 3));
            }
        }
    }
}
