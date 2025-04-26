using System.Numerics;
using DspFiltersNet.Filter;

namespace DspFiltersNet.FilterImplementations;

internal static class Bessel
{
    /// <summary>
    /// Bessel lowPass prototype.
    /// As in Python + Scipy => z,p,k = besselap(order)
    /// As in MATLAB => [z,p,k] = besselap(order)
    /// </summary>
    /// <param name="filterOrder"></param>
    /// <returns> Zeros, poles and gain of the Bessel analog lowPass filter prototype of the specified order </returns>
    public static Zpk PrototypeAnalogLowPass(int filterOrder)
    {
        // The bessel polynomial has pretty high numbers pretty fast. Using BigInteger enables
        // orders higher than 10 but above 16 the Roots function starts failing
        if (filterOrder < 1 || filterOrder > 16)
        {
            throw new ArgumentOutOfRangeException(nameof(filterOrder));
        }
        return new Zpk([], GetPoles(filterOrder), 1.0);
    }

    /// <summary>
    /// Calculate IIR Filter transferFunction coefficients
    /// As in MATLAB => [z,p,k] = bessel(n, Wn, ...)
    ///
    /// In contrast to matlab this method does not require to normalize the frequencies before calling.
    /// If BandPass or BandStop is selected the actual order of the filter created is double that of the specified order.
    /// </summary>
    /// <param name="frequencyFilterType"></param>
    /// <param name="freqSampling"></param>
    /// <param name="freqLowCutOff">The LowCutofff is not used if 'filterType' is 'HighPass'.</param>
    /// <param name="freqHighCutOff">The HighCutOff is not used if 'filterType' is 'LowPass'.</param>
    /// <param name="filterOrder"></param>
    /// <returns> zeros(z), poles(p) and gain(k) for the specified filter settings </returns>
    public static Zpk CalcZpk(FrequencyFilterType frequencyFilterType,
        double freqSampling, double freqLowCutOff, double freqHighCutOff, int filterOrder)
    {
        FilterTools.FrequencyVerification(frequencyFilterType, freqSampling, freqLowCutOff, freqHighCutOff);
        var lowPassPrototype = PrototypeAnalogLowPass(filterOrder);
        var filter = FilterTools.CalcFilterSettings(frequencyFilterType, freqSampling, freqLowCutOff, freqHighCutOff, filterOrder, lowPassPrototype);
        return filter.zpk;
    }

    /// <summary>
    /// Calculate IIR Filter transferFunction coefficients
    /// As in MATLAB => [b,a] = bessel(n, Wn, ...)
    ///
    /// In contrast to matlab this method does not require to normalize the frequencies before calling.
    /// If BandPass or BandStop is selected the actual order of the filter created is double that of the specified order.
    /// </summary>
    /// <param name="frequencyFilterType"></param>
    /// <param name="freqSampling"></param>
    /// <param name="freqLowCutOff">The LowCutofff is not used if 'filterType' is 'HighPass'.</param>
    /// <param name="freqHighCutOff">The HighCutOff is not used if 'filterType' is 'LowPass'.</param>
    /// <param name="filterOrder"></param>
    /// <returns> numerator(b) and denominator(a) for the specified filter settings </returns>
    public static TransferFunction CalcTransferFunction(FrequencyFilterType frequencyFilterType,
        double freqSampling, double freqLowCutOff, double freqHighCutOff, int filterOrder)
    {
        FilterTools.FrequencyVerification(frequencyFilterType, freqSampling, freqLowCutOff, freqHighCutOff);
        var lowPassPrototype = PrototypeAnalogLowPass(filterOrder);
        var filter = FilterTools.CalcFilterSettings(frequencyFilterType, freqSampling, freqLowCutOff, freqHighCutOff, filterOrder, lowPassPrototype);
        return filter.tf;
    }

    private static Complex[] GetPoles(int order)
    {
        var polyCoefficients = BesselPoly(order);

        var roots = PolynomialRoots(polyCoefficients.ToArray().Reverse().ToArray());

        var scaleFactor = 1 / Math.Pow(polyCoefficients[0], 1.0 / order);

        for (var i = 0; i < roots.Length; i++)
        {
            roots[i] *= new Complex(scaleFactor, 0);
        }

        return roots;
    }

    private static List<double> BesselPoly(int order)
    {
        var a = new List<double>();
        for (var i = 0; i <= order; i++)
        {
            BigInteger num = Factorial(2 * order - i);
            BigInteger den = BigInteger.Pow(2, order - i) * Factorial(i) * Factorial(order - i);
            double result = (double)(num / den);
            a.Add(result);
        }

        return a;
    }

    private static BigInteger Factorial(int n)
    {
        BigInteger result = 1;
        for (int i = 2; i <= n; i++)
        {
            result *= i;
        }   
        return result;
    }

    /// <summary>
    /// Evaluates complex roots of polynomials using Durand-Kerner algorithm. 
    /// Generally works for polynomials of order up to approx. 50
    /// </summary>
    public static Complex[] PolynomialRoots(double[] a)
    {
        // This two settings are optimized for the finding of bessel roots. This works
        // up to order 16 before inf values appear and with 1e-8 as a stop tolerance threshold
        // the result is found in around 500 iterations and matches the one from matlab to 1e-14 at least.
        const int rootFindingIterations = 5000;
        const double stopIterationToleranceThreshold = 1e-8;

        var c1 = Complex.One;

        var rootsPrev = new Complex[a.Length - 1];
        var roots = new Complex[a.Length - 1];

        var result = new Complex(0.4, 0.9);
        rootsPrev[0] = c1;

        for (var i = 1; i < rootsPrev.Length; i++)
        {
            rootsPrev[i] = rootsPrev[i - 1] * result;
        }

        var iter = 0;
        while (true)
        {
            for (int i = 0; i < rootsPrev.Length; i++)
            {
                result = c1;

                for (int j = 0; j < rootsPrev.Length; j++)
                {
                    if (i != j)
                    {
                        result = (rootsPrev[i] - rootsPrev[j]) * result;
                    }
                }

                roots[i] = rootsPrev[i] - (EvaluatePolynomial(a, rootsPrev[i]) / result);
            }

            if (++iter > rootFindingIterations || ArraysAreEqual(rootsPrev, roots, stopIterationToleranceThreshold))
            {
                break;
            }

            Array.Copy(roots, rootsPrev, roots.Length);
        }

        return roots;
    }

    /// <summary>
    /// Evaluates polynomial according to Horner scheme.
    /// </summary>
    public static Complex EvaluatePolynomial(double[] coefficients, Complex x)
    {
        var result = Complex.Zero;

        for (int i = 0; i < coefficients.Length; i++)
        {
            result = result * x + coefficients[i];
        }

        return result;
    }

    /// <summary>
    /// Checks if two arrays of complex numbers are essentially identical.
    /// </summary>
    private static bool ArraysAreEqual(Complex[] a, Complex[] b, double tolerance)
    {
        for (var i = 0; i < a.Length; i++)
        {
            if (Complex.Abs(a[i] - b[i]) > tolerance)
            {
                return false;
            }
        }

        return true;
    }
}