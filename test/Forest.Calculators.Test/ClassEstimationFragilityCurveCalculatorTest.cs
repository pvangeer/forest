using System;
using Forest.Data;
using Forest.Data.Estimations;
using Forest.Data.Hydrodynamics;
using Forest.Data.Tree;
using NUnit.Framework;

namespace Forest.Calculators.Test
{
    [TestFixture]
    public class ClassEstimationFragilityCurveCalculatorTest
    {
        [Test]
        public void LogTest()
        {
            Assert.AreEqual(7.118, Math.Log(1234), 0.0001);
        }

        [Test]
        public void CalculateCombinedProbabilityFragilityCurveTest()
        {
            var hydraulicConditions = new[]
            {
                new HydrodynamicCondition(2.3, (Probability)3.33E-02, double.NaN, double.NaN),
                new HydrodynamicCondition(2.6, (Probability)1.00E-02, double.NaN, double.NaN),
                new HydrodynamicCondition(2.9, (Probability)3.33E-03, double.NaN, double.NaN),
                new HydrodynamicCondition(3.2, (Probability)1.00E-03, double.NaN, double.NaN),
                new HydrodynamicCondition(3.5, (Probability)3.33E-04, double.NaN, double.NaN),
                new HydrodynamicCondition(3.8, (Probability)1.00E-04, double.NaN, double.NaN)
            };

            var criticalPathElements = new[]
            {
                new CriticalPathElement(new TreeEvent(""),
                    new FragilityCurve
                    {
                        new FragilityCurveElement(2.3, (Probability)0.001),
                        new FragilityCurveElement(2.6, (Probability)0.001),
                        new FragilityCurveElement(2.9, (Probability)0.001),
                        new FragilityCurveElement(3.2, (Probability)0.0028),
                        new FragilityCurveElement(3.5, (Probability)0.0226),
                        new FragilityCurveElement(3.8, (Probability)0.0406)
                    }, true)
            };

            var interpolatedValues =
                ClassEstimationFragilityCurveCalculator.CalculateCombinedProbabilityFragilityCurve(hydraulicConditions,
                    criticalPathElements);

            Assert.AreEqual(6, interpolatedValues.Count);
            Assert.AreEqual(2.33E-05, interpolatedValues[0].Probability, 1e-4);
            Assert.AreEqual(6.67E-06, interpolatedValues[1].Probability, 1e-4);
            Assert.AreEqual(4.02E-06, interpolatedValues[2].Probability, 1e-4);
            Assert.AreEqual(7.28E-06, interpolatedValues[3].Probability, 1e-4);
            Assert.AreEqual(6.96E-06, interpolatedValues[4].Probability, 1e-4);
            Assert.AreEqual(1.00E-04, interpolatedValues[5].Probability, 1e-4);
        }

        [Test]
        public void CalculateProbabilityWorks()
        {
            var hydraulicConditions = new[]
            {
                new HydrodynamicCondition(2.3, (Probability)3.33E-02, double.NaN, double.NaN),
                new HydrodynamicCondition(2.6, (Probability)1.00E-02, double.NaN, double.NaN),
                new HydrodynamicCondition(2.9, (Probability)3.33E-03, double.NaN, double.NaN),
                new HydrodynamicCondition(3.2, (Probability)1.00E-03, double.NaN, double.NaN),
                new HydrodynamicCondition(3.5, (Probability)3.33E-04, double.NaN, double.NaN),
                new HydrodynamicCondition(3.8, (Probability)1.00E-04, double.NaN, double.NaN)
            };

            var criticalPathElements = new[]
            {
                new CriticalPathElement(new TreeEvent(""),
                    new FragilityCurve
                    {
                        new FragilityCurveElement(2.3, (Probability)0.001),
                        new FragilityCurveElement(2.6, (Probability)0.001),
                        new FragilityCurveElement(2.9, (Probability)0.001),
                        new FragilityCurveElement(3.2, (Probability)0.0028),
                        new FragilityCurveElement(3.5, (Probability)0.0226),
                        new FragilityCurveElement(3.8, (Probability)0.0406)
                    }, true)
            };

            var probability =
                ClassEstimationFragilityCurveCalculator.CalculateProbability(hydraulicConditions, criticalPathElements);

            Assert.AreEqual(1.48e-4, probability, 1e-6);
        }

        [Test]
        public void CalculateProbabilityWorksWithPassingElement()
        {
            var hydraulicConditions = new[]
            {
                new HydrodynamicCondition(2.3, (Probability)3.33E-02, double.NaN, double.NaN),
                new HydrodynamicCondition(2.6, (Probability)1.00E-02, double.NaN, double.NaN),
                new HydrodynamicCondition(2.9, (Probability)3.33E-03, double.NaN, double.NaN),
                new HydrodynamicCondition(3.2, (Probability)1.00E-03, double.NaN, double.NaN),
                new HydrodynamicCondition(3.5, (Probability)3.33E-04, double.NaN, double.NaN),
                new HydrodynamicCondition(3.8, (Probability)1.00E-04, double.NaN, double.NaN)
            };

            var criticalPathElements = new[]
            {
                new CriticalPathElement(new TreeEvent(""),
                    new FragilityCurve
                    {
                        new FragilityCurveElement(2.3, (Probability)0.001),
                        new FragilityCurveElement(2.6, (Probability)0.001),
                        new FragilityCurveElement(2.9, (Probability)0.001),
                        new FragilityCurveElement(3.2, (Probability)0.0028),
                        new FragilityCurveElement(3.5, (Probability)0.0226),
                        new FragilityCurveElement(3.8, (Probability)0.0406)
                    }, false)
            };

            var probability =
                ClassEstimationFragilityCurveCalculator.CalculateProbability(hydraulicConditions, criticalPathElements);

            Assert.AreEqual(0.0332517, probability, 1e-6);
        }
    }
}