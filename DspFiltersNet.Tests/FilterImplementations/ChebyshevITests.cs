using DspFiltersNet.Filter;
using DspFiltersNet.FilterImplementations;
using System.Numerics;

namespace DspFiltersNet.Tests.FilterImplementations;

[TestFixture]
internal class ChebyshevITests
{
    public static IEnumerable<TestCaseData> LowPassPrototype_TestDataChebyshevI
    {
        get
        {
            yield return new TestCaseData(6, 3.0,
                new Zpk(
                    [],

                    [new Complex(-0.038229512502700, +0.976406016979622)
                    ,new Complex(-0.104444970505967, +0.714778813245043)
                    ,new Complex(-0.142674483008667, +0.261627203734579)
                    ,new Complex(-0.142674483008667, -0.261627203734579)
                    ,new Complex(-0.104444970505967, -0.714778813245043)
                    ,new Complex(-0.038229512502700, -0.976406016979622)],

                    0.031324290406487)).SetName("LowPassPrototype Test: Order 6; RippleDb 3,0");

            yield return new TestCaseData(2, 2.5,
                new Zpk(
                    [],

                    [new Complex(-0.357625433341244, +0.792398858260480)
                    ,new Complex(-0.357625433341244, -0.792398858260480)],

                    0.566763970129022)).SetName("LowPassPrototype Test: Order 2; RippleDb 2,5");

            yield return new TestCaseData(1, 3.0,
                new Zpk(
                    [],

                    [new Complex(-1.002377293007601, 0.0)],

                    1.002377293007601)).SetName("LowPassPrototype Test: Order 1; RippleDb 3,0");
        }
    }

    [TestCaseSource(nameof(LowPassPrototype_TestDataChebyshevI))]
    public void Chebyshev_Type1_TestPrototype(int filterOrder, double rippleDb, Zpk expectedZpk)
    {
        var calculatedPrototype = ChebyshevI.PrototypeAnalogLowPass(filterOrder, rippleDb);

        TestHelper.CompareZpk(calculatedPrototype, expectedZpk);
    }

    public static IEnumerable<TestCaseData> ZpkTestDataChebyshevI
    {
        get
        {
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 1, 1.0,
                new Zpk([new(-1.0, 0.0)],
                    [new(0.883665323160146, 0.0)],
                    0.058167338419927)).SetName("(Zpk Test) LowPass: 1st Order; 10Hz with 1000Hz Sampling; RippleDb 1,0");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 2, 3.0,
                new Zpk([new(-1.0, 0.0), new(-1.0, 0.0)],
                    [new(0.837026686361742, 0.271775568163893), new(0.837026686361742, -0.271775568163893)],
                    0.017773378978727)).SetName("(Zpk Test) LowPass: 2nd Order; 42,42Hz with 666Hz Sampling; RippleDb 3,0");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 3, 3.0,
                new Zpk([new(-1.0, 0.0), new(-1.0, 0.0), new(-1.0, 0.0)],
                    [new(0.881652287058507, 0.334780313593118), new(0.881652287058507, -0.334780313593118), new(0.885788553511604, + 0.0)],
                    0.001800030066699)).SetName("(Zpk Test) LowPass: 3rd Order; 42,42Hz with 666Hz Sampling; RippleDb 3,0");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 4, 3.0,
                new Zpk([new(-1.0, 0.0), new(-1.0, 0.0), new(-1.0, 0.0), new(-1.0, 0.0)],
                    [new(0.986861528862660, 0.024322087935234), new(0.986861528862660, -0.024322087935234), 
                     new(0.992907323647874, 0.059119736142994), new(0.992907323647874, -0.059119736142994)],
                    1.198805930863082e-07)).SetName("(Zpk Test) LowPass: 4th Order; 10Hz with 1000Hz Sampling; RippleDb 3,0");

            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 2, 3.0,
                new Zpk([new(1.0, 0.0), new(1.0, 0.0)],
                    [new(0.969498159675381, 0.066985978120821), new(0.969498159675381, -0.066985978120821)],
                    0.687310956266769)).SetName("(Zpk Test) HighPass: 2nd Order; 10Hz with 1000Hz Sampling; RippleDb 3,0");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 666, 0, 42.42, 3, 2.0,
                new Zpk([new(1.0, 0.0), new(1.0, 0.0), new(1.0, 0.0)],
                    [new(0.290519295211036, 0.0), new(0.843234138543278, 0.373659958313811), new(0.843234138543278, -0.373659958313811)],
                    0.570592436031469)).SetName("(Zpk Test) HighPass: 3rd Order; 42,42Hz with 666Hz Sampling; RippleDb 2,0");
            
            yield return new TestCaseData(FrequencyFilterType.BandPass, 1000, 0.1, 10, 2, 3.0,
                new Zpk([new(1.0, 0.0), new(1.0, 0.0), new(-1.0, 0.0), new(-1.0, 0.0)],
                    [new(0.979241644484636, 0.048065883137934), new(0.979241644484636, -0.048065883137934)
                    ,new(0.999720491625882, 0.000692263490469), new(0.999720491625882, -0.000692263490469)],
                    4.752612693205063e-04)).SetName("(Zpk Test) BandPass: 2nd Order; Low: 0,1Hz; High: 10Hz with 1000Hz Sampling; RippleDb 3,0");
            yield return new TestCaseData(FrequencyFilterType.BandPass, 666, 25.81, 42.42, 3, 2.0,
                new Zpk([new(1.0, 0.0), new(1.0, 0.0), new(1.0, 0.0), new(-1.0, 0.0), new(-1.0, 0.0), new(-1.0, 0.0)],
                    [new(0.907856350008236, 0.375889947504639), new(0.907856350008236, -0.375889947504639)
                    ,new(0.924786347132471, 0.297444202924584), new(0.924786347132471, -0.297444202924584)
                    ,new(0.958594165097478, 0.242554568944367), new(0.958594165097478, -0.242554568944367)],
                    1.486305119776800e-04)).SetName("(Zpk Test) BandPass: 3rd Order; Low: 25,81Hz; High: 42,42Hz with 666Hz Sampling; RippleDb 2,0");

            yield return new TestCaseData(FrequencyFilterType.BandStop, 1000, 0.1, 10, 2, 3.0,
                new Zpk(
                    [new(0.999980254488989, 0.006284157233677), new(0.999980254488989, -0.006284157233677)
                    ,new(0.999980254488989, 0.006284157233677), new(0.999980254488989, -0.006284157233677)],

                    [new(0.969983341078402, 0.066826256450609), new(0.969983341078402, -0.066826256450609)
                    ,new(0.999798766391683, 0.000491644504088), new(0.999798766391683, -0.000491644504088)],
                    0.687520705477987)).SetName("(Zpk Test) BandStop: 2nd Order; Low: 0,1Hz; High: 10Hz with 1000Hz Sampling; RippleDb 3,0").SetProperty("Precision", 1e-13);
            yield return new TestCaseData(FrequencyFilterType.BandStop, 666, 25.81, 42.42, 3, 2.0,
                new Zpk(
                    [new(0.951571739562319, -0.307426779032574), new(0.951571739562319, 0.307426779032574)
                    ,new(0.951571739562319, -0.307426779032574), new(0.951571739562319, 0.307426779032574)
                    ,new(0.951571739562319, -0.307426779032574), new(0.951571739562319, 0.307426779032574)],

                    [new(0.901137084173862, 0.385136433106849), new(0.901137084173862, -0.385136433106849)
                    ,new(0.784593905871373, 0.182922819298543), new(0.784593905871373, -0.182922819298543)
                    ,new(0.959353982397505, 0.235416396951823), new(0.959353982397505, -0.235416396951823)],
                    0.793083509829668)).SetName("(Zpk Test) BandStop: 3rd Order; Low: 25,81Hz; High: 42,42Hz with 666Hz Sampling; RippleDb 2,0");
        }
    }


    [TestCaseSource(nameof(ZpkTestDataChebyshevI))]
    public void CalcZpkTestsChebyshevI(FrequencyFilterType frequencyFilterType, double sourceFrequency, double lowCutOff, double highCutoff, int order, double rippleDb, Zpk expectedZpk)
    {
        //Arrange + Act
        var zpk = ChebyshevI.CalcZpk(frequencyFilterType, sourceFrequency, lowCutOff, highCutoff, order, rippleDb);

        //Assert
        double comparePrecision = 1e-14;

        // Override if the test case has custom precision
        if (TestContext.CurrentContext.Test.Properties.ContainsKey("Precision"))
        {
            comparePrecision = (double)TestContext.CurrentContext.Test.Properties.Get("Precision")!;
        }

        TestHelper.CompareZpk(zpk, expectedZpk, comparePrecision);
    }
}
