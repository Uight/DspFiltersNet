using DspFiltersNet.Filter;
using System.Numerics;

namespace DspFiltersNet.FilterImplementations;

/// <summary>
/// This class is for designing IIR digital filters of all orders using the bi-linear transform and a Zpk2Tf function.
/// Additional Resources: http://en.wikipedia.org/wiki/Butterworth_filter
///                       https://docs.scipy.org/doc/scipy/reference/signal.html
///                       https://uk.mathworks.com/help/signal/ref/butter.html
/// </summary>
internal static class FilterTools
{
    public static void FrequencyVerification(FrequencyFilterType frequencyFilterType, double freqSampling, double freqLowCutOff, double freqHighCutOff)
    {
        var freqSamplingQuarter = freqSampling / 4;

        void ValidateCutoff(string paramName, double value)
        {
            if (value <= 0 || value > freqSamplingQuarter)
            {
                throw new ArgumentOutOfRangeException(paramName, value,
                    $"The {paramName} must be greater than zero and less than or equal to samplingFrequency / 4 = {freqSamplingQuarter}.");
            }
        }

        switch (frequencyFilterType)
        {
            case FrequencyFilterType.LowPass:
                ValidateCutoff(nameof(freqLowCutOff), freqLowCutOff);
                break;
            case FrequencyFilterType.HighPass:
                ValidateCutoff(nameof(freqHighCutOff), freqHighCutOff);
                break; 
            case FrequencyFilterType.BandPass:
            case FrequencyFilterType.BandStop:
                ValidateCutoff(nameof(freqLowCutOff), freqLowCutOff);
                ValidateCutoff(nameof(freqHighCutOff), freqHighCutOff);
                if (freqLowCutOff >= freqHighCutOff)
                {
                    throw new ArgumentOutOfRangeException(nameof(freqLowCutOff), freqLowCutOff,
                        $"The {nameof(freqLowCutOff)} must be less than {nameof(freqHighCutOff)} (currently {freqHighCutOff}).");
                }
                break;
        }
    }
    
    public static (Zpk zpk, TransferFunction tf) CalcFilterSettings(FrequencyFilterType frequencyFilterType, double freqSampling, double freqLowCutOff, double freqHighCutOff, 
        int filterOrder, List<Complex> lowPassPrototypePoles, double lowPassPrototypeGain = 1.0, List<Complex>? lowPassPrototypeZeros = null)
    {
        // Pre-warp frequencies
        var warpedLow = 2 * Math.Tan(Math.PI * freqLowCutOff / freqSampling);
        var warpedHigh = 2 * Math.Tan(Math.PI * freqHighCutOff / freqSampling);

        // A lowPass prototype has no zeros so only poles are set.
        var poles = lowPassPrototypePoles;
        var zeros = lowPassPrototypeZeros ?? [];

        //Initialize gain with 1 as the gain for a lowPass prototype is 1.
        var gain = 1.0;

        // Convert analogue lowPass prototype to target filter type
        switch (frequencyFilterType)
        {
            case FrequencyFilterType.LowPass:
                gain = Convert2LowPass(warpedLow, ref poles, ref zeros, lowPassPrototypeGain);
                break;
            case FrequencyFilterType.HighPass:
                gain = Convert2HighPass(warpedHigh, ref poles, ref zeros, lowPassPrototypeGain);
                break;
            case FrequencyFilterType.BandPass:
                gain = Convert2BandPass(warpedLow, warpedHigh, ref poles, ref zeros, lowPassPrototypeGain);
                filterOrder *= 2;
                break;
            case FrequencyFilterType.BandStop:
                gain = Convert2BandStop(warpedLow, warpedHigh, ref poles, ref zeros, lowPassPrototypeGain);
                filterOrder *= 2;
                break;
        }

        // Map zeros & poles from S-plane to Z-plane
        var preBltGain = gain;
        S2Z(ref gain, ref poles, ref zeros);
        
        // Fill zeros with -1 to match size of poles
        for(var i = zeros.Count; i < filterOrder; i++)
        {
            zeros.Add(new Complex(-1.0, 0.0));
        }
        
        var overallGain = preBltGain * (preBltGain / gain); //For highPass and bandStop preBltGain is 1. This simplifies the formula to 1 / gain
        
        Zpk2Tf(zeros, poles, overallGain, out var a, out var b);
        var zpk = new Zpk(zeros.ToArray(), poles.ToArray(), overallGain);
        var tf = new TransferFunction(b, a);
        return (zpk, tf);
    }

    public static double Asinh(double x) => Math.Log(x + Math.Sqrt(x * x + 1));

    /// <summary>
    /// Converts an analog lowpass prototype (with poles, optional zeros, and optional gain)
    /// to a lowpass filter with a specified cutoff frequency.
    /// using: lowPassPole = wc * lowPassPT_Pole
    /// </summary>
    private static double Convert2LowPass(
        double lowCutOff,
        ref List<Complex> poles,
        ref List<Complex> zeros,
        double prototypeGain)
    {
        var wc = lowCutOff;

        for (var i = 0; i < poles.Count; i++)
        {
            poles[i] *= wc;
        }

        for (int i = 0; i < zeros.Count; i++)
        {
            zeros[i] *= wc;
        }

        var gain = prototypeGain * Math.Pow(wc, poles.Count - zeros.Count);

        return gain;
    }

    /// <summary>
    /// Convert analog lowPass prototype poles to highPass poles and generate zeros
    /// using:  highPassPole = wc / lowPassPT_Pole
    /// </summary>
    private static double Convert2HighPass(
        double highCutOff,
        ref List<Complex> poles,
        ref List<Complex> zeros,
        double prototypeGain)
    {
        var wc = highCutOff;
        var prototypesZeroCount = zeros.Count; // Is zero for butterworth, bessel, and chebyshev type 1

        var gain = 1.0;
        // Check for chebyshev type 1 filter
        if (prototypesZeroCount == 0 && prototypeGain != 1.0)
        {
            gain = ChebyshevI.AdjustHighpassAndBandstopGain(prototypeGain, poles);
        }

        // Convert LP poles to HP
        for (var i = 0; i < poles.Count; i++)
        {
            if (Complex.Abs(poles[i]) > 0.0)
            {
                poles[i] = new Complex(wc, 0.0) / poles[i];
            }
        }

        // Convert LP zeros to HP
        for (int i = 0; i < zeros.Count; i++)
        {
            if (Complex.Abs(zeros[i]) > 0.0)
            {
                zeros[i] = new Complex(wc, 0.0) / zeros[i];
            }
        }

        for (var i = zeros.Count; i < poles.Count; i++)
        {
            zeros.Add(new Complex(0.0, 0.0));
        }

        return gain;
    }

    /// <summary>
    /// Convert analog lowPass prototype poles to bandPass poles and generate zeros
    /// using:  bandPassPoles = 0.5 * bw * lowPassPT_Pole +- 0.5 * sqrt( bw^2 / lowPassPT_Pole^2 - 4 * wc^2 )
    /// </summary>
    private static double Convert2BandPass(
        double lowCutOff,
        double highCutOff,
        ref List<Complex> poles,
        ref List<Complex> zeros,
        double prototypeGain)
    {
        var bandWidth = highCutOff - lowCutOff;
        var wc = Math.Sqrt(highCutOff * lowCutOff);
        
        // Calculate bandpass gain
        var gain = prototypeGain * Math.Pow(bandWidth, poles.Count);

        // Init zeros
        zeros = new List<Complex>();
        for (var i = 0; i < poles.Count; i++)
        {
            zeros.Add(new Complex(0.0, 0.0));
        }
        
        // Convert lowPass poles to bandPass poles
        var tempPoles = new List<Complex>();
        foreach (var pole in poles)
        {
            if (Complex.Abs(pole) > 0)
            {
                var firstTerm = pole * bandWidth * 0.5;
                var secondTerm = 0.5 * Complex.Sqrt(Math.Pow(bandWidth, 2) * Complex.Pow(pole, 2) - 4 * Math.Pow(wc, 2));
                tempPoles.Add(firstTerm + secondTerm);
                tempPoles.Add(firstTerm - secondTerm); // Complex conjugate
            }
        }
        
        poles = tempPoles;
        
        return gain;
    }
    
