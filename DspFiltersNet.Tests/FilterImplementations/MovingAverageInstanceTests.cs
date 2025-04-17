using DspFiltersNet.Filter;
using DspFiltersNet.FilterImplementations;

namespace DspFiltersNet.Tests.FilterImplementations;

[TestFixture]
internal class MovingAverageInstanceTests
{
    private static readonly double[] ExpectedValuesNormal = [1.0, 1.5, 2.0, 3.0, 4.0];
    private static readonly double[] ExpectedValuesWithNaN = [1.0, 1.5, 2.0, 3.0, 4.0, double.NaN, 1.0, 1.5, 2.0, 3.0, 4.0];
    
    [Test]
    public void MovingAverageFilter_Process_CalculatesCorrectAverage()
    {
        var filterData = new MovingAverageFilterDefinition(3);
        var filterInstance = new MovingAverageInstance(filterData);
        
        var result1 = filterInstance.Process(1.0);
        var result2 = filterInstance.Process(2.0);
        var result3 = filterInstance.Process(3.0);
        var result4 = filterInstance.Process(4.0);
        var result5 = filterInstance.Process(5.0);
        
        Assert.Multiple(() =>
        {
            Assert.That(result1, Is.EqualTo(1.0));
            Assert.That(result2, Is.EqualTo(1.5));
            Assert.That(result3, Is.EqualTo(2.0));
            Assert.That(result4, Is.EqualTo(3.0));
            Assert.That(result5, Is.EqualTo(4.0));
        });
    }
    
    [Test]
    public void MovingAverageFilter_ProcessCurve_CalculatesCorrectAverage()
    {
        var filterData = new MovingAverageFilterDefinition(3);
        var filterInstance = new MovingAverageInstance(filterData);

        var result = filterInstance.ProcessSamples([1.0, 2.0, 3.0, 4.0, 5.0]);
        Assert.That(result, Is.EqualTo(ExpectedValuesNormal));
    }
    
    [Test]
    public void MovingAverageFilter_ProcessCurveWithNaN_CalculatesCorrectAverage()
    {
        var filterData = new MovingAverageFilterDefinition(3);
        var filterInstance = new MovingAverageInstance(filterData);

        var result = filterInstance.ProcessSamples([1.0, 2.0, 3.0, 4.0, 5.0, double.NaN, 1.0, 2.0, 3.0, 4.0, 5.0]);
        Assert.That(result, Is.EqualTo(ExpectedValuesWithNaN));
    }
    
    [Test]
    public void MovingAverageFilter_ResetFilter()
    {
        var filterData = new MovingAverageFilterDefinition(3);
        var filterInstance = new MovingAverageInstance(filterData);
        
        filterInstance.Process(1.0);
        filterInstance.Process(2.0);
        filterInstance.Process(3.0);
        
        filterInstance.ResetCalculation();
        
        Assert.That(filterInstance.Process(1.0), Is.EqualTo(1.0));
    }

    [TestCase(0)]
    public void MovingAverageFilter_InvalidWidth_ThrowsException(int width)
    {
        var invalidFilterData = new MovingAverageFilterDefinition(width);
        Assert.Throws<ArgumentOutOfRangeException>(() => { _ = new MovingAverageInstance(invalidFilterData); });
    }
}