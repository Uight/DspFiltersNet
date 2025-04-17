using DspFiltersNet.Filter;
using DspFiltersNet.FilterImplementations;

namespace DspFiltersNet;

public class DspFilter
{
    private FilterInstanceBase filterInstance;

    public DspFilter(IFilterDefinition filterDefinition)
    {

    }

    public void ResetCalculation()
    {
        filterInstance.ResetCalculation();
    }

    public double[] ProcessSamples(double[] samples)
    {
        return filterInstance.ProcessSamples(samples);
    }

    public double Process(double sample)
    {
        return filterInstance.Process(sample);
    }
}
