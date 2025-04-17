namespace DspFiltersNet.FilterImplementations;

internal abstract class FilterInstanceBase
{
    public abstract void ResetCalculation();

    public virtual double[] ProcessSamples(double[] samples)
    {
        var outValues = new double[samples.Length];
        for (var i = 0; i < samples.Length; i++)
        {
            outValues[i] = Process(samples[i]);
        }
        return outValues;
    }
    
    public abstract double Process(double sample);

    protected static bool CheckDouble(double value)
    {
        if (double.IsNaN(value) || double.IsPositiveInfinity(value) || double.IsNegativeInfinity(value))
        {
            return false;
        }

        return true;
    }
}