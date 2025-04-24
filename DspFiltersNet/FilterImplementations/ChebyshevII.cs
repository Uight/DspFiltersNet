using DspFiltersNet.Filter;
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
    public static Zpk PrototypeAnalogLowPass(int filterOrder, double stopbandRippleDb)
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

        return new Zpk(zeros.ToArray(), poles.ToArray(), gain);
    }

    /// <summary>
    /// Calculate IIR Filter transferFunction coefficients
    /// As in MATLAB => [z,p,k] = cheby2(n, Rs, Ws)
    ///
    /// In contrast to matlab this method does not require to normalize the frequencies before calling.
    /// If BandPass or BandStop is selected the actual order of the filter created is double that of the specified order.
    /// </summary>
    /// <param name="frequencyFilterType"></param>
    /// <param name="freqSampling"></param>
    /// <param name="freqLowCutOff">The LowCutofff is not used if 'filterType' is 'HighPass'.</param>
    /// <param name="freqHighCutOff">The HighCutOff is not used if 'filterType' is 'LowPass'.</param>
    /// <param name="filterOrder"></param>
    /// <param name="rippleDb"></param>
    /// <returns> zeros(z), poles(p) and gain(k) for the specified filter settings </returns>
    public static Zpk CalcZpk(FrequencyFilterType frequencyFilterType,
        double freqSampling, double freqLowCutOff, double freqHighCutOff, int filterOrder, double rippleDb)
    {
        FilterTools.FrequencyVerification(frequencyFilterType, freqSampling, freqLowCutOff, freqHighCutOff);
        var lowPassPrototype = PrototypeAnalogLowPass(filterOrder, rippleDb);
        var filter = FilterTools.CalcFilterSettings(frequencyFilterType, freqSampling, freqLowCutOff, freqHighCutOff, filterOrder, lowPassPrototype);
        return filter.zpk;
    }

    /// <summary>
    /// Calculate IIR Filter transferFunction coefficients
    /// As in MATLAB => [b,a] = cheby2(n, Rs, Ws)
    ///
    /// In contrast to matlab this method does not require to normalize the frequencies before calling.
    /// If BandPass or BandStop is selected the actual order of the filter created is double that of the specified order.
    /// </summary>
    /// <param name="frequencyFilterType"></param>
    /// <param name="freqSampling"></param>
    /// <param name="freqLowCutOff">The LowCutofff is not used if 'filterType' is 'HighPass'.</param>
    /// <param name="freqHighCutOff">The HighCutOff is not used if 'filterType' is 'LowPass'.</param>
    /// <param name="filterOrder"></param>
    /// <param name="rippleDb"></param>
    /// <returns> numerator(b) and denominator(a) for the specified filter settings </returns>
    public static TransferFunction CalcTransferFunction(FrequencyFilterType frequencyFilterType,
        double freqSampling, double freqLowCutOff, double freqHighCutOff, int filterOrder, double rippleDb)
    {
        FilterTools.FrequencyVerification(frequencyFilterType, freqSampling, freqLowCutOff, freqHighCutOff);
        var lowPassPrototype = PrototypeAnalogLowPass(filterOrder, rippleDb);
        var filter = FilterTools.CalcFilterSettings(frequencyFilterType, freqSampling, freqLowCutOff, freqHighCutOff, filterOrder, lowPassPrototype);
        return filter.tf;
    }
}
