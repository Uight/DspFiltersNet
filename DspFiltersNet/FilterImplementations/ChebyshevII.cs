using System.Numerics;

namespace DspFiltersNet.FilterImplementations;

internal class ChebyshevII
{
    /// <summary>
    /// Chebyshev Type II lowPass prototype.
    /// As in MATLAB => [z,p,k] = cheb2ap(order, Rs)
    /// </summary>
    /// <param name="filterOrder"></param>
    /// <param name="stopbandRippleDb"></param>
    /// <returns> Zeros, poles and gain of the ChebyshevII analog lowPass filter prototype of the specified order </returns>
    public static (List<Complex> poles, List<Complex> zeros, double gain) PrototypeAnalogLowPass(int filterOrder, double stopbandRippleDb)
    {
        if (filterOrder < 1 || filterOrder > 14)
        {
            throw new ArgumentOutOfRangeException(nameof(filterOrder));
        }
        if (stopbandRippleDb <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(stopbandRippleDb), "Stopband ripple must be positive.");
        }
            
        var epsilon = 1.0 / Math.Sqrt(Math.Pow(10, stopbandRippleDb / 10.0) - 1);
        var mu = FilterTools.Asinh(1.0 / epsilon) / filterOrder;

        var poles = new List<Complex>();
        var zeros = new List<Complex>();

        int numZeros = filterOrder / 2;
        for (int k = 1; k <= numZeros; k++)
        {
            var theta = Math.PI * (2 * k - 1) / (2.0 * filterOrder);
            zeros.Add(new Complex(0, 1.0 / Math.Cos(theta)));
            zeros.Add(new Complex(0, -1.0 / Math.Cos(theta)));
        }

        for (int k = 1; k <= filterOrder; k++)
        {
            var theta = Math.PI * (2 * k - 1) / (2.0 * filterOrder);
            var sigma = -Math.Sinh(mu) * Math.Sin(theta);
            var omega = Math.Cosh(mu) * Math.Cos(theta);
            var s = new Complex(sigma, omega);

            // Invert to get Chebyshev Type II pole
            poles.Add(Complex.One / s);
        }

        double gain;

        var numMag = 1.0;
        foreach (var z in zeros)
        {
            numMag *= z.Magnitude;
        }
        var denMag = 1.0;
        foreach (var p in poles)
        {
            denMag *= p.Magnitude;
        }

        gain = denMag / numMag;

        return (poles, zeros, gain);
    }
}
