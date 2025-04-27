using DspFiltersNet.Filter;
using System.Numerics;

namespace DspFiltersNet.FilterImplementations;

internal class ChebyshevI
{
    /// <summary>
    /// Chebyshev Type I lowPass prototype.
    /// As in MATLAB => [z,p,k] = cheb1ap(order, Rp)
    /// </summary>
    /// <param name="filterOrder"></param>
    /// <param name="passbandRippleDb">Max ripple for the passband in db.</param>
    /// <returns> Zeros, poles and gain of the ChebyshevI analog lowPass filter prototype of the specified order </returns>
    public static Zpk PrototypeAnalogLowPass(int filterOrder, double passbandRippleDb)
    {
        if (filterOrder < 1 || filterOrder > 16)
        {
            throw new ArgumentOutOfRangeException(nameof(filterOrder));
        }

        if (passbandRippleDb <= 0.0)
        {
            throw new ArgumentOutOfRangeException(nameof(passbandRippleDb), "Passband ripple must be positive.");
        } 

        var epsilon = Math.Sqrt(Math.Pow(10, passbandRippleDb / 10.0) - 1);
        var sinhAsinh = FilterTools.Asinh(1 / epsilon) / filterOrder;

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

        double gain;
        if (filterOrder % 2 != 0)
        {
            gain = product.Magnitude;
        }
        else
        {
            gain = (1.0 / Math.Sqrt(1 + epsilon * epsilon)) * product.Magnitude;
        }

        return new Zpk([], poles.ToArray(), gain);
    }

    /// <summary>
    /// Calculate IIR Filter transferFunction coefficients
    /// As in MATLAB => [z,p,k] = cheby1(n, Rp, Wp)
    ///
    /// In contrast to matlab this method does not require to normalize the frequencies before calling.
    /// If BandPass or BandStop is selected the actual order of the filter created is double that of the specified order.
    /// </summary>
    /// <param name="frequencyFilterType"></param>
    /// <param name="freqSampling"></param>
    /// <param name="freqLowCutOff">The LowCutofff is not used if 'filterType' is 'HighPass'.</param>
    /// <param name="freqHighCutOff">The HighCutOff is not used if 'filterType' is 'LowPass'.</param>
    /// <param name="filterOrder"></param>
    /// <param name="passbandRippleDb">Max ripple for the passband in db.</param>
    /// <returns> zeros(z), poles(p) and gain(k) for the specified filter settings </returns>
    public static Zpk CalcZpk(FrequencyFilterType frequencyFilterType,
        double freqSampling, double freqLowCutOff, double freqHighCutOff, int filterOrder, double passbandRippleDb)
    {
        FilterTools.FrequencyVerification(frequencyFilterType, freqSampling, freqLowCutOff, freqHighCutOff);
        var lowPassPrototype = PrototypeAnalogLowPass(filterOrder, passbandRippleDb);
        var zpk = FilterTools.CalcFilterSettings(frequencyFilterType, freqSampling, freqLowCutOff, freqHighCutOff, filterOrder, lowPassPrototype);
        return zpk;
    }

    /// <summary>
    /// Calculate IIR Filter transferFunction coefficients
    /// As in MATLAB => [b,a] = cheby1(n, Rp, Wp)
    ///
    /// In contrast to matlab this method does not require to normalize the frequencies before calling.
    /// If BandPass or BandStop is selected the actual order of the filter created is double that of the specified order.
    /// </summary>
    /// <param name="frequencyFilterType"></param>
    /// <param name="freqSampling"></param>
    /// <param name="freqLowCutOff">The LowCutofff is not used if 'filterType' is 'HighPass'.</param>
    /// <param name="freqHighCutOff">The HighCutOff is not used if 'filterType' is 'LowPass'.</param>
    /// <param name="filterOrder"></param>
    /// <param name="passbandRippleDb">Max ripple for the passband in db.</param>
    /// <returns> numerator(b) and denominator(a) for the specified filter settings </returns>
    public static TransferFunction CalcTransferFunction(FrequencyFilterType frequencyFilterType,
        double freqSampling, double freqLowCutOff, double freqHighCutOff, int filterOrder, double passbandRippleDb)
    {
        var zpk = CalcZpk(frequencyFilterType, freqSampling, freqLowCutOff, freqHighCutOff, filterOrder, passbandRippleDb);
        var tf = FilterTools.Zpk2Tf(zpk);
        return tf;
    }
}
