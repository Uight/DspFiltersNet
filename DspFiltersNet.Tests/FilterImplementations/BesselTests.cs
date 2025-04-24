using DspFiltersNet.Filter;
using DspFiltersNet.FilterImplementations;
using System.Numerics;

namespace DspFiltersNet.Tests.FilterImplementations;

[TestFixture]
internal class BesselTests
{
    [Test]
    public void TestBesselPoleLimits()
    {
        Complex[] expectedPoles = new Complex[]
        {
            new Complex(-0.907793213839649, -0.082196399419402),
            new Complex(-0.907793213839649, +0.082196399419402),
            new Complex(-0.886950667491645, -0.247007917876533),
            new Complex(-0.886950667491645, +0.247007917876533),
            new Complex(-0.844119916090985, -0.413165382510269),
            new Complex(-0.844119916090985, +0.413165382510269),
            new Complex(-0.776659138706362, -0.581917067737761),
            new Complex(-0.776659138706362, +0.581917067737761),
            new Complex(-0.679425642511923, -0.755285730504203),
            new Complex(-0.679425642511923, +0.755285730504203),
            new Complex(-0.541876677511230, -0.937304368351692),
            new Complex(-0.541876677511230, +0.937304368351692),
            new Complex(-0.336386822490204, -1.139172297839860),
            new Complex(-0.336386822490204, +1.139172297839860),
        };

        var expectedZpk = new Zpk([], expectedPoles, 1.0);

        var calculatedPrototype = Bessel.PrototypeAnalogLowPass(14);

        TestHelper.CompareZpk(calculatedPrototype, expectedZpk, 1e-8);
    }

    //All testData are created with SciPy
    //The order of poles and zeros in SciPy result might differ but doesn't matter
    //The tests are not as extensive as for the Butterworth Filter but as only the internally called LowPassPrototype function is different this is ok
    public static IEnumerable<TestCaseData> ZpkTestDataBessel
    {
        get
        {
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 2,
                new Zpk([new(-1.0, 0.0), new(-1.0, 0.0)],
                    [new(0.9465547965606139, 0.02977609003147858), new(0.9465547965606139, -0.02977609003147858)],
                    0.0009357513270600223)).SetName("(Zpk Test) LowPass: 2nd Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 3,
                new Zpk([new (-1.0, 0.0),
                         new (-1.0, 0.0),
                         new (-1.0, 0.0)],
                        [new (0.7104146724701721, 0.2143540530450776),
                         new (0.7104146724701721, -0.2143540530450776),
                         new (0.6793041959000489, 0.0)],
                    0.005203582937450832)).SetName("(Zpk Test) LowPass: 3rd Order, 42.42Hz with 666Hz Sampling");

            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 2,
                new Zpk([new(1.0, 0.0), new(1.0, 0.0)],
                    [new(0.9465547965606139, -0.029776090031478592), new(0.9465547965606139, 0.029776090031478592)],
                    0.9474905478876746)).SetName("(Zpk Test) HighPass: 2nd Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 666, 0, 42.42, 3,
                new Zpk([new (1.0, 0.0),
                         new (1.0, 0.0),
                         new (1.0, 0.0)],
                        [new (0.7262969509908046, -0.20528479165595986),
                         new (0.7262969509908046, 0.20528479165595986),
                         new (0.6455586740081618, 0.0)],
                    0.6216597747378438)).SetName("(Zpk Test) HighPass: 3rd Order, 42.42Hz with 666Hz Sampling");

            yield return new TestCaseData(FrequencyFilterType.BandPass, 1000, 0.1, 10, 2,
                new Zpk([new (1.0, 0.0),
                        new (1.0, 0.0),
                        new (-1.0, 0.0),
                        new (-1.0, 0.0)],
                        [new (0.9994505814507483, -0.0003236912046215028), new (0.9475908107123208,  0.02981701542245074),
                         new (0.9994505814507483,  0.0003236912046215028), new (0.9475908107123208, -0.02981701542245074)],
                    0.0009176085114121442)).SetName("(Zpk Test) BandPass: 2nd Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandPass, 666, 0.2581, 42.42, 3,
                new Zpk([new (1.0, 0.0),
                        new (1.0, 0.0),
                        new (1.0, 0.0),
                        new (-1.0, 0.0),
                        new (-1.0, 0.0),
                        new (-1.0, 0.0)],
                        [new (0.9982892319685228, -0.0016481989407214756),
                        new (0.7129720125320612, 0.21494339447695793),
                        new (0.9982892319685228, 0.0016481989407214756),
                        new (0.7129720125320612, -0.21494339447695793),
                        new (0.9973837394530889, 0),
                        new (0.6827747121334972, 0)],
                    0.005121030868663763)).SetName("(Zpk Test) BandPass: 3rd Order, Low: 0.2581Hz, High: 42.42Hz, Sampling: 666Hz");

