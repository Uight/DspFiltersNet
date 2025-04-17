using DspFiltersNet.Filter;

namespace DspFiltersNet.FilterImplementations;

internal class MovingAverageInstance : FilterInstanceBase
{
    private readonly MovingAverageFilterDefinition settings;
    private readonly Queue<double> values;
    private double sum;

    public MovingAverageInstance(MovingAverageFilterDefinition filterConfig)
    {
        settings = filterConfig;
        
        if (settings.Width <= 1)
        {
            throw new ArgumentOutOfRangeException(nameof(filterConfig.Width));
        }
        values = new Queue<double>(settings.Width);
        sum = 0;
    }
    
    public int GetCurrentCount()
    {
        return values.Count;
    }
    
    public override void ResetCalculation()
    {
        values.Clear();
        sum = 0;
    }

    public override double Process(double sample)
    {
        if (!CheckDouble(sample))
        {
            ResetCalculation();
            return double.NaN;
        }
        
        if (values.Count == settings.Width)
        {
            sum -= values.Dequeue();
        }
        
        values.Enqueue(sample);
        sum += sample;
        
        return sum / values.Count;
    }
}