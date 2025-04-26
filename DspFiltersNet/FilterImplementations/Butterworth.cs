using DspFiltersNet.Filter;
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
    /// <returns> Zeros, poles and gain of the Butterworth analog lowPass filter prototype of the specified order </returns>
    public static Zpk PrototypeAnalogLowPass(int filterOrder)
    {
        if (filterOrder < 1 || filterOrder > 16)
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

        return new Zpk([], poles.ToArray(), 1.0);
    }

    /// <summary>
    /// Calculate IIR Filter transferFunction coefficients
    /// As in MATLAB => [z,p,k] = butter(n, Wn, ...)
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
    /// As in MATLAB => [b,a] = butter(n, Wn, ...)
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
}