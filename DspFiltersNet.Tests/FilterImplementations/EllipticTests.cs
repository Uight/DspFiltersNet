using DspFiltersNet.FilterImplementations;
using System.Numerics;

namespace DspFiltersNet.Tests.FilterImplementations;

[TestFixture]
internal class EllipticTests
{
    public static IEnumerable<TestCaseData> LowPassPrototype_Elliptic
    {
        get
        {
            yield return new TestCaseData(1, 0.2, 2.5,
                new Zpk(
                    [],

                    [new Complex(-4.606360993381418, 0.0)],

                    4.606360993381418)).SetName("LowPassPrototype Test: Order 1; RippleDb 0,2; StopBandRippleDb 2,5");

            yield return new TestCaseData(2, 2.0, 3.0,
                new Zpk(
                    [new Complex(0.0, -1.073419580096317)
                    ,new Complex(0.0, +1.073419580096317)],

                    [new Complex(-0.066488061528181, -1.011189915448505)
                    ,new Complex(-0.066488061528181, +1.011189915448505)],

                    0.707945784384138)).SetName("LowPassPrototype Test: Order 2; RippleDb 2,0; StopBandRippleDb 3,0");

            yield return new TestCaseData(3, 0.7, 1.2,
                new Zpk(
                    [new Complex(0.000000000000000, +1.003006296494213)
                    ,new Complex(0.000000000000000, -1.003006296494213)],

                    [new Complex(-0.002198430827007, -1.001928796741542)
                    ,new Complex(-0.002198430827007, +1.001928796741542)
                    ,new Complex(-2.052132140744201, 0.0)],

                    2.047735279090187)).SetName("LowPassPrototype Test: Order 3; RippleDb 0,7; StopBandRippleDb 1,2");

            yield return new TestCaseData(4, 3.0, 4.5,
                new Zpk(
                    [new Complex(0.000000000000000, -1.078141383168042)
                    ,new Complex(0.000000000000000, +1.078141383168042)
                    ,new Complex(0.000000000000000, -1.000114443852390)
                    ,new Complex(0.000000000000000, +1.000114443852390)],

                    [new Complex(-0.073472695747612, -0.986341123760096)
                    ,new Complex(-0.073472695747612, +0.986341123760096)
                    ,new Complex(-0.000104999362920, -0.999992402703697)
                    ,new Complex(-0.000104999362920, +0.999992402703697)],

                    0.595662143529061)).SetName("LowPassPrototype Test: Order 4; RippleDb 3,0; StopBandRippleDb 4,5");
        }
    }

    [TestCaseSource(nameof(LowPassPrototype_Elliptic))]
    public void Chebyshev_Type2_TestPrototype(int filterOrder, double passbandRippleDb, double stopbandAttenuationDb, Zpk expectedZpk)
    {
        var calculatedPrototype = Elliptic.PrototypeAnalogLowPass(filterOrder, passbandRippleDb, stopbandAttenuationDb);

        TestHelper.CompareZpk(calculatedPrototype, expectedZpk, 1e-9);
    }
}
