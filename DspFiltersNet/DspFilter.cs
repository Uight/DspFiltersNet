using DspFiltersNet.Filter;
using DspFiltersNet.FilterImplementations;

namespace DspFiltersNet;

public class DspFilter
{
    private FilterInstanceBase filterInstance;

    public DspFilter(IFilterDefinition filterDefinition)
    {
        if (filterDefinition is MovingAverageFilterDefinition movingAverageFilterDefinition)
        {
            filterInstance = new MovingAverageInstance(movingAverageFilterDefinition);
            return;
        }
        if (filterDefinition is FrequencyFilterDefinition frequencyFilterDefinition)
        {
            filterInstance = new FrequencyFilterInstance(frequencyFilterDefinition);
            return;
        }
        throw new NotSupportedException($"Unsupported filter definition type: {filterDefinition.GetType().Name}");
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
