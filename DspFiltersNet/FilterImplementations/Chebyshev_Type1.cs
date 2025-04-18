using System.Numerics;

namespace DspFiltersNet.FilterImplementations;

internal class Chebyshev_Type1
{
    /// <summary>
    /// Bessel lowPass prototype.
    /// As in MATLAB => [z,p,k] = cheb1ap(order, Rp)
    /// </summary>
    /// <param name="filterOrder"></param>
    /// <param name="rippleDb"></param>
    /// <returns> Poles of the ChebyshevI analog lowPass filter prototype of the specified order </returns>
    public static (List<Complex> poles, double gain) PrototypeAnalogLowPass(int filterOrder, double rippleDb)
    {
        if (filterOrder < 1 || filterOrder > 14)
        {
            throw new ArgumentOutOfRangeException(nameof(filterOrder));
        }

        if (rippleDb <= 0.0)
        {
            throw new ArgumentOutOfRangeException(nameof(rippleDb), "Ripple must be positive.");
        } 

        var epsilon = Math.Sqrt(Math.Pow(10, rippleDb / 10.0) - 1);
        var sinhAsinh = Asinh(1 / epsilon) / filterOrder;

        var poles = new List<Complex>();
        for (int k = 1; k <= filterOrder; k++)
        {
            var theta = Math.PI * (2 * k - 1) / (2.0 * filterOrder);
            var sigma = -Math.Sinh(sinhAsinh) * Math.Sin(theta);
            var omega = Math.Cosh(sinhAsinh) * Math.Cos(theta);
            poles.Add(new Complex(sigma, omega));
        }

        Complex product = Complex.One;
        foreach (var p in poles)
        {
            product *= -p;
        }

        var gain = (1.0 / Math.Sqrt(1 + epsilon * epsilon)) * product.Magnitude;

        return (poles, gain);
    }

    private static double Asinh(double x)
    {
        return Math.Log(x + Math.Sqrt(x * x + 1.0));
    }
}
