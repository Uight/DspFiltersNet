﻿namespace DspFiltersNet.Filter;

public class BesselFilterDefinition : IFilterDefinition
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

    public BesselFilterDefinition(FrequencyFilterType filterType, double cutoffFrequencyLow, double cutoffFrequencyHigh, int filterOrder, double samplingFrequency)
    {
        FilterType = filterType;
        CutoffFrequencyLow = cutoffFrequencyLow;
        CutoffFrequencyHigh = cutoffFrequencyHigh;
        FilterOrder = filterOrder;
        SamplingFrequency = samplingFrequency;
    }
}