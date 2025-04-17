using DspFiltersNet.FilterImplementations;

namespace DspFiltersNetTests;

public class TestClass
{
    public TestClass() 
    {
        var test = Bessel.PrototypeAnalogLowPass(1);
    }
}
