using DspFiltersNet.Filter;

namespace DspFiltersNet.FilterImplementations;

internal class FrequencyFilterInstance : FilterInstanceBase
{
    private readonly double[] inputSamples; // Input samples (mostly called x)
    private readonly double[] outputSamples; // Output samples (mostly called y)
    private readonly IReadOnlyList<double> denominator; // Denominator of transfer function (sometimes called a)
    private readonly IReadOnlyList<double> numerator; // Numerator of transfer function (sometimes called b)
    private readonly int size;
    private bool reset;

    public FrequencyFilterInstance(IFilterDefinition filterDefinition)
    {
        TransferFunction tf;

        if (filterDefinition is ButterworthFilterDefinition butterworthFilterDefinition)
        {
            tf = Butterworth.CalcTransferFunction(butterworthFilterDefinition.FilterType, butterworthFilterDefinition.SamplingFrequency,
                butterworthFilterDefinition.CutoffFrequencyLow, butterworthFilterDefinition.CutoffFrequencyHigh, butterworthFilterDefinition.FilterOrder);
        }
        else if (filterDefinition is BesselFilterDefinition besselFilterDefinition)
        {
            tf = Bessel.CalcTransferFunction(besselFilterDefinition.FilterType, besselFilterDefinition.SamplingFrequency, 
                besselFilterDefinition.CutoffFrequencyLow, besselFilterDefinition.CutoffFrequencyHigh, besselFilterDefinition.FilterOrder);
        }
        else if (filterDefinition is ChebyshevTypeOneFilterDefinition cheby1FilterDefinition)
        {
            tf = ChebyshevI.CalcTransferFunction(cheby1FilterDefinition.FilterType, cheby1FilterDefinition.SamplingFrequency,
                cheby1FilterDefinition.CutoffFrequencyLow, cheby1FilterDefinition.CutoffFrequencyHigh, cheby1FilterDefinition.FilterOrder, 
                cheby1FilterDefinition.PassbandRipple);
        }
        else if (filterDefinition is ChebyshevTypeTwoFilterDefinition cheby2FilterDefinition)
        {
            tf = ChebyshevII.CalcTransferFunction(cheby2FilterDefinition.FilterType, cheby2FilterDefinition.SamplingFrequency,
                cheby2FilterDefinition.CutoffFrequencyLow, cheby2FilterDefinition.CutoffFrequencyHigh, cheby2FilterDefinition.FilterOrder, 
                cheby2FilterDefinition.StopbandAttenuation);
        }
        else if (filterDefinition is EllipticFilterDefinition ellipticFilterDefinition)
        {
            tf = Elliptic.CalcTransferFunction(ellipticFilterDefinition.FilterType, ellipticFilterDefinition.SamplingFrequency,
                ellipticFilterDefinition.CutoffFrequencyLow, ellipticFilterDefinition.CutoffFrequencyHigh, ellipticFilterDefinition.FilterOrder, 
                ellipticFilterDefinition.PassbandRipple, ellipticFilterDefinition.StopbandAttenuation);
        }
        else
        {
            throw new NotSupportedException($"Unsupported filter definition type: {filterDefinition.GetType().Name}");
        }

        numerator = tf.B.ToList().AsReadOnly();
        denominator = tf.A.ToList().AsReadOnly();
        size = denominator.Count;
        inputSamples = new double[size];
        outputSamples = new double[size];
        reset = true;
    }
    
    public override void ResetCalculation()
    {
        reset = true;
    }
    
    public override double Process(double sample)
    {
        if (!CheckDouble(sample))
        {
            ResetCalculation();
            return double.NaN;
        }

        if (reset)
        {
            InitFilterMemory(sample);
            reset = false;
        }
        
        for (var i = size - 1; i > 0; i--)
        {
            inputSamples[i] = inputSamples[i - 1];
            outputSamples[i] = outputSamples[i - 1];
        }
        inputSamples[0] = sample;

        var newOut = numerator[0] * inputSamples[0];
        for (var i = 1; i < size; i++)
        {
            newOut += numerator[i] * inputSamples[i] - denominator[i] * outputSamples[i];
        }
        
        outputSamples[0] = newOut;
        
        return outputSamples[0];
    }

    private void InitFilterMemory(double value)
    {
        for (var i = 0; i < inputSamples.Length; i++)
        {
            inputSamples[i] = value;
            outputSamples[i] = value;
        }
    }
}