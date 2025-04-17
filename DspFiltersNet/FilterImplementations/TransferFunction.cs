namespace DspFiltersNet.FilterImplementations;

public class TransferFunction
{
    public double[] B { get; } //Numerator
    public double[] A { get; } //Denominator
    
    public TransferFunction(double[] b, double[] a)
    {
        B = b;
        A = a;
    }
}