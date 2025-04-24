namespace DspFiltersNet.FilterImplementations;

internal class TransferFunction
{
    public IReadOnlyCollection<double> B { get; } //Numerator
    public IReadOnlyCollection<double> A { get; } //Denominator
    
    public TransferFunction(double[] b, double[] a)
    {
        B = b.AsReadOnly();
        A = a.AsReadOnly();
    }
}