using DspFiltersNet.FilterImplementations;
using System.Numerics;

namespace DspFiltersNet.Tests.FilterImplementations;

[TestFixture]
internal class ChebyshevIITests
{
    public static IEnumerable<TestCaseData> LowPassPrototype_TestDataChebyshevII
    {
        get
        {
            yield return new TestCaseData(2, 1.5,
                new Zpk(
                    [new Complex(0.000000000000000, +1.414213562373095)
                    ,new Complex(0.000000000000000, -1.414213562373095)],

                    [new Complex(-0.365307209429348, -1.244725241179487)
                    ,new Complex(-0.365307209429348, +1.244725241179487)],

                    0.841395141645195)).SetName("LowPassPrototype Test: Order 1; StopBandRippleDb 1.5");

            yield return new TestCaseData(1, 0.5,
                new Zpk(
                    [],

                    [new Complex(-2.862775161243190, 0.0)],

                    2.862775161243190)).SetName("LowPassPrototype Test: Order 2; StopBandRippleDb 0.5");

            yield return new TestCaseData(3, 1.5,
                new Zpk(
                    [new Complex(0.000000000000000, +1.154700538379252)
                    ,new Complex(0.000000000000000, -1.154700538379252)],

                    [new Complex(-0.128257008136217, -1.116902145128026)
                    ,new Complex(-4.927295125211721, +0.000000000000000)
                    ,new Complex(-0.128257008136217, +1.116902145128026)],

                    4.670781108939289)).SetName("LowPassPrototype Test: Order 3; StopBandRippleDb 1.5");

            yield return new TestCaseData(6, 0.7,
                new Zpk(
                    [new Complex(0.000000000000000, +1.035276180410083)
                    ,new Complex(0.000000000000000, -1.035276180410083)
                    ,new Complex(0.000000000000000, +1.414213562373095)
                    ,new Complex(0.000000000000000, -1.414213562373095)
                    ,new Complex(0.000000000000000, +3.863703305156274)
                    ,new Complex(0.000000000000000, -3.863703305156274)],

                    [new Complex(-0.018733783487269, -1.032560327368543)
                    ,new Complex(-0.018733783487269, +1.032560327368543)
                    ,new Complex(-0.915646223813966, -3.623452326612253)
                    ,new Complex(-0.915646223813966, +3.623452326612253)
                    ,new Complex(-0.095101676759954, -1.404528527836549)
                    ,new Complex(-0.095101676759954, +1.404528527836549)],

                    0.922571427154765)).SetName("LowPassPrototype Test: Order 6; StopBandRippleDb 0.7");
        }
    }

    [TestCaseSource(nameof(LowPassPrototype_TestDataChebyshevII))]
    public void Chebyshev_Type2_TestPoles_FilterOrder2(int filterOrder, double stopbandRippleDb, Zpk expectedZpk)
    {
        var calculatedPrototype = ChebyshevII.PrototypeAnalogLowPass(filterOrder, stopbandRippleDb);
        var actualZpK = new Zpk(calculatedPrototype.zeros.ToArray(), calculatedPrototype.poles.ToArray(), calculatedPrototype.gain);

        TestHelper.CompareZpk(actualZpK, expectedZpk);
    }
}
