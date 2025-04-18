using DspFiltersNet.FilterImplementations;

namespace DspFiltersNet.Tests.FilterImplementations;

internal class TestHelper
{
    public static void CompareZpk(Zpk actual, Zpk expected)
    {
        Assert.Multiple(() =>
        {
            Assert.That(actual.K, Is.EqualTo(expected.K).Within(1.0e-14), "Gain incorrect");

            Assert.That(actual.Z, Has.Length.EqualTo(expected.Z.Length), "Zeros length not matching");
            for (var i = 0; i < actual.Z.Length; i++)
            {
                Assert.That(actual.Z[i].Real, Is.EqualTo(expected.Z[i].Real).Within(1.0e-14), "Real of zero does not match");
                Assert.That(actual.Z[i].Imaginary, Is.EqualTo(expected.Z[i].Imaginary).Within(1.0e-14), "Imag of zero does not match");
            }

            // Sort both by real part (then imag part) to align them
            var sortedExpectedPoles = expected.P.OrderBy(c => c.Real).ThenBy(c => c.Imaginary).ToArray();
            var sortedActualPoles = actual.P.OrderBy(c => c.Real).ThenBy(c => c.Imaginary).ToArray();

            Assert.That(actual.P, Has.Length.EqualTo(expected.P.Length), "Poles length not matching");
            for (var i = 0; i < actual.P.Length; i++)
            {
                Assert.That(sortedActualPoles[i].Real, Is.EqualTo(sortedExpectedPoles[i].Real).Within(1.0e-14), "Real of pole does not match");
                Assert.That(sortedActualPoles[i].Imaginary, Is.EqualTo(sortedExpectedPoles[i].Imaginary).Within(1.0e-14), "Imag of pole does not match");
            }
        });
    }
}
