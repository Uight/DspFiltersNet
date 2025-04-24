using DspFiltersNet.Filter;
using DspFiltersNet.FilterImplementations;

namespace DspFiltersNet.Tests.FilterImplementations;

//This test only tests the basic function of the filter instance. Not all filter configurations are tested as the configuration itself is tested by the FilterTools Test
//Therefore it is enough to just test the instance with on filter configuration as the other will just work the same

[TestFixture]
internal class FrequencyFilterInstanceTests
{
    private static readonly double[] ExpectedValuesNormal = [0.0000000000000000D, 0.0005612709253673D, 0.0031056862801681D, 0.0087206296400223D, 0.0171477912799752D];
    private static readonly double[] ExpectedValuesWithNaN = [0.0000000000000000D, 0.0005612709253673D, 0.0031056862801681D, 0.0087206296400223D, 0.0171477912799752D, double.NaN, 0.0000000000000000D, 0.0005612709253673D, 0.0031056862801681D, 0.0087206296400223D, 0.0171477912799752D];
    
    [Test]
    public void FrequencyFilterInstance_Process_CalculatesCorrectFiltered()
    {
        var filterData = new ButterworthFilterDefinition(FrequencyFilterType.LowPass, 10, 100, 2, 1000);
        var filterInstance = new FrequencyFilterInstance(filterData);
        
        const double tolerance = 1E-12d;
        
        var result1 = filterInstance.Process(0.0000000000000000D);
        var result2 = filterInstance.Process(0.5941312281110626D);
        var result3 = filterInstance.Process(0.9637482198843191D);
        var result4 = filterInstance.Process(0.9700934515686609D);
        var result5 = filterInstance.Process(0.6131666751454520D);
        
        Assert.Multiple(() =>
        {
            Assert.That(result1, Is.EqualTo(0.0000000000000000D).Within(tolerance));
            Assert.That(result2, Is.EqualTo(0.0005612709253673D).Within(tolerance));
            Assert.That(result3, Is.EqualTo(0.0031056862801681D).Within(tolerance));
            Assert.That(result4, Is.EqualTo(0.0087206296400223D).Within(tolerance));
            Assert.That(result5, Is.EqualTo(0.0171477912799752D).Within(tolerance));
        });
    }
    
    [Test]
    public void FrequencyFilterInstance_ProcessCurve_CalculatesCorrectFiltered()
    {
        var filterData = new ButterworthFilterDefinition(FrequencyFilterType.LowPass, 10, 100, 2, 1000);
        var filterInstance = new FrequencyFilterInstance(filterData);
        
        const double tolerance = 1E-12d;

        var result = filterInstance.ProcessSamples([0.0000000000000000D, 0.5941312281110626D, 0.9637482198843191D, 0.9700934515686609D, 0.6131666751454520D]);
        Assert.That(result, Is.EqualTo(ExpectedValuesNormal).Within(tolerance));
    }
    
        
    [Test]
    public void FrequencyFilterInstance_ProcessCurveWithNaN_CalculatesCorrectFiltered()
    {
        var filterData = new ButterworthFilterDefinition(FrequencyFilterType.LowPass, 10, 100, 2, 1000);
        var filterInstance = new FrequencyFilterInstance(filterData);
        
        const double tolerance = 1E-12d;

        var result = filterInstance.ProcessSamples([0.0000000000000000D, 0.5941312281110626D, 0.9637482198843191D, 0.9700934515686609D, 0.6131666751454520D, double.NaN, 0.0000000000000000D, 0.5941312281110626D, 0.9637482198843191D, 0.9700934515686609D, 0.6131666751454520D]);
        Assert.That(result, Is.EqualTo(ExpectedValuesWithNaN).Within(tolerance));
    }

    [Test]
    public void FrequencyFilterInstance_ResetFilter()
    {
        var filterData = new ButterworthFilterDefinition(FrequencyFilterType.LowPass, 10, 100, 2, 1000);
        var filterInstance = new FrequencyFilterInstance(filterData);

        const double tolerance = 1E-12d;

        filterInstance.Process(0.0000000000000000D);
        filterInstance.Process(0.5941312281110626D);
        filterInstance.Process(0.9637482198843191D);

        filterInstance.ResetCalculation();
        filterInstance.Process(0.0000000000000000D);
        var result = filterInstance.Process(0.5941312281110626D);
        
        Assert.That(result, Is.EqualTo(0.0005612709253673D).Within(tolerance));
    }
    
    [Test]
    public void FrequencyFilterInstance_ResetFilter_WithValueAfterReset()
    {
        var filterData = new ButterworthFilterDefinition(FrequencyFilterType.LowPass, 10, 100, 2, 1000);
        var filterInstance = new FrequencyFilterInstance(filterData);

        const double tolerance = 1E-12d;

        filterInstance.Process(0.0000000000000000D);
        filterInstance.Process(0.5941312281110626D);
        filterInstance.Process(0.9637482198843191D);

        filterInstance.ResetCalculation();
        var result1 = filterInstance.Process(1.0000000000000000D);
        var result2 = filterInstance.Process(1.5941312281110626D);
        var result3 = filterInstance.Process(1.9637482198843191D);
        
        Assert.Multiple(() =>
        {
            Assert.That(result1, Is.EqualTo(1.0D).Within(tolerance));
            Assert.That(result2, Is.EqualTo(1.0005612709253673D).Within(tolerance));
            Assert.That(result3, Is.EqualTo(1.0031056862801681D).Within(tolerance));
        });
    }

    [Test]
    public void FrequencyFilterInstance_InvalidDesignType_ThrowsException()
    {
        var invalidFilterData = new MovingAverageFilterDefinition(10);
        Assert.Throws<NotSupportedException>(() => { _ = new FrequencyFilterInstance(invalidFilterData); });
    }
    
    [Test]
    public void FrequencyFilterInstance_InvalidCutOffFrequenciesSetting_ThrowsException()
    {
        var invalidFilterData = new ButterworthFilterDefinition(FrequencyFilterType.BandPass, 100, 100, 2, 1000);
        Assert.Throws<ArgumentOutOfRangeException>(() => { _ = new FrequencyFilterInstance(invalidFilterData); });
    }
}