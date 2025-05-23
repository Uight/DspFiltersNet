﻿using DspFiltersNet.Filter;
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

                    0.841395141645195)).SetName("LowPassPrototype Test: Order 1; StopBandRippleDb 1,5");

            yield return new TestCaseData(1, 0.5,
                new Zpk(
                    [],

                    [new Complex(-2.862775161243190, 0.0)],

                    2.862775161243190)).SetName("LowPassPrototype Test: Order 2; StopBandRippleDb 0,5");

            yield return new TestCaseData(3, 1.5,
                new Zpk(
                    [new Complex(0.000000000000000, +1.154700538379252)
                    ,new Complex(0.000000000000000, -1.154700538379252)],

                    [new Complex(-0.128257008136217, -1.116902145128026)
                    ,new Complex(-4.927295125211721, +0.000000000000000)
                    ,new Complex(-0.128257008136217, +1.116902145128026)],

                    4.670781108939289)).SetName("LowPassPrototype Test: Order 3; StopBandRippleDb 1,5");

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

                    0.922571427154765)).SetName("LowPassPrototype Test: Order 6; StopBandRippleDb 0,7");
        }
    }

    [TestCaseSource(nameof(LowPassPrototype_TestDataChebyshevII))]
    public void Chebyshev_Type2_TestPrototype(int filterOrder, double stopbandRippleDb, Zpk expectedZpk)
    {
        var calculatedPrototype = ChebyshevII.PrototypeAnalogLowPass(filterOrder, stopbandRippleDb);

        TestHelper.CompareZpk(calculatedPrototype, expectedZpk);
    }

    public static IEnumerable<TestCaseData> ZpkTestDataChebyshevII
    {
        get
        {
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 1, 1.0,
                new Zpk([new(-1.0, 0.0)],
                    [new(0.883665323160146, 0.0)],
                    0.058167338419927)).SetName("(Zpk Test) LowPass: 1st Order; 10Hz with 1000Hz Sampling; RippleDb 1,0");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 2, 3.0,
                new Zpk(
                    [new(0.847973072142402, +0.530039308845465)
                    ,new(0.847973072142402, -0.530039308845465)],

                    [new(0.757844226499809, +0.358925572595592)
                    ,new(0.757844226499809, -0.358925572595592)],
                    0.616558487185070)).SetName("(Zpk Test) LowPass: 2nd Order; 42,42Hz with 666Hz Sampling; RippleDb 3,0");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 3, 3.0,
                new Zpk(
                    [new(0.896013934413071, +0.444025933181395)
                    ,new(0.896013934413071, -0.444025933181395)
                    ,new(-1.0, 0.0)],

                    [new(0.848294079736655, +0.389913572250465)
                    ,new(0.848294079736655, -0.389913572250465)
                    ,new(0.189176251093807, 0.0)],
                    0.341229594223785)).SetName("(Zpk Test) LowPass: 3rd Order; 42,42Hz with 666Hz Sampling; RippleDb 3,0");

            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 2, 3.0,
                new Zpk(
                    [new(0.999012877248820, +0.044421516082118)
                    ,new(0.999012877248820, -0.044421516082118)],

                    [new(0.978861579471482, +0.047813896797458)
                    ,new(0.978861579471482, -0.047813896797458)],
                    0.980028534090235)).SetName("(Zpk Test) HighPass: 2nd Order; 10Hz with 1000Hz Sampling; RippleDb 3,0");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 666, 0, 42.42, 3, 2.0,
                new Zpk(
                    [new(0.940146350013228, +0.340770950283039)
                    ,new(0.940146350013228, -0.340770950283039)
                    ,new(1.0, 0.0)],

                    [new(0.894176163386405, +0.333894252851086)
                    ,new(0.894176163386405, -0.333894252851086)
                    ,new(0.908221441800919, 0.0)],
                    0.909628912528167)).SetName("(Zpk Test) HighPass: 3rd Order; 42,42Hz with 666Hz Sampling; RippleDb 2,0");

            yield return new TestCaseData(FrequencyFilterType.BandPass, 1000, 0.1, 10, 2, 3.0,
                new Zpk(
                    [new(0.996096389510611, +0.088272208559242)
                    ,new(0.996096389510611, -0.088272208559242)
                    ,new(0.999999900314878, +0.000446508941441)
                    ,new(0.999999900314878, -0.000446508941441)],

                    [new(0.970021093593686, +0.066940235089540)
                    ,new(0.970021093593686, -0.066940235089540)
                    ,new(0.999799576524327, +0.000491295064748)
                    ,new(0.999799576524327, -0.000491295064748)],
                    0.688881301948538)).SetName("(Zpk Test) BandPass: 2nd Order; Low: 0,1Hz; High: 10Hz with 1000Hz Sampling; RippleDb 3,0");
            yield return new TestCaseData(FrequencyFilterType.BandPass, 666, 25.81, 42.42, 3, 2.0,
                new Zpk(
                    [new(0.914994397360931, +0.403466544831298)
                    ,new(0.914994397360931, -0.403466544831298)
                    ,new(0.972635003939728, +0.232338436577258)
                    ,new(0.972635003939728, -0.232338436577258)
                    ,new(1.0, 0.0)
                    ,new(-1.0, 0.0)],

                    [new(0.903934452585091, +0.393026182839214)
                    ,new(0.903934452585091, -0.393026182839214)
                    ,new(0.622585176993431, 0.0)
                    ,new(0.807199162338482, 0.0)
                    ,new(0.963772786910616, +0.232994350647842)
                    ,new(0.963772786910616, -0.232994350647842)],
                    0.226320224603446)).SetName("(Zpk Test) BandPass: 3rd Order; Low: 25,81Hz; High: 42,42Hz with 666Hz; RippleDb 2,0");
            
            yield return new TestCaseData(FrequencyFilterType.BandStop, 1000, 0.1, 10, 2, 3.0,
                new Zpk(
                    [new(0.998993439394469, +0.044856527360111)
                    ,new(0.998993439394469, -0.044856527360111)
                    ,new(0.999999612843379, +0.000879950619101)
                    ,new(0.999999612843379, -0.000879950619101)],

                    [new(0.979322142348896, +0.048037908938875)
                    ,new(0.979322142348896, -0.048037908938875)
                    ,new(0.999720994321846, +0.000693384676833)
                    ,new(0.999720994321846, -0.000693384676833)],
                    0.980226173793281)).SetName("(Zpk Test) BandStop: 2nd Order; Low: 0,1Hz; High: 10Hz with 1000Hz Sampling; RippleDb 3,0");
            yield return new TestCaseData(FrequencyFilterType.BandStop, 666, 25.81, 42.42, 3, 2.0,
                new Zpk(
                    [new(0.951571739562318, +0.307426779032574)
                    ,new(0.951571739562318, -0.307426779032574)
                    ,new(0.925883216496115, +0.377809832337391)
                    ,new(0.925883216496115, -0.377809832337391)
                    ,new(0.968502359491254, +0.249004376788589)
                    ,new(0.968502359491254, -0.249004376788589)],

                    [new(0.914747750264144, +0.375563317908826)
                    ,new(0.914747750264144, -0.375563317908826)
                    ,new(0.934178513609807, +0.301253495973557)
                    ,new(0.934178513609807, -0.301253495973557)
                    ,new(0.961846781776758, +0.245638259806134)
                    ,new(0.961846781776758, -0.245638259806134)],
                    0.963531909758167)).SetName("(Zpk Test) BandStop: 3rd Order; Low: 25,81Hz; High: 42,42Hz with 666Hz; RippleDb 2,0");
        }
    }


    [TestCaseSource(nameof(ZpkTestDataChebyshevII))]
    public void CalcZpkTestsChebyshevI(FrequencyFilterType frequencyFilterType, double sourceFrequency, double lowCutOff, double highCutoff, int order, double rippleDb, Zpk expectedZpk)
    {
        //Arrange + Act
        var zpk = ChebyshevII.CalcZpk(frequencyFilterType, sourceFrequency, lowCutOff, highCutoff, order, rippleDb);

        //Assert
        TestHelper.CompareZpk(zpk, expectedZpk);
    }
}
