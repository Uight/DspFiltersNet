using System.Numerics;

namespace DspFiltersNet.FilterImplementations;

internal class Zpk
{
    public IReadOnlyCollection<Complex> Z { get; } //Zeros
    public IReadOnlyCollection<Complex> P { get; } //Poles
    public double K { get; } //Gain
    
    public Zpk(Complex[] z, Complex[] p, double k)
    {
        Z = z.AsReadOnly();
        P = p.AsReadOnly();
        K = k;
    }
}