namespace DspFiltersNet.Filter;

public class FrequencyFilterDefinition : IFilterDefinition
{
    public FrequencyFilterType FilterType { get; }
    public double CutoffFrequencyLow { get; }
    public double CutoffFrequencyHigh { get; }
    public FrequencyFilterDesignType FilterDesignType { get; }
    public int FilterOrder { get; }

    public FrequencyFilterDefinition(FrequencyFilterType filterType, double cutoffFrequencyLow, double cutoffFrequencyHigh, FrequencyFilterDesignType filterDesignType, int filterOrder)
    {
        FilterType = filterType;
        CutoffFrequencyLow = cutoffFrequencyLow;
        CutoffFrequencyHigh = cutoffFrequencyHigh;
        FilterDesignType = filterDesignType;
        FilterOrder = filterOrder;
    }
}