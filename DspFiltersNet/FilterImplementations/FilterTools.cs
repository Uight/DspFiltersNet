using DspFiltersNet.Filter;
using System.ComponentModel;
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
    
    public static (Zpk zpk, TransferFunction tf) CalcFilterSettings(FrequencyFilterType frequencyFilterType, double freqSampling, 
        double freqLowCutOff, double freqHighCutOff, int filterOrder, Zpk lowPassPrototype)
    {
        // Pre-warp frequencies
        var warpedLow = 2 * Math.Tan(Math.PI * freqLowCutOff / freqSampling);
        var warpedHigh = 2 * Math.Tan(Math.PI * freqHighCutOff / freqSampling);

        // Convert analogue lowPass prototype to target filter type
        Zpk convertedZpk;

        switch (frequencyFilterType)
        {
            case FrequencyFilterType.LowPass:
                convertedZpk = Convert2LowPass(warpedLow, lowPassPrototype);
                break;
            case FrequencyFilterType.HighPass:
                convertedZpk = Convert2HighPass(warpedHigh, lowPassPrototype);
                break;
            case FrequencyFilterType.BandPass:
                convertedZpk = Convert2BandPass(warpedLow, warpedHigh, lowPassPrototype);
                filterOrder *= 2;
                break;
            case FrequencyFilterType.BandStop:
                convertedZpk = Convert2BandStop(warpedLow, warpedHigh, lowPassPrototype);
                filterOrder *= 2;
                break;
            default:
                throw new InvalidEnumArgumentException(nameof(frequencyFilterType), (int)frequencyFilterType, typeof(FrequencyFilterType));
        }

        // Map zeros & poles from S-plane to Z-plane
        var preBltGain = convertedZpk.K;
        var zPlaneZpk = S2Z(convertedZpk);

        // Fill zeros with -1 to match size of poles
        var filledZeros = zPlaneZpk.Z.ToList();
        for (var i = filledZeros.Count; i < filterOrder; i++)
        {
            filledZeros.Add(new Complex(-1.0, 0.0));
        }

        var overallGain = preBltGain * (preBltGain / zPlaneZpk.K); //For highPass and bandStop preBltGain is 1. This simplifies the formula to 1 / gain

        var zpk = new Zpk(filledZeros.ToArray(), zPlaneZpk.P.ToArray(), overallGain);
        var tf = Zpk2Tf(zpk);
        return (zpk, tf);
    }

    public static double Asinh(double x) => Math.Log(x + Math.Sqrt(x * x + 1));

    /// <summary>
    /// Converts an analog lowpass prototype (with poles, optional zeros, and optional gain)
    /// to a lowpass filter with a specified cutoff frequency.
    /// using: lowPassPole = wc * lowPassPT_Pole
    /// </summary>
    private static Zpk Convert2LowPass(double lowCutOff, Zpk lowPassPrototype)
    {
        var wc = lowCutOff;

        var poles = lowPassPrototype.P.ToList();
        var zeros = lowPassPrototype.Z.ToList();

        var gain = lowPassPrototype.K * Math.Pow(wc, poles.Count - zeros.Count);

        for (var i = 0; i < poles.Count; i++)
        {
            poles[i] *= wc;
        }

        for (int i = 0; i < zeros.Count; i++)
        {
            zeros[i] *= wc;
        }

        return new Zpk(zeros.ToArray(), poles.ToArray(), gain);
    }

    /// <summary>
    /// Convert analog lowPass prototype poles to highPass poles and generate zeros
    /// using:  highPassPole = wc / lowPassPT_Pole
    /// </summary>
    private static Zpk Convert2HighPass(double highCutOff, Zpk lowPassPrototype)
    {
        var wc = highCutOff;

        var poles = lowPassPrototype.P.ToList();
        var zeros = lowPassPrototype.Z.ToList();

        var gain = HighpassAndBandstopGainCorrection(lowPassPrototype);

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

        return new Zpk(zeros.ToArray(), poles.ToArray(), gain);
    }

    /// <summary>
    /// Convert analog lowPass prototype poles to bandPass poles and generate zeros
    /// using:  bandPassPoles = 0.5 * bw * lowPassPT_Pole +- 0.5 * sqrt( bw^2 / lowPassPT_Pole^2 - 4 * wc^2 )
    /// </summary>
    private static Zpk Convert2BandPass(double lowCutOff, double highCutOff, Zpk lowPassPrototype)
    {
        var bandWidth = highCutOff - lowCutOff;
        var wc = Math.Sqrt(highCutOff * lowCutOff);

        var poles = new List<Complex>();
        var zeros = new List<Complex>();

        // Calculate bandpass gain
        var gain = lowPassPrototype.K * Math.Pow(bandWidth, lowPassPrototype.P.Count - lowPassPrototype.Z.Count);

        // Convert lowPass poles to bandPass poles
        foreach (var pole in lowPassPrototype.P)
        {
            if (Complex.Abs(pole) > 0)
            {
                var firstTerm = pole * bandWidth * 0.5;
                var secondTerm = 0.5 * Complex.Sqrt(Math.Pow(bandWidth, 2) * Complex.Pow(pole, 2) - 4 * Math.Pow(wc, 2));
                poles.Add(firstTerm + secondTerm);
                poles.Add(firstTerm - secondTerm); // Complex conjugate
            }
        }   

        // Convert lowPass zeros to bandPass zeros
        foreach (var zero in lowPassPrototype.Z)
        {
            if (Complex.Abs(zero) > 0)
            {
                var a = 0.5 * bandWidth * zero;
                var b = 0.5 * Complex.Sqrt(Complex.Pow(bandWidth * zero, 2) - 4 * wc * wc);
                zeros.Add(a + b);
                zeros.Add(a - b); // Complex conjugate
            }
        }

        // Fill zeros
        for (var i = lowPassPrototype.Z.Count; i < lowPassPrototype.P.Count; i++)
        {
            zeros.Add(new Complex(0.0, 0.0));
        }

        return new Zpk(zeros.ToArray(), poles.ToArray(), gain);
    }
    
    /// <summary>
    /// Convert analog lowPass prototype poles to bandStop poles and generate zeros
    /// using:  bandStopPoles = 0.5 * bw / lowPassPT_Pole +- 0.5 * sqrt( bw^2 / lowPassPT_Pole^2 - 4 * wc^2 )
    /// </summary>
    private static Zpk Convert2BandStop(double lowCutOff, double highCutOff, Zpk lowPassPrototype)
    {
        var bandWidth = highCutOff - lowCutOff;
        var wc = Math.Sqrt(highCutOff * lowCutOff);

        var poles = new List<Complex>();
        var zeros = new List<Complex>();

        var gain = HighpassAndBandstopGainCorrection(lowPassPrototype);

        // Calc band stop poles
        foreach (var pole in lowPassPrototype.P)
        {
            if (Complex.Abs(pole) > 0)
            {
                var firstTerm = 0.5 * bandWidth / pole;
                var secondTerm = 0.5 * Complex.Sqrt(Math.Pow(bandWidth, 2) / Complex.Pow(pole, 2) - 4 * Math.Pow(wc, 2));
                poles.Add(firstTerm + secondTerm);
                poles.Add(firstTerm - secondTerm); // Complex conjugate
            }
        }

        // Convert zeros
        foreach (var zero in lowPassPrototype.Z)
        {
            if (Complex.Abs(zero) > 0)
            {
                var a = 0.5 * bandWidth / zero;
                var b = 0.5 * Complex.Sqrt(Complex.Pow(bandWidth / zero, 2) - 4 * wc * wc);
                zeros.Add(a + b);
                zeros.Add(a - b); // Complex conjugate
            }
        }

        // Calc band stop zeros
        for (var i = lowPassPrototype.Z.Count; i < lowPassPrototype.P.Count; i++)
        {
            zeros.Add(new Complex(0.0, wc));
            zeros.Add(new Complex(0.0, -wc)); // Complex conjugate
        }

        return new Zpk(zeros.ToArray(), poles.ToArray(), gain);
    }

    /// <summary>
    /// Method that takes the analog lowpass prototype and corrects the gain in a way that
    /// the gain change caused by the inversion in highpass and bandstop conversion is canceled out.
    /// </summary>
    /// <returns>Corrected highpass or bandstop gain.</returns>
    private static double HighpassAndBandstopGainCorrection(Zpk prototypeLowPass)
    {
        var prodP = Complex.One;
        foreach (var pole in prototypeLowPass.P)
        {
            prodP *= -pole;
        }

        var prodZ = Complex.One;
        foreach (var zero in prototypeLowPass.Z)
        {
            prodZ *= -zero;
        }

        return prototypeLowPass.K * (prodZ.Real / prodP.Real);
    }

    /// <summary>
    /// z = (2 + s) / (2 - s) is bilinear transform to convert the S-plane to Z-plane
    /// Additional Resources: http://en.wikipedia.org/wiki/Bilinear_transform
    /// </summary>
    /// <param name="s">Value to convert from S-plane to Z-plane.</param>
    /// <param name="z">Z-plane converted value.</param>
    /// <returns>Gain</returns>
    private static double BiLinearTransformation(Complex s, out Complex z)
    {
        var complexTwo = new Complex(2.0, 0);
        z = (complexTwo + s) / (complexTwo - s);
        
        return Complex.Abs(complexTwo - s);
    }

    /// <summary>
    /// Convert poles & zeros from S-plane to Z-plane via bi-linear transform (BLT)
    /// </summary>
    /// <param name="gain"></param>
    /// <param name="poles"></param>
    /// <param name="zeros"></param>
    private static Zpk S2Z(Zpk sPlaneZpk)
    {
        var gain = sPlaneZpk.K;

        // blt zeros
        var sPlaneZeros = sPlaneZpk.Z.ToList();
        var zPlaneZeros = new List<Complex>();
        for(var i = 0; i < sPlaneZeros.Count; i++)
        {
            gain /= BiLinearTransformation(sPlaneZeros[i], out var zero);
            zPlaneZeros.Add(zero);
        }

        // blt poles
        var sPlanePoles = sPlaneZpk.P.ToList();
        var zPlanePoles = new List<Complex>();
        for (var i = 0; i < sPlanePoles.Count; i++)
        {
            gain *= BiLinearTransformation(sPlanePoles[i], out var pole);
            zPlanePoles.Add(pole);
        }

        return new Zpk(zPlaneZeros.ToArray(), zPlanePoles.ToArray(), gain);
    }

    /// <summary>
    /// Convert poles, zeros and gain to transfer function.
    /// As in MATLAB => zp2tf(z,p,k)
    /// As in Math.js => zpk2tf(z,p,k)
    /// </summary>
    private static TransferFunction Zpk2Tf(Zpk zpk)
    {
        var num = new List<Complex> { new Complex(1, 0) };
        var den = new List<Complex> { new Complex(1, 0) };

        foreach (var zero in zpk.Z)
        {
            num = Multiply(num, new List<Complex> { new Complex(1, 0), -zero });
        }

        foreach (var pole in zpk.P)
        {
            den = Multiply(den, new List<Complex> { new Complex(1, 0), -pole });
        }

        //Multiply numerator with gain
        for (var i = 0; i < num.Count; i++)
        {
            num[i] *= zpk.K;
        }

        var a = den.Select(x => x.Real).ToArray();
        var b = num.Select(x => x.Real).ToArray();

        var tf = new TransferFunction(b, a);
        return tf;
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