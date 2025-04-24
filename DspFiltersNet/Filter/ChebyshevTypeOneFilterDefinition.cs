namespace DspFiltersNet.Filter;

public class ChebyshevTypeOneFilterDefinition : IFilterDefinition
{
    public FrequencyFilterType FilterType { get; }
    public double CutoffFrequencyLow { get; }
    public double CutoffFrequencyHigh { get; }
    public int FilterOrder { get; }
    public double SamplingFrequency { get; }
    public double PassbandRipple { get; }

    public ChebyshevTypeOneFilterDefinition(FrequencyFilterType filterType, double cutoffFrequencyLow, double cutoffFrequencyHigh, int filterOrder, double samplingFrequency, double passbandRipple)
    {
        FilterType = filterType;
        CutoffFrequencyLow = cutoffFrequencyLow;
        CutoffFrequencyHigh = cutoffFrequencyHigh;
        FilterOrder = filterOrder;
        SamplingFrequency = samplingFrequency;
        PassbandRipple = passbandRipple;
    }
}