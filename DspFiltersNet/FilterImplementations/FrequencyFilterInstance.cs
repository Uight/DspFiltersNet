using DspFiltersNet.Filter;
using System.ComponentModel;

namespace DspFiltersNet.FilterImplementations;

internal class FrequencyFilterInstance : FilterInstanceBase
{
    private readonly double[] inputSamples; // Input samples (mostly called x)
    private readonly double[] outputSamples; // Output samples (mostly called y)
    private readonly IReadOnlyList<double> denominator; // Denominator of transfer function (sometimes called a)
    private readonly IReadOnlyList<double> numerator; // Numerator of transfer function (sometimes called b)
    private readonly int size;
    private bool reset;

    public FrequencyFilterInstance(FrequencyFilterDefinition filterData)
    {
        TransferFunction tf;
        switch (filterData.FilterDesignType)
        {
            case FrequencyFilterDesignType.Butterworth:
                tf = Butterworth.CalcTransferFunction(filterData.FilterType, filterData.SamplingFrequency, filterData.CutoffFrequencyLow, filterData.CutoffFrequencyHigh, filterData.FilterOrder);
                break;
            case FrequencyFilterDesignType.Bessel:
                tf = Bessel.CalcTransferFunction(filterData.FilterType, filterData.SamplingFrequency, filterData.CutoffFrequencyLow, filterData.CutoffFrequencyHigh, filterData.FilterOrder);
                break;
            default:
                throw new InvalidEnumArgumentException(nameof(filterData.FilterDesignType), (int)filterData.FilterDesignType, typeof(FrequencyFilterDesignType));
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