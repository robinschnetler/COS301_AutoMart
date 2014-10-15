using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dot_Slash;

namespace DotSlash_UnitTest
{
    [TestClass]
    public class CoverageDetectorTest
    {
        [TestMethod]
        public void calculateCoverageValue_CorrectCalulation()
        {
            int width = 480;
            int height = 320;
            int carWidth = 48;
            int carHeight = 32;
            double expectedCoverage = 14.6484375;
            CoverageDetector cd = new CoverageDetector();

            double actual = cd.calculateCoverageValue(width, height, carWidth, carHeight);

            Assert.AreEqual(expectedCoverage, actual, "Coverage not calculated correctly.");
        }

        [TestMethod]
        public void calculateCoverageRating_CorrectRatingValue()
        {
            double coverage = 46.88;
            int expectedRating = 1;
            CoverageDetector cd = new CoverageDetector();

            int actual = cd.calculateCoverageRating(coverage);

            Assert.AreEqual(expectedRating, actual);
        }
    }
}
