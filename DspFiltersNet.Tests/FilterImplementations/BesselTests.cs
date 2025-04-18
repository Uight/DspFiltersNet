using DspFiltersNet.FilterImplementations;
using System.Numerics;

namespace DspFiltersNet.Tests.FilterImplementations;

[TestFixture]
internal class BesselTests
{
    [Test]
    public void TestBesselLimits()
    {
        Complex[] referencePoles = new Complex[]
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

        var calculatedPoles = Bessel.PrototypeAnalogLowPass(14);

        // Sort both by real part (then imag part) to align them
        var sortedExpected = referencePoles.OrderBy(c => c.Real).ThenBy(c => c.Imaginary).ToArray();
        var sortedActual = calculatedPoles.OrderBy(c => c.Real).ThenBy(c => c.Imaginary).ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(calculatedPoles, Has.Count.EqualTo(referencePoles.Length), "Pole count mismatch.");

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
}
