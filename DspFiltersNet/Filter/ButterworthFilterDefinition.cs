namespace DspFiltersNet.Filter;

public class ButterworthFilterDefinition : IFilterDefinition
{
    public FrequencyFilterType FilterType { get; }
    public double CutoffFrequencyLow { get; }
    public double CutoffFrequencyHigh { get; }
    public int FilterOrder { get; }
    public double SamplingFrequency { get; }

    public ButterworthFilterDefinition(FrequencyFilterType filterType, double cutoffFrequencyLow, double cutoffFrequencyHigh, int filterOrder, double samplingFrequency)
    {
        FilterType = filterType;
        CutoffFrequencyLow = cutoffFrequencyLow;
        CutoffFrequencyHigh = cutoffFrequencyHigh;
        FilterOrder = filterOrder;
        SamplingFrequency = samplingFrequency;
    }
}