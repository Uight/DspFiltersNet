namespace DspFiltersNet.Filter;

public class ChebyshevTypeTwoFilterDefinition : IFilterDefinition
{
    public FrequencyFilterType FilterType { get; }
    public double CutoffFrequencyLow { get; }
    public double CutoffFrequencyHigh { get; }
    public int FilterOrder { get; }
    public double SamplingFrequency { get; }
    public double StopbandRipple { get; }

    public ChebyshevTypeTwoFilterDefinition(FrequencyFilterType filterType, double cutoffFrequencyLow, double cutoffFrequencyHigh, int filterOrder, double samplingFrequency, double stopbandRipple)
    {
        FilterType = filterType;
        CutoffFrequencyLow = cutoffFrequencyLow;
        CutoffFrequencyHigh = cutoffFrequencyHigh;
        FilterOrder = filterOrder;
        SamplingFrequency = samplingFrequency;
        StopbandRipple = stopbandRipple;
    }
}