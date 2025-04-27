# DspFiltersNet

[![Continuous Integration](https://github.com/Uight/DspFiltersNet/actions/workflows/ci.yml/badge.svg)](https://github.com/Uight/DspFiltersNet/actions/workflows/ci.yml)
[![](https://img.shields.io/nuget/vpre/DspFiltersNet?color=%23004880&label=NuGet&logo=NuGet)](https://www.nuget.org/packages/DspFiltersNet/)
[![GitHub](https://img.shields.io/github/license/uight/dspfiltersnet?color=%231281c0)](LICENSE)

**DspFiltersNet** is a digital signal processing library for **.NET 8.0**.

It allows the creation of the following FilterTypes:

1. MovingAverage
2. FrequencyFilters

For FrequencyFilters it allows the creation of the following filter design types:

1. Butterworth
2. Bessel
3. Chebyshev Type I
4. Chebyshev Type II
5. Elliptic

The package supports the creation of Lowpass, Highpass, Bandpass and Bandstop filters up to order 16.

The package only supports Filters and does not have other features. For more DSP features in C# you 
may want to check out [![](https://raw.githubusercontent.com/ar1st0crat/NWaves/master/assets/logo/logo_draft.bmp)](NWaves)

### Example Usage

```csharp
using DspFiltersNet;

class Program
{
    static void Main()
    {
        // Define a butterworth lowpass of second order with a cutoff frequency of 10Hz and a sampling frequency of 1kHz.
        var filterData = new ButterworthFilterDefinition(FrequencyFilterType.LowPass, 10, 0, 2, 1000);
        // Create your filter instance
        var filterInstance = new FrequencyFilterInstance(filterData);
        // Use the filter instance to process your samples (single sample mode also available)
        var result = filterInstance.ProcessSamples([0.0000000000000000D, 0.5941312281110626D, 0.9637482198843191D, 0.9700934515686609D, 0.6131666751454520D]);
    }
}
```

### Remarks

> [!IMPORTANT]
> The frequency filters provided by this implementation use the transfer functions to filter your data.
> According to multiple documentations this can cause some stability problems and using SOS (second order sections) 
> would be better but is currently not implemented

> [!IMPORTANT]
> This library just creates filters based on the users input parameters. The user is still responsible to
> select plausible filter settings that work nice with the provided data.

### Internal function

The package first creates an analog lowpass prototype of the selected settings similar to the matlab functions
besselap, butterap, cheb1ap and cheb2ap. After that the lowpass prototype is converted to the selected filter (Lowpass, Highpass, Bandpass, Bandstop).
This all works on the Zeros, Poles and Gain notation. After that the filter is transformed to a digital filter
using the bilinear transform. In the end the Zeros, Poles and Gain returned from the bilinear transform are converted
to a transfer function which is then used to in the filter instance to filter user data.

### Validation

The code is validated with UnitTests against testdata generated with Matlab and with SciPy and should match these implementations pretty much perfectly (1e-14).
