using DspFiltersNet.Filter;
using DspFiltersNet.FilterImplementations;

namespace DspFiltersNet.Tests.FilterImplementations;

[TestFixture]
internal class FilterToolsTests
{            
    [TestCase(FrequencyFilterType.LowPass, 1000.0, 0.0, 0.0, TestName = "LowPass frequency not set")]
    [TestCase(FrequencyFilterType.LowPass, 1000.0, 251.0, 0.0, TestName = "LowPass frequency to high")]
    [TestCase(FrequencyFilterType.HighPass, 1000.0, 0.0, 0.0, TestName = "HighPass frequency not set")]
    [TestCase(FrequencyFilterType.HighPass, 1000.0, 0.0, 251.0, TestName = "HighPass frequency to high")]
    [TestCase(FrequencyFilterType.BandPass, 1000.0, 0.0, 10.0, TestName = "BandPass lowCutOffFrequency not set")]
    [TestCase(FrequencyFilterType.BandPass, 1000.0, 10.0, 0.0, TestName = "BandPass highCutOffFrequency not set")]
    [TestCase(FrequencyFilterType.BandPass, 1000.0, 251.0, 0.0, TestName = "BandPass lowCutOffFrequency to high")]
    [TestCase(FrequencyFilterType.BandPass, 1000.0, 0.0, 251.0, TestName = "BandPass highCutOffFrequency to high")]
    [TestCase(FrequencyFilterType.BandPass, 1000.0, 25.0, 10.0, TestName = "BandPass lowCutOffFrequency > highCutOffFrequency")]
    [TestCase(FrequencyFilterType.BandStop, 1000.0, 0.0, 10.0, TestName = "BandStop lowCutOffFrequency not set")]
    [TestCase(FrequencyFilterType.BandStop, 1000.0, 10.0, 0.0, TestName = "BandStop highCutOffFrequency not set")]
    [TestCase(FrequencyFilterType.BandStop, 1000.0, 251.0, 0.0, TestName = "BandStop lowCutOffFrequency to high")]
    [TestCase(FrequencyFilterType.BandStop, 1000.0, 0.0, 251.0, TestName = "BandStop highCutOffFrequency to high")]
    [TestCase(FrequencyFilterType.BandStop, 1000.0, 25.0, 10.0, TestName = "BandStop lowCutOffFrequency > highCutOffFrequency")]
    public void CalcWithInvalidFrequencySettings(FrequencyFilterType frequencyFilterType, double sourceFrequency, double lowCutOff, double highCutoff)
    {
        //Arrange

        //Act + Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => FilterTools.FrequencyVerification(frequencyFilterType, sourceFrequency, lowCutOff, highCutoff));
    }
}