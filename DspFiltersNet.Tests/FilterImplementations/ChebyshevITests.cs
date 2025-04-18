using DspFiltersNet.Filter;
using DspFiltersNet.FilterImplementations;
using System.Numerics;

namespace DspFiltersNet.Tests.FilterImplementations;

[TestFixture]
internal class ChebyshevITests
{
    [Test]
    public void Chebyshev_Type1_TestPoles()
    {
        double referenceGain = 0.031324290406487;

        Complex[] referencePoles = new Complex[]
        {
            new Complex(-0.038229512502700, +0.976406016979622),
            new Complex(-0.104444970505967, +0.714778813245043),
            new Complex(-0.142674483008667, +0.261627203734579),
            new Complex(-0.142674483008667, -0.261627203734579),
            new Complex(-0.104444970505967, -0.714778813245043),
            new Complex(-0.038229512502700, -0.976406016979622)
        };

        var calculatedPrototype = ChebyshevI.PrototypeAnalogLowPass(6, 3);

        // Sort both by real part (then imag part) to align them
        var sortedExpected = referencePoles.OrderBy(c => c.Real).ThenBy(c => c.Imaginary).ToArray();
        var sortedActual = calculatedPrototype.poles.OrderBy(c => c.Real).ThenBy(c => c.Imaginary).ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(calculatedPrototype.gain, Is.EqualTo(referenceGain).Within(1e-8), "Gain mismatch");
            Assert.That(calculatedPrototype.poles, Has.Count.EqualTo(referencePoles.Length), "Pole count mismatch.");

            for (int i = 0; i < sortedExpected.Length; i++)
            {
                var expected = sortedExpected[i];
                var actual = sortedActual[i];

                Assert.That(actual.Real, Is.EqualTo(expected.Real).Within(1e-8),
                    $"Mismatch at index {i} (Real part)");
                Assert.That(actual.Imaginary, Is.EqualTo(expected.Imaginary).Within(1e-8),
                    $"Mismatch at index {i} (Imaginary part)");
            }
        });
    }

    [Test]
    public void Chebyshev_Type1_TestPolesWithFilterOrder1()
    {
        double referenceGain = 1.002377293007601;

        Complex[] referencePoles = new Complex[]
        {
            new Complex(-1.002377293007601, 0.0)
        };

        var calculatedPrototype = ChebyshevI.PrototypeAnalogLowPass(1, 3);

        Assert.Multiple(() =>
        {
            Assert.That(calculatedPrototype.gain, Is.EqualTo(referenceGain).Within(1e-8), "Gain mismatch");
            Assert.That(calculatedPrototype.poles, Has.Count.EqualTo(referencePoles.Length), "Pole count mismatch.");

            Assert.That(calculatedPrototype.poles.First().Real, Is.EqualTo(referencePoles.First().Real).Within(1e-8));
            Assert.That(calculatedPrototype.poles.First().Imaginary, Is.EqualTo(referencePoles.First().Imaginary).Within(1e-8));
        });
    }


    public static IEnumerable<TestCaseData> ZpkTestDataChebyshevI
    {
        get
        {
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 1, 1.0,
                new Zpk([new(-1.0, 0.0)],
                    [new(0.883665323160146, 0.0)],
                    0.058167338419927)).SetName("(Zpk Test) LowPass: 1st Order, 10Hz with 1000Hz Sampling, RippleDb 1.0");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 2, 3.0,
                new Zpk([new(-1.0, 0.0), new(-1.0, 0.0)],
                    [new(0.837026686361742, 0.271775568163893), new(0.837026686361742, -0.271775568163893)],
                    0.017773378978727)).SetName("(Zpk Test) LowPass: 2nd Order, 42.42Hz with 666Hz Sampling, RippleDb 3.0");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 3, 3.0,
                new Zpk([new(-1.0, 0.0), new(-1.0, 0.0), new(-1.0, 0.0)],
                    [new(0.881652287058507, 0.334780313593118), new(0.881652287058507, -0.334780313593118), new(0.885788553511604, + 0.0)],
                    0.001800030066699)).SetName("(Zpk Test) LowPass: 3rd Order, 42.42Hz with 666Hz Sampling, RippleDb 3.0");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 4, 3.0,
                new Zpk([new(-1.0, 0.0), new(-1.0, 0.0), new(-1.0, 0.0), new(-1.0, 0.0)],
                    [new(0.986861528862660, 0.024322087935234), new(0.986861528862660, -0.024322087935234), 
                     new(0.992907323647874, 0.059119736142994), new(0.992907323647874, -0.059119736142994)],
                    1.198805930863082e-07)).SetName("(Zpk Test) LowPass: 4th Order, 10Hz with 1000Hz Sampling, RippleDb 3.0");
        }
    }


    [TestCaseSource(nameof(ZpkTestDataChebyshevI))]
    public void CalcZpkTestsChebyshevI(FrequencyFilterType frequencyFilterType, double sourceFrequency, double lowCutOff, double highCutoff, int order, double rippleDb, Zpk expectedZpk)
    {
        //Arrange + Act
        var zpk = ChebyshevI.CalcZpk(frequencyFilterType, sourceFrequency, lowCutOff, highCutoff, order, rippleDb);

        //Assert
        TestHelper.CompareZpk(zpk, expectedZpk);
    }
}
