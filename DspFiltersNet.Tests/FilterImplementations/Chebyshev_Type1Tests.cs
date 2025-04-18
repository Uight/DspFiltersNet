using DspFiltersNet.FilterImplementations;
using System.Numerics;

namespace DspFiltersNet.Tests.FilterImplementations;

[TestFixture]
internal class Chebyshev_Type1Tests
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

        var calculatedPrototype = Chebyshev_Type1.PrototypeAnalogLowPass(6, 3);

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
}
