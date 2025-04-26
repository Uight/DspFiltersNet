using DspFiltersNet.Filter;
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

                    4.606360993381418)).SetName("LowPassPrototype Test: Order 1; PassbandRippleDb 0,2; StopbandAttenuationDb 2,5");

            yield return new TestCaseData(2, 2.0, 3.0,
                new Zpk(
                    [new Complex(0.0, -1.073419580096317)
                    ,new Complex(0.0, +1.073419580096317)],

                    [new Complex(-0.066488061528181, -1.011189915448505)
                    ,new Complex(-0.066488061528181, +1.011189915448505)],

                    0.707945784384138)).SetName("LowPassPrototype Test: Order 2; PassbandRippleDb 2,0; StopbandAttenuationDb 3,0");

            yield return new TestCaseData(3, 0.7, 1.2,
                new Zpk(
                    [new Complex(0.000000000000000, +1.003006296494213)
                    ,new Complex(0.000000000000000, -1.003006296494213)],

                    [new Complex(-0.002198430827007, -1.001928796741542)
                    ,new Complex(-0.002198430827007, +1.001928796741542)
                    ,new Complex(-2.052132140744201, 0.0)],

                    2.047735279090187)).SetName("LowPassPrototype Test: Order 3; PassbandRippleDb 0,7; StopbandAttenuationDb 1,2");

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

                    0.595662143529061)).SetName("LowPassPrototype Test: Order 4; PassbandRippleDb 3,0; StopbandAttenuationDb 4,5");
        }
    }

    [TestCaseSource(nameof(LowPassPrototype_Elliptic))]
    public void Chebyshev_Type2_TestPrototype(int filterOrder, double passbandRippleDb, double stopbandAttenuationDb, Zpk expectedZpk)
    {
        var calculatedPrototype = Elliptic.PrototypeAnalogLowPass(filterOrder, passbandRippleDb, stopbandAttenuationDb);

        TestHelper.CompareZpk(calculatedPrototype, expectedZpk, 1e-9);
    }

    public static IEnumerable<TestCaseData> ZpkTestDataElliptic
    {
        get
        {
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 1, 1.0, 2.0,
                new Zpk([new(-1.0, 0.0)],
                    [new(0.883665323160146, 0.0)],
                    0.058167338419927)).SetName("(Zpk Test) LowPass: 1st Order; 10Hz with 1000Hz Sampling; PassbandRippleDb 1,0; StopbandAttenuationDb 2,0");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 2, 0.3, 3.0,
                new Zpk(
                    [new(0.822673738075543, +0.568513782313861)
                    ,new(0.822673738075543, -0.568513782313861)],

                    [new(0.767949124832565, +0.426581287443820)
                    ,new(0.767949124832565, -0.426581287443820)],
                    0.642356485485927)).SetName("(Zpk Test) LowPass: 2nd Order; 42,42Hz with 666Hz Sampling; PassbandRippleDb 0,3; StopbandAttenuationDb 3,0").SetProperty("Precision", 1e-8);
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 3, 3.0, 4.0,
                new Zpk(
                    [new(0.920765210804798, +0.390117195944497)
                    ,new(0.920765210804798, -0.390117195944497)
                    ,new(-1.0, 0.0)],

                    [new(0.920507572757794, +0.389371789965060)
                    ,new(0.920507572757794, -0.389371789965060)
                    ,new(0.690356010582328, 0.0)],
                    0.154294285871356)).SetName("(Zpk Test) LowPass: 3rd Order; 42,42Hz with 666Hz Sampling; PassbandRippleDb 3,0; StopbandAttenuationDb 4,0");

            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 2, 2.0, 3.0,
                new Zpk(
                    [new(0.998287208700958, +0.058503409678839)
                    ,new(0.998287208700958, -0.058503409678839)],

                    [new(0.994037196430898, +0.061579617471930)
                    ,new(0.994037196430898, -0.061579617471930)],
                    0.791029339120464)).SetName("(Zpk Test) HighPass: 2nd Order; 10Hz with 1000Hz Sampling; PassbandRippleDb 2,0; StopbandAttenuationDb 3,0");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 666, 0, 42.42, 3, 1.0, 2.0,
                new Zpk(
                    [new(0.921842335237326, +0.387565102872012)
                    ,new(0.921842335237326, -0.387565102872012)
                    ,new(1.0, 0.0)],

                    [new(0.919750660750111, +0.387905704216792)
                    ,new(0.919750660750111, -0.387905704216792)
                    ,new(0.774583303940096, 0.0)],
                    0.885497708627143)).SetName("(Zpk Test) HighPass: 3rd Order; 42,42Hz with 666Hz Sampling; PassbandRippleDb 1,0; StopbandAttenuationDb 2,0");

            yield return new TestCaseData(FrequencyFilterType.BandPass, 1000, 0.1, 10, 2, 2.0, 3.0,
                new Zpk(
                    [new(0.997732622106534, +0.067302412916785)
                    ,new(0.997732622106534, -0.067302412916785)
                    ,new(0.999999828237342, +0.000586110301806)
                    ,new(0.999999828237342, -0.000586110301806)],

                    [new(0.993906861003125, +0.063216365274762)
                    ,new(0.993906861003125, -0.063216365274762)
                    ,new(0.999959896966510, +0.000618927423175)
                    ,new(0.999959896966510, -0.000618927423175)],
                    0.705117301178397)).SetName("(Zpk Test) BandPass: 2nd Order; Low: 0,1Hz; High: 10Hz with 1000Hz Sampling; PassbandRippleDb 2,0; StopbandAttenuationDb 3,0").SetProperty("Precision", 1e-9);
            yield return new TestCaseData(FrequencyFilterType.BandPass, 666, 25.81, 42.42, 3, 1.0, 2.0,
                new Zpk(
                    [new(0.920768862720030, +0.390108576482576)
                    ,new(0.920768862720030, -0.390108576482576)
                    ,new(0.970582715296382, +0.240767923046034)
                    ,new(0.970582715296382, -0.240767923046034)
                    ,new(1.0, 0.0)
                    ,new(-1.0, 0.0)],

                    [new(0.920467593090236, +0.389673263369982)
                    ,new(0.920467593090236, -0.389673263369982)
                    ,new(0.845574429760139, +0.249439852072122)
                    ,new(0.845574429760139, -0.249439852072122)
                    ,new(0.970272376728099, +0.240870928321128)
                    ,new(0.970272376728099, -0.240870928321128)],
                    0.110668634705720)).SetName("(Zpk Test) BandPass: 3rd Order; Low: 25,81Hz; High: 42,42Hz with 666Hz; PassbandRippleDb 1,0; StopbandAttenuationDb 2,0");

            yield return new TestCaseData(FrequencyFilterType.BandStop, 1000, 0.1, 10, 2, 1.0, 2.0,
                new Zpk(
                    [new(0.998415109310899, +0.056278499444324)
                    ,new(0.998415109310899, -0.056278499444324)
                    ,new(0.999999754188797, +0.000701157860747)
                    ,new(0.999999754188797, -0.000701157860747)],

                    [new(0.992968359255317, +0.058968890792633)
                    ,new(0.992968359255317, -0.058968890792633)
                    ,new(0.999940791946563, +0.000660271141726)
                    ,new(0.999940791946563, -0.000660271141726)],
                    0.886419992761177)).SetName("(Zpk Test) BandStop: 2nd Order; Low: 0,1Hz; High: 10Hz with 1000Hz Sampling; PassbandRippleDb 1,0; StopbandAttenuationDb 2,0");
            yield return new TestCaseData(FrequencyFilterType.BandStop, 666, 25.81, 42.42, 3, 0.3, 1.7,
                new Zpk(
                    [new(0.951571739562318, +0.307426779032574)
                    ,new(0.951571739562318, -0.307426779032574)
                    ,new(0.922230088323862, +0.386641518968361)
                    ,new(0.922230088323862, -0.386641518968361)
                    ,new(0.970015819993956, +0.243041784394069)
                    ,new(0.970015819993956, -0.243041784394069)],

                    [new(0.920181434672973, +0.386684825514426)
                    ,new(0.920181434672973, -0.386684825514426)
                    ,new(0.919547565612201, +0.295168290130190)
                    ,new(0.919547565612201, -0.295168290130190)
                    ,new(0.969006604659148, +0.242244932620364)
                    ,new(0.969006604659148, -0.242244932620364)],
                    0.963308123246063)).SetName("(Zpk Test) BandStop: 3rd Order; Low: 25,81Hz; High: 42,42Hz with 666Hz; PassbandRippleDb 0,3; StopbandAttenuationDb 1,7").SetProperty("Precision", 1e-9);
        }
    }


    [TestCaseSource(nameof(ZpkTestDataElliptic))]
    public void CalcZpkTestsElliptic(FrequencyFilterType frequencyFilterType, double sourceFrequency, double lowCutOff, double highCutoff, int order, double passbandRippleDb, double stopbandAttenuationDb, Zpk expectedZpk)
    {
        //Arrange + Act
        var zpk = Elliptic.CalcZpk(frequencyFilterType, sourceFrequency, lowCutOff, highCutoff, order, passbandRippleDb, stopbandAttenuationDb);

        //Assert
        double comparePrecision = 1e-10;

        // Override if the test case has custom precision
        if (TestContext.CurrentContext.Test.Properties.ContainsKey("Precision"))
        {
            comparePrecision = (double)TestContext.CurrentContext.Test.Properties.Get("Precision")!;
        }

        TestHelper.CompareZpk(zpk, expectedZpk, comparePrecision);
    }
}