            yield return new TestCaseData(FrequencyFilterType.BandStop, 1000, 0.1, 10, 2,
                new Zpk([new (0.9999802544889885, 0.006284157233677085),
                        new (0.9999802544889885, -0.006284157233677085),
                        new (0.9999802544889885, 0.006284157233677085),
                        new (0.9999802544889885, -0.006284157233677085)],
                        [new (0.9994505814507483, 0.0003236912046215028), new (0.9475908107123208,  -0.029817015422450747),
                         new (0.9994505814507483,  -0.0003236912046215028), new (0.9475908107123208, 0.029817015422450747)],
                    0.9479974467608678)).SetName("(Zpk Test) BandStop: 2nd Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandStop, 666, 0.2581, 42.42, 3,
                new Zpk([new (0.9995062761841162, 0.03141980058117618),
                         new (0.9995062761841162, -0.03141980058117618),
                         new (0.9995062761841162, 0.03141980058117618),
                         new (0.9995062761841162, -0.03141980058117618),
                         new (0.9995062761841162, 0.03141980058117618),
                         new (0.9995062761841162, -0.03141980058117618)],
                        [new (0.9981843743159441, 0.0017516038265944186),
                         new (0.7288680547781037, -0.20600205727233625),
                         new (0.9981843743159441, -0.0017516038265944186),
                         new (0.7288680547781037, 0.20600205727233625),
                         new (0.9976835095973491, 0),
                         new (0.6488860208193175, 0)],
                    0.6234143718105545)).SetName("(Zpk Test) BandStop: 3rd Order, Low: 0.2581Hz, High: 42.42Hz, Sampling: 666Hz");
        }
    }

    //All testData are created with SciPy
    public static IEnumerable<TestCaseData> TfTestDataBessel
    {
        get
        {
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 1,
                new TransferFunction([0.03046874709125383, 0.03046874709125383],
                    [1, -0.9390625058174923]),
                1.0e-15).SetName("(Tf Test) LowPass: 1st Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 2,
                new TransferFunction([0.0009357513270600223, 0.0018715026541200445, 0.0009357513270600223],
                    [1, -1.8931095931212278, 0.896852598429468]),
                1.0e-15).SetName("(Tf Test) LowPass: 2nd Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 3,
                new TransferFunction([0.005203582937450832, 0.015610748812352497, 0.015610748812352497, 0.005203582937450832],
                    [1, -2.100133540840393, 1.5158120025936475, -0.37404979825364776]),
                1.0e-15).SetName("(Tf Test) LowPass: 3rd Order, 42.42Hz with 666Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 4,
                new TransferFunction([8.846036313160676e-07, 3.5384145252642704e-06, 5.307621787896406e-06, 3.5384145252642704e-06, 8.846036313160676e-07],
                    [1, -3.805643184431058, 5.4337661221168725, -3.4498535363729785, 0.821744752345264]),
                1.0e-14).SetName("(Tf Test) LowPass: 4th Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 6,
                new TransferFunction([8.367960734579576e-10, 5.020776440747746e-09, 1.2551941101869365e-08, 1.673592146915915e-08, 1.2551941101869365e-08, 5.020776440747746e-09, 8.367960734579576e-10],
                    [1, -5.719477889709566, 13.634615252897994, -17.340737492937027, 12.409444495524522, -4.7377439340260725, 0.7538996218050958]),
                1.0e-14).SetName("(Tf Test) LowPass: 6th Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 8,
                new TransferFunction([7.917182989393493e-13, 6.333746391514794e-12, 2.2168112370301782e-11, 4.4336224740603563e-11, 5.542028092575445e-11, 4.4336224740603563e-11, 2.2168112370301782e-11, 6.333746391514794e-12, 7.917182989393493e-13],
                    [1, -7.633683992720539, 25.50068955150787, -48.68958172113381, 58.11704573517522, -44.4071889723683, 21.212131266066173, -5.791319289443753, 0.6919074231198276]),
                1.0e-13).SetName("(Tf Test) LowPass: 8th Order, 10Hz with 1000Hz Sampling");

            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 2,
                new TransferFunction([0.9474905478876746, -1.8949810957753492, 0.9474905478876746],
                    [1, -1.8931095931212278, 0.896852598429468]),
                1.0e-14).SetName("(Tf Test) HighPass: 2nd Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 666, 0, 42.42, 3,
                new TransferFunction([0.6216597747378438, -1.8649793242135315, 1.8649793242135315, -0.6216597747378438],
                    [1, -2.0981525759897712, 1.5073836999393595, -0.36774192197361966]),
                1.0e-14).SetName("(Tf Test) HighPass: 3rd Order, 42.42Hz with 666Hz Sampling");

            yield return new TestCaseData(FrequencyFilterType.BandPass, 1000, 0.1, 10, 2,
                new TransferFunction([0.0009176085114121442, 0, -0.0018352170228242883, 0, 0.0009176085114121442],
                    [1, -3.894082784326138, 5.68599971546863, -3.6897470402086108, 0.8978301105445591]),
                1.0e-15).SetName("(Tf Test) BandPass: 2nd Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandPass, 666, 0.2581, 42.42, 3,
                new TransferFunction([0.005121030868663763, 0, -0.015363092605991288, 0, 0.015363092605991288, 0, -0.005121030868663763],
                    [1, -5.102680940587755, 10.829491473162264, -12.248476931257136, 7.795542193957259, -2.6502141839302022, 0.3763383892578121]),
                1.0e-14).SetName("(Tf Test) BandPass: 3rd Order, Low: 0.2581Hz, High: 42.42Hz, Sampling: 666Hz");

            yield return new TestCaseData(FrequencyFilterType.BandStop, 1000, 0.1, 10, 2,
                new TransferFunction([0.9479974467608678, -3.791914912267376, 5.687834932491456, -3.791914912267375, 0.9479974467608675],
                    [1, -3.894082784326138, 5.68599971546863, -3.6897470402086108, 0.8978301105445591]),
                1.0e-15).SetName("(Tf Test) BandStop: 2nd Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandStop, 666, 0.2581, 42.42, 3,
                new TransferFunction([0.6234143718105545, -3.738639463728164, 9.3438303322035, -12.45721047997155, 9.3438303322035, -3.7386394637281635, 0.6234143718105541],
                    [1, -5.100674388604762, 10.815046111927547, -12.21089383229871, 7.749395392409139, -2.6229211865244104, 0.3700479036914302]),
                1.0e-14).SetName("(Tf Test) BandStop: 3rd Order, Low: 0.2581Hz, High: 42.42Hz, Sampling: 666Hz");
        }
    }

    [TestCaseSource(nameof(ZpkTestDataBessel))]
    public void CalcZpkTestsBessel(FrequencyFilterType frequencyFilterType, double sourceFrequency, double lowCutOff, double highCutoff, int order, Zpk expectedZpk)
    {
        //Arrange + Act
        var zpk = Bessel.CalcZpk(frequencyFilterType, sourceFrequency, lowCutOff, highCutoff, order);

        //Assert
        TestHelper.CompareZpk(zpk, expectedZpk);
    }

    [TestCaseSource(nameof(TfTestDataBessel))]
    public void CalcTransferFunctionTestsBessel(FrequencyFilterType frequencyFilterType, double sourceFrequency, double lowCutOff, double highCutoff, int order, TransferFunction expectedTf, double tolerance)
    {
        //Arrange + Act
        var tf = Bessel.CalcTransferFunction(frequencyFilterType, sourceFrequency, lowCutOff, highCutoff, order);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(tf.A, Is.EqualTo(expectedTf.A).Within(tolerance), "A incorrect");
            Assert.That(tf.B, Is.EqualTo(expectedTf.B).Within(tolerance), "B incorrect");
        });
    }

    [TestCase(15)]
    public void CalcBesselZpkOrderOutOfRange(int order)
    {
        //Arrange

        //Act + Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => Bessel.CalcZpk(FrequencyFilterType.LowPass, 1000, 10, 0, order));
    }

    [TestCase(15)]
    public void CalcBesselTransferFunctionOrderOutOfRange(int order)
    {
        //Arrange

        //Act + Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => Bessel.CalcTransferFunction(FrequencyFilterType.LowPass, 1000, 10, 0, order));
    }
}
