using DspFiltersNet.FilterImplementations;

namespace DspFiltersNet.Tests.FilterImplementations;

internal class TestHelper
{
    public static void CompareZpk(Zpk actual, Zpk expected, double tolerance = 1e-14)
    {
        Assert.Multiple(() =>
        {
            Assert.That(actual.K, Is.EqualTo(expected.K).Within(tolerance), "Gain incorrect");

            // Sort zeros by real part (then imag part) to align them
            var sortedExpectedZeros = expected.Z.OrderBy(c => c.Real).ThenBy(c => c.Imaginary).ToArray();
            var sortedActualZeros = actual.Z.OrderBy(c => c.Real).ThenBy(c => c.Imaginary).ToArray();

            Assert.That(actual.Z, Has.Length.EqualTo(expected.Z.Length), "Zeros length not matching");
            for (var i = 0; i < actual.Z.Length; i++)
            {
                Assert.That(sortedActualZeros[i].Real, Is.EqualTo(sortedExpectedZeros[i].Real).Within(tolerance), "Real of zero does not match");
                Assert.That(sortedActualZeros[i].Imaginary, Is.EqualTo(sortedExpectedZeros[i].Imaginary).Within(tolerance), "Imag of zero does not match");
            }

            // Sort poles by real part (then imag part) to align them
            var sortedExpectedPoles = expected.P.OrderBy(c => Math.Round(c.Real / tolerance) * tolerance).ThenBy(c => c.Imaginary).ToArray();
            var sortedActualPoles = actual.P.OrderBy(c => Math.Round(c.Real / tolerance) * tolerance).ThenBy(c => c.Imaginary).ToArray();

            Assert.That(actual.P, Has.Length.EqualTo(expected.P.Length), "Poles length not matching");
            for (var i = 0; i < actual.P.Length; i++)
            {
                Assert.That(sortedActualPoles[i].Real, Is.EqualTo(sortedExpectedPoles[i].Real).Within(tolerance), "Real of pole does not match");
                Assert.That(sortedActualPoles[i].Imaginary, Is.EqualTo(sortedExpectedPoles[i].Imaginary).Within(tolerance), "Imag of pole does not match");
            }
        });
    }
}
