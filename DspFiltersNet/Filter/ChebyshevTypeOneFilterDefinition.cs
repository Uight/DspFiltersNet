namespace DspFiltersNet.Filter;

public class ChebyshevTypeOneFilterDefinition : IFilterDefinition
{
    /// <summary>
    /// FilterType of the filter. {Lowpass, Highpass, Bandpass or Bandstop}
    /// </summary>
    public FrequencyFilterType FilterType { get; }
    /// <summary>
    /// Low cutoff frequency in Hz. Needed for Lowpass, bandpass and bandstop filter types
    /// </summary>
    public double CutoffFrequencyLow { get; }
    /// <summary>
    /// High cutoff frequency in Hz. Needed for Highpass, bandpass and bandstop filter types
    /// </summary>
    public double CutoffFrequencyHigh { get; }
    public int FilterOrder { get; }
    /// <summary>
    /// Sampling frequency in Hz
    /// </summary>
    public double SamplingFrequency { get; }
    /// <summary>
    /// Passband ripple in db
    /// </summary>
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