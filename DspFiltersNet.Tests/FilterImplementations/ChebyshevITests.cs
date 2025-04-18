using DspFiltersNet.Filter;
using DspFiltersNet.FilterImplementations;
using System.Numerics;

namespace DspFiltersNet.Tests.FilterImplementations;

[TestFixture]
internal class ChebyshevITests
{
    [Test]
    public void Chebyshev_Type1_TestPoles_FilterOrder4()
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
    public void Chebyshev_Type1_TestPoles_FilterOrder2()
    {
        double referenceGain = 0.566763970129022;

        Complex[] referencePoles = new Complex[]
        {
            new Complex(-0.357625433341244, +0.792398858260480),
            new Complex(-0.357625433341244, -0.792398858260480)
        };

        var calculatedPrototype = ChebyshevI.PrototypeAnalogLowPass(2, 2.5);

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
    public void Chebyshev_Type1_TestPoles_FilterOrder1()
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

            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 2, 3.0,
                new Zpk([new(1.0, 0.0), new(1.0, 0.0)],
                    [new(0.969498159675381, 0.066985978120821), new(0.969498159675381, -0.066985978120821)],
                    0.687310956266769)).SetName("(Zpk Test) HighPass: 2nd Order, 10Hz with 1000Hz Sampling, RippleDb 3.0");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 666, 0, 42.42, 3, 2.0,
                new Zpk([new(1.0, 0.0), new(1.0, 0.0), new(1.0, 0.0)],
                    [new(0.290519295211036, 0.0), new(0.843234138543278, 0.373659958313811), new(0.843234138543278, -0.373659958313811)],
                    0.570592436031469)).SetName("(Zpk Test) HighPass: 3rd Order, 42.42Hz with 666Hz Sampling, RippleDb 2.0");
            
            yield return new TestCaseData(FrequencyFilterType.BandPass, 1000, 0.1, 10, 2, 3.0,
                new Zpk([new(1.0, 0.0), new(1.0, 0.0), new(-1.0, 0.0), new(-1.0, 0.0)],
                    [new(0.979241644484636, 0.048065883137934), new(0.979241644484636, -0.048065883137934)
                    ,new(0.999720491625882, 0.000692263490469), new(0.999720491625882, -0.000692263490469)],
                    4.752612693205063e-04)).SetName("(Zpk Test) BandPass: 2nd Order, Low: 0.1Hz, High: 10Hz with 1000Hz Sampling, RippleDb 3.0");
            yield return new TestCaseData(FrequencyFilterType.BandPass, 666, 25.81, 42.42, 3, 2.0,
                new Zpk([new(1.0, 0.0), new(1.0, 0.0), new(1.0, 0.0), new(-1.0, 0.0), new(-1.0, 0.0), new(-1.0, 0.0)],
                    [new(0.907856350008236, 0.375889947504639), new(0.907856350008236, -0.375889947504639)
                    ,new(0.924786347132471, 0.297444202924584), new(0.924786347132471, -0.297444202924584)
                    ,new(0.958594165097478, 0.242554568944367), new(0.958594165097478, -0.242554568944367)],
                    1.486305119776800e-04)).SetName("(Zpk Test) BandPass: 3rd Order, Low: 25.81Hz, High: 42.42Hz with 666Hz Sampling, RippleDb 2.0");

            yield return new TestCaseData(FrequencyFilterType.BandStop, 1000, 0.1, 10, 2, 3.0,
                new Zpk(
                    [new(0.999980254488989, 0.006284157233677), new(0.999980254488989, -0.006284157233677)
                    ,new(0.999980254488989, 0.006284157233677), new(0.999980254488989, -0.006284157233677)],

                    [new(0.969983341078402, 0.066826256450609), new(0.969983341078402, -0.066826256450609)
                    ,new(0.999798766391683, 0.000491644504088), new(0.999798766391683, -0.000491644504088)],
                    0.687520705477987)).SetName("(Zpk Test) BandStop: 2nd Order, Low: 0.1Hz, High: 10Hz with 1000Hz Sampling, RippleDb 3.0");
            yield return new TestCaseData(FrequencyFilterType.BandStop, 666, 25.81, 42.42, 3, 2.0,
                new Zpk(
                    [new(0.951571739562319, -0.307426779032574), new(0.951571739562319, 0.307426779032574)
                    ,new(0.951571739562319, -0.307426779032574), new(0.951571739562319, 0.307426779032574)
                    ,new(0.951571739562319, -0.307426779032574), new(0.951571739562319, 0.307426779032574)],

                    [new(0.901137084173862, 0.385136433106849), new(0.901137084173862, -0.385136433106849)
                    ,new(0.784593905871373, 0.182922819298543), new(0.784593905871373, -0.182922819298543)
                    ,new(0.959353982397505, 0.235416396951823), new(0.959353982397505, -0.235416396951823)],
                    0.793083509829668)).SetName("(Zpk Test) BandStop: 3rd Order, Low: 25.81Hz, High: 42.42Hz with 666Hz Sampling, RippleDb 2.0");
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