    /// <summary>
    /// Convert analog lowPass prototype poles to bandStop poles and generate zeros
    /// using:  bandStopPoles = 0.5 * bw / lowPassPT_Pole +- 0.5 * sqrt( bw^2 / lowPassPT_Pole^2 - 4 * wc^2 )
    /// </summary>
    private static double Convert2BandStop(
        double lowCutOff,
        double highCutOff,
        ref List<Complex> poles,
        ref List<Complex> zeros,
        double prototypeGain)
    {
        var bandWidth = highCutOff - lowCutOff;
        var wc = Math.Sqrt(highCutOff * lowCutOff);

        var prototypesZeroCount = zeros.Count; // Is zero for butterworth, bessel, and chebyshev type 1

        var gain = 1.0;
        // Check for chebyshev type 1 filter
        if (prototypesZeroCount == 0 && prototypeGain != 1.0)
        {
            gain = ChebyshevI.AdjustHighpassAndBandstopGain(prototypeGain, poles);
        }

        // Calc band stop zeros
        zeros = new List<Complex>();
        for (var i = 0; i < poles.Count; i++)
        {
            zeros.Add(new Complex(0.0, wc));
            zeros.Add(new Complex(0.0, -wc)); // Complex conjugate
        }
        
        // Calc band stop poles
        var tempPoles = new List<Complex>();
        foreach (var pole in poles)
        {
            if (Complex.Abs(pole) > 0)
            {
                var firstTerm = 0.5 * bandWidth / pole;
                var secondTerm = 0.5 * Complex.Sqrt(Math.Pow(bandWidth, 2) / Complex.Pow(pole, 2) - 4 * Math.Pow(wc, 2));
                tempPoles.Add(firstTerm + secondTerm);
                tempPoles.Add(firstTerm - secondTerm); // Complex conjugate
            }
        }
        
        poles = tempPoles;
        return gain;
    }
    
    /// <summary>
    /// z = (2 + s) / (2 - s) is bilinear transform to coonvert the S-plane to Z-plane
    /// Additional Resources: http://en.wikipedia.org/wiki/Bilinear_transform
    /// </summary>
    /// <param name="sz">Ref value that is converted from S-plane to Z-plane.</param>
    /// <returns>Gain</returns>
    private static double BiLinearTransformation(ref Complex sz)
    {
        var complexTwo = new Complex(2.0, 0);
        var s = new Complex(sz.Real, sz.Imaginary);
        sz = (complexTwo + s) / (complexTwo - s);
        
        return Complex.Abs(complexTwo - s);
    }

    /// <summary>
    /// Convert poles & zeros from S-plane to Z-plane via bi-linear transform (BLT)
    /// </summary>
    /// <param name="gain"></param>
    /// <param name="poles"></param>
    /// <param name="zeros"></param>
    private static void S2Z(ref double gain, ref List<Complex> poles, ref List<Complex> zeros)
    {
        // blt zeros
        for(var i = 0; i < zeros.Count; i++)
        {
            var zero = zeros[i];
            gain /= BiLinearTransformation(ref zero);
            zeros[i] = zero;
        }
    
        // blt poles
        for(var i = 0; i < poles.Count; i++)
        {
            var pole = poles[i];
            gain *= BiLinearTransformation(ref pole);
            poles[i] = pole;
        }
    }

    /// <summary>
    /// Convert poles, zeros and gain to transfer function.
    /// As in MATLAB => zp2tf(z,p,k)
    /// As in Math.js => zpk2tf(z,p,k)
    /// </summary>
    private static void Zpk2Tf(List<Complex> z, List<Complex> p, double k, out double[] a, out double[] b)
    {
        var num = new List<Complex> { new Complex(1, 0) };
        var den = new List<Complex> { new Complex(1, 0) };

        foreach (var zero in z)
        {
            num = Multiply(num, new List<Complex> { new Complex(1, 0), -zero });
        }

        foreach (var pole in p)
        {
            den = Multiply(den, new List<Complex> { new Complex(1, 0), -pole });
        }

        //Multiply numerator with gain
        for (var i = 0; i < num.Count; i++)
        {
            num[i] *= k;
        }

        a = den.Select(x => x.Real).ToArray();
        b = num.Select(x => x.Real).ToArray();
    }

    private static List<Complex> Multiply(List<Complex> a, List<Complex> b)
    {
        var c = new List<Complex>(new Complex[a.Count + b.Count - 1]);

        for (var i = 0; i < c.Count; i++)
        {
            c[i] = new Complex(0, 0);
            for (var j = 0; j < a.Count; j++)
            {
                if (i - j >= 0 && i - j < b.Count)
                {
                    c[i] += a[j] * b[i - j];
                }
            }
        }

        return c;
    }
}