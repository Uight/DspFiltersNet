using System.Numerics;
using MathNet.Numerics;

namespace DspFiltersNet.FilterImplementations;

internal static class Bessel
{
    /// <summary>
    /// Bessel lowPass prototype.
    /// As in Python + Scipy => z,p,k = besselap(order)
    /// As in MATLAB => [z,p,k] = besselap(order)
    /// </summary>
    /// <param name="filterOrder"></param>
    /// <returns> Poles of the Bessel analog lowPass filter prototype of the specified order </returns>
    public static List<Complex> PrototypeAnalogLowPass(int filterOrder)
    {
        // The bessel polynomial has pretty high numbers pretty fast. With current implementation this leads
        // to precision loss pretty fast, which is why the max order is limited to 8.
        if (filterOrder < 1 || filterOrder > 8)
        {
            throw new ArgumentOutOfRangeException(nameof(filterOrder));
        }
        return GetPoles(filterOrder);
    }

    private static List<Complex> GetPoles(int order)
    {
        var polyCoefficients = BesselPoly(order);

        var polynomial = new Polynomial(polyCoefficients);
        var roots = polynomial.Roots();

        var scaleFactor = 1 / Math.Pow(polyCoefficients[0], 1.0 / order);

        for (var i = 0; i < roots.Length; i++)
        {
            roots[i] *= new Complex(scaleFactor, 0);
        }

        return roots.ToList();
    }

    private static List<double> BesselPoly(int order)
    {
        var a = new List<double>();
        for (var i = 0; i <= order; i++)
        {
            var num = Factorial(2 * order - i);
            var den = Convert.ToInt64(Math.Pow(2, order - i) * (Factorial(i) * Factorial(order - i)));
            a.Add(Convert.ToDouble(num / den));
        }

        return a;
    }

    private static long Factorial(long f)
    {
        if (f <= 1)
        {
            return 1;
        }
        return f * Factorial(f - 1);
    }
}