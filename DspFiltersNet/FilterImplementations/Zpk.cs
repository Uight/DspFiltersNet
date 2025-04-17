using System.Numerics;

namespace DspFiltersNet.FilterImplementations;

internal class Zpk
{
    public Complex[] Z { get; } //Zeros
    public Complex[] P { get; } //Poles
    public double K { get; } //Gain
    
    public Zpk(Complex[] z, Complex[] p, double k)
    {
        Z = z;
        P = p;
        K = k;
    }
}