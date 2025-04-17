using System.Numerics;

namespace DspFiltersNet.FilterImplementations;

internal static class Butterworth
{
    /// <summary>
    /// Butterworth lowPass prototype. Places poles evenly around the negative side of the S-plane unit circle.
    /// As in Python + Scipy => z,p,k = buttap(order)
    /// As in MATLAB => [z,p,k] = buttap(order)
    /// </summary>
    /// <param name="filterOrder"></param>
    /// <returns> Poles of the Butterworth analog lowPass filter prototype of the specified order </returns>
    public static List<Complex> PrototypeAnalogLowPass(int filterOrder)
    {
        if (filterOrder < 1 || filterOrder > 10)
        {
            throw new ArgumentOutOfRangeException(nameof(filterOrder));
        }

        var poles = new List<Complex>();

        for (var k = 0; k < filterOrder / 2; k++)
        {
            var theta = (2.0 * k + 1.0) * Math.PI / (2.0 * filterOrder);
            var real = -Math.Sin(theta);
            var imag = Math.Cos(theta);
            poles.Add(new Complex(real, imag));
            poles.Add(new Complex(real, -imag)); // Conjugate
        }
        if (filterOrder % 2 == 1)
        {
            poles.Add(new Complex(-1.0, 0.0));
        }

        return poles;
    }
}