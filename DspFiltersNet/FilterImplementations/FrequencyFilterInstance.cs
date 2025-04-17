using DspFiltersNet.Filter;

namespace DspFiltersNet.FilterImplementations;

internal class FrequencyFilterInstance : FilterInstanceBase
{
    private readonly double[] inputSamples; // Input samples (mostly called x)
    private readonly double[] outputSamples; // Output samples (mostly called y)
    private readonly double[] denominator; // Denominator of transfer function (sometimes called a)
    private readonly double[] numerator; // Numerator of transfer function (sometimes called b)
    private readonly int size;
    private bool reset;

    public FrequencyFilterInstance(FrequencyFilterDefinition filterData)
    {
        var tf = FilterTools.CalcTransferFunction(filterData.FilterDesignType, filterData.FilterType, filterData.SamplingFrequency, filterData.CutoffFrequencyLow, filterData.CutoffFrequencyHigh, filterData.FilterOrder);
        
        numerator = tf.B;
        denominator = tf.A;
        size = denominator.Length;
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