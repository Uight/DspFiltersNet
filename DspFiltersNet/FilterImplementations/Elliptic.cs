// This file contains code adapted from SciPy (https://scipy.org/)
// SciPy is licensed under the BSD 3-Clause License.
// 
// See the SciPy license: https://github.com/scipy/scipy/blob/main/LICENSE.txt
// This code was written on 26.04.2025 and used the license available at that date ( as in release 1.15.2)
// Original license text:

/*
Copyright (c) 2001-2002 Enthought, Inc. 2003-2024, SciPy Developers.
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions
are met:

1. Redistributions of source code must retain the above copyright
   notice, this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above
   copyright notice, this list of conditions and the following
   disclaimer in the documentation and/or other materials provided
   with the distribution.

3. Neither the name of the copyright holder nor the names of its
   contributors may be used to endorse or promote products derived
   from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
"AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using DspFiltersNet.Filter;
using System.Numerics;

namespace DspFiltersNet.FilterImplementations;

internal class Elliptic
{
    /// <summary>
    /// Elliptic filter lowPass prototype.
    /// As in MATLAB => [z,p,k] = ellipap(order, Rp, Rs)
    /// </summary>
    /// <param name="filterOrder"></param>
    /// <param name="passbandRippleDb"></param>
    /// <param name="stopbandAttenuationDb"></param>
    /// <returns> Zeros, poles and gain of the Elliptic analog lowPass filter prototype with the specified settings </returns>
    /// This is a near perfect copy of SciPy python code. To get more info of how it works you might want to check out
    /// the SciPy code itself.
    public static Zpk PrototypeAnalogLowPass(int filterOrder, double passbandRippleDb, double stopbandAttenuationDb)
    {
        if (filterOrder < 1 || filterOrder > 16)
        {
            throw new ArgumentOutOfRangeException(nameof(filterOrder));
        }

        if (passbandRippleDb <= 0.0)
        {
            throw new ArgumentOutOfRangeException(nameof(passbandRippleDb), "Passband ripple must be positive.");
        }

        if (stopbandAttenuationDb <= passbandRippleDb)
        {
            throw new ArgumentOutOfRangeException(nameof(stopbandAttenuationDb), "Stopband attenuation must be greater than ripple.");
        }

        if (filterOrder == 1)
        {
            var p = -1.0 / Math.Sqrt(Pow10Minus1(0.1 * passbandRippleDb));
            return new Zpk([], [new Complex(p, 0)], -p);
        }

        const double EPSILON = 1e-16;

        var eps_sq = Pow10Minus1(0.1 * passbandRippleDb);
        var eps = Math.Sqrt(eps_sq);
        var ck1_sq = eps_sq / Pow10Minus1(0.1 * stopbandAttenuationDb);
        if (ck1_sq == 0)
        {
            throw new ArgumentException("Cannot design filter with given rp and rs");
        }

        var val = EllipK(ck1_sq);

        var m = EllipDeg(filterOrder, ck1_sq);
        var capk = EllipK(m);

        var j = new List<int>();
        for (var i = 1 - (filterOrder % 2); i < filterOrder; i += 2)
        {
            j.Add(i);
        }
        var jj = j.Count;

        var u = j.Select(x => x * capk / filterOrder).ToArray();
        var (sn, cn, dn, am) = EllipJ(u, m);

        // Handle zeros
        var snew = sn.Where(sv => Math.Abs(sv) > EPSILON);

        var zeros = snew.Select(sv => new Complex(0, 1.0 / (Math.Sqrt(m) * sv))).ToArray();

        // Add conjugates
        var zConjugates = zeros.Select(zi => Complex.Conjugate(zi));
        zeros = zeros.Concat(zConjugates).ToArray();

        // Handle poles
        var r = ArcJacSc1(1.0 / eps, ck1_sq);
        var v0 = capk * r / (filterOrder * val);

        var (sv, cv, dv, phi) = EllipJ(v0, 1.0 - m);

        // Compute each pole
        var poles = new Complex[jj];
        for (int i = 0; i < jj; i++)
        {
            var numerator = new Complex(cn[i] * dn[i] * sv * cv, sn[i] * dv);
            var denominator = 1.0 - (dn[i] * sv) * (dn[i] * sv);
            poles[i] = -numerator / denominator;
        }

        if (filterOrder % 2 != 0)
        {
            // Find those poles whose imaginary part is big enough
            var threshold = EPSILON * Math.Sqrt(poles.Sum(p => (p * Complex.Conjugate(p)).Real));

            var newp = poles.Where(p => Math.Abs(p.Imaginary) > threshold).ToArray();

            var fullPoles = poles.Concat(newp.Select(p => Complex.Conjugate(p))).ToArray();
            poles = fullPoles;
        }
        else
        {
            poles = poles.Concat(poles.Select(p => Complex.Conjugate(p))).ToArray();
        }

        var prodP = Complex.One;
        foreach (var pole in poles)
        {
            prodP *= -pole;
        }

        var prodZ = Complex.One;
        foreach (var zero in zeros)
        {
            prodZ *= -zero;
        }

        var k = (prodP / prodZ).Real;

        if (filterOrder % 2 == 0)
        {
            k /= Math.Sqrt(1.0 + eps_sq);
        }

        return new Zpk(zeros, poles, k);
    }

    /// <summary>
    /// Calculate IIR Filter transferFunction coefficients
    /// As in MATLAB => [z,p,k] = ellip(n, Rp, Rs, Wp)
    ///
    /// In contrast to matlab this method does not require to normalize the frequencies before calling.
    /// If BandPass or BandStop is selected the actual order of the filter created is double that of the specified order.
    /// </summary>
    /// <param name="frequencyFilterType"></param>
    /// <param name="freqSampling"></param>
    /// <param name="freqLowCutOff">The LowCutofff is not used if 'filterType' is 'HighPass'.</param>
    /// <param name="freqHighCutOff">The HighCutOff is not used if 'filterType' is 'LowPass'.</param>
    /// <param name="filterOrder"></param>
    /// <param name="passbandRippleDb"></param>
    /// <param name="stopbandAttenuationDb"></param>
    /// <returns> zeros(z), poles(p) and gain(k) for the specified filter settings </returns>
    public static Zpk CalcZpk(FrequencyFilterType frequencyFilterType,
        double freqSampling, double freqLowCutOff, double freqHighCutOff, int filterOrder, double passbandRippleDb, double stopbandAttenuationDb)
    {
        FilterTools.FrequencyVerification(frequencyFilterType, freqSampling, freqLowCutOff, freqHighCutOff);
        var lowPassPrototype = PrototypeAnalogLowPass(filterOrder, passbandRippleDb, stopbandAttenuationDb);
        var filter = FilterTools.CalcFilterSettings(frequencyFilterType, freqSampling, freqLowCutOff, freqHighCutOff, filterOrder, lowPassPrototype);
        return filter.zpk;
    }

    /// <summary>
    /// Calculate IIR Filter transferFunction coefficients
    /// As in MATLAB => [b,a] = ellip(n, Rp, Rs, Wp)
    ///
    /// In contrast to matlab this method does not require to normalize the frequencies before calling.
    /// If BandPass or BandStop is selected the actual order of the filter created is double that of the specified order.
    /// </summary>
    /// <param name="frequencyFilterType"></param>
    /// <param name="freqSampling"></param>
    /// <param name="freqLowCutOff">The LowCutofff is not used if 'filterType' is 'HighPass'.</param>
    /// <param name="freqHighCutOff">The HighCutOff is not used if 'filterType' is 'LowPass'.</param>
    /// <param name="filterOrder"></param>
    /// <param name="passbandRippleDb"></param>
    /// <param name="stopbandAttenuationDb"></param>
    /// <returns> numerator(b) and denominator(a) for the specified filter settings </returns>
    public static TransferFunction CalcTransferFunctionCalcZpk(FrequencyFilterType frequencyFilterType,
        double freqSampling, double freqLowCutOff, double freqHighCutOff, int filterOrder, double passbandRippleDb, double stopbandAttenuationDb)
    {
        FilterTools.FrequencyVerification(frequencyFilterType, freqSampling, freqLowCutOff, freqHighCutOff);
        var lowPassPrototype = PrototypeAnalogLowPass(filterOrder, passbandRippleDb, stopbandAttenuationDb);
        var filter = FilterTools.CalcFilterSettings(frequencyFilterType, freqSampling, freqLowCutOff, freqHighCutOff, filterOrder, lowPassPrototype);
        return filter.tf;
    }

    private static double Pow10Minus1(double x)
    {
        var ln10 = Math.Log(10);
        var value = ln10 * x;

        if (Math.Abs(value) < 1e-5)
        {
            // Use a Taylor series expansion for better accuracy when x is small
            return x + 0.5 * x * x + (1.0 / 6.0) * x * x * x;
        }
        else
        {
            return Math.Exp(value) - 1.0;
        }
    }

    // Computes the complete elliptic integral of the first kind
    private static double EllipK(double m)
    {
        if (m < 0.0 || m > 1.0)
        {
            throw new ArgumentOutOfRangeException(nameof(m), "m must be in [0, 1]");
        }

        var a = 1.0;
        var b = Math.Sqrt(1.0 - m);
        var c = m;

        for (int i = 0; i < 20; i++) // iterate until convergence
        {
            var an = (a + b) / 2.0;
            var bn = Math.Sqrt(a * b);

            if (Math.Abs(an - bn) < 1e-15)
                break;

            a = an;
            b = bn;
        }

        return Math.PI / (2.0 * a);
    }

    // Computes the complete elliptic integral of the first kind
    private static double EllipKComplement(double m)
    {
        return EllipK(1.0 - m);
    }

    private static double EllipDeg(int n, double m1)
    {
        if (m1 <= 0.0 || m1 >= 1.0)
        {
            throw new ArgumentOutOfRangeException(nameof(m1), "m1 must be in (0, 1)");
        }

        const int ELLIPDEGMAXM = 7;

        // Compute K(m1) and K'(m1)
        var K1 = EllipK(m1);
        var K1p = EllipKComplement(m1);

        // Compute nome q1 and q
        var q1 = Math.Exp(-Math.PI * K1p / K1);
        var q = Math.Pow(q1, 1.0 / n);

        // Numerator: sum q^(m(m+1)) for m = 0..ELLIPDEGMAXM
        var num = 0.0;
        for (var m = 0; m <= ELLIPDEGMAXM; m++)
        {
            var exp = m * (m + 1);
            num += Math.Pow(q, exp);
        }

        // Denominator: 1 + 2 * sum q^(m^2) for m = 1..ELLIPDEGMAXM+1
        var den = 1.0;
        for (var m = 1; m <= ELLIPDEGMAXM + 1; m++)
        {
            den += 2.0 * Math.Pow(q, m * m);
        }

        var result = 16.0 * q * Math.Pow(num / den, 4.0);
        return result;
    }

    private static Complex Atanh(Complex z)
    {
        // atanh(z) = 0.5 * log((1+z)/(1-z))
        return 0.5 * Complex.Log((Complex.One + z) / (Complex.One - z));
    }

    private static Complex Asin(Complex z)
    {
        // asin(z) = -i * log(iz + sqrt(1 - z^2))
        return -Complex.ImaginaryOne * Complex.Log(Complex.ImaginaryOne * z + Complex.Sqrt(Complex.One - z * z));
    }

    private static (double sn, double cn, double dn, double am) EllipJ(double u, double m)
    {
        if (m < 0.0 || m > 1.0)
        {
            throw new ArgumentOutOfRangeException(nameof(m), "m must be in [0, 1]");
        }

        var a = 1.0;
        var b = Math.Sqrt(1.0 - m);
        var c = Math.Sqrt(m);
        var twon = 1.0;

        var aList = new List<double> { a };
        var cList = new List<double> { c };

        int i = 0;
        while (i < 16 && Math.Abs(cList[i]) > 1e-12)
        {
            var aNext = (aList[i] + b) / 2.0;
            cList.Add((aList[i] - b) / 2.0);
            b = Math.Sqrt(aList[i] * b);
            aList.Add(aNext);
            twon *= 2.0;
            i++;
        }

        var phi = twon * aList[i] * u;

        for (int j = i - 1; j >= 0; j--)
        {
            phi = (Math.Asin(cList[j + 1] / aList[j + 1] * Math.Sin(phi)) + phi) / 2.0;
        }

        var sn = Math.Sin(phi);
        var cn = Math.Cos(phi);
        var dn = Math.Sqrt(1.0 - m * sn * sn);
        var am = phi;

        return (sn, cn, dn, am);
    }

    private static (double[] sn, double[] cn, double[] dn, double[] am) EllipJ(double[] u, double m)
    {
        var len = u.Length;
        var sn = new double[len];
        var cn = new double[len];
        var dn = new double[len];
        var am = new double[len];

        for (var i = 0; i < len; i++)
        {
            var (s, c, d, a) = EllipJ(u[i], m);
            sn[i] = s;
            cn[i] = c;
            dn[i] = d;
            am[i] = a;
        }

        return (sn, cn, dn, am);
    }

    private static double ArcJacSc1(double w, double m)
    {
        var zcomplex = ArcJacSn(Complex.ImaginaryOne * w, m);

        if (Math.Abs(zcomplex.Real) > 1e-14)
        {
            throw new InvalidOperationException("Real part is not negligible.");
        }

        return zcomplex.Imaginary;
    }

    private static Complex ArcJacSn(Complex w, double m)
    {
        const int ARC_JAC_SN_MAXITER = 10;

        static double Complement(double kx)
        {
            return Math.Sqrt((1 - kx) * (1 + kx));
        }

        static Complex ComplexComplement(Complex kx)
        {
            return Complex.Sqrt((1 - kx) * (1 + kx));
        }

        var k = Math.Sqrt(m);

        if (k > 1.0)
        {
            return Complex.NaN;
        }
        else if (k == 1.0)
        {
            return Atanh(w);
        }

        var ks = new List<double> { k };
        var niter = 0;

        while (ks[ks.Count - 1] != 0.0)
        {
            var k_ = ks[ks.Count - 1];
            var k_p = Complement(k_);
            var next = (1 - k_p) / (1 + k_p);
            ks.Add(next);
            niter++;
            if (niter > ARC_JAC_SN_MAXITER)
            {
                throw new InvalidOperationException("Landen transformation not converging.");
            }
        }

        var K = 1.0;
        for (int i = 1; i < ks.Count; i++)
        {
            K *= (1 + ks[i]);
        }
        K *= Math.PI / 2.0;

        var wns = new List<Complex> { w };

        for (int i = 0; i < ks.Count - 1; i++)
        {
            var kn = ks[i];
            var knext = ks[i + 1];
            var wn = wns[wns.Count - 1];
            var numerator = 2.0 * wn;
            var denominator = (1.0 + knext) * (1.0 + ComplexComplement(kn * wn));
            var wnext = numerator / denominator;
            wns.Add(wnext);
        }

        var finalWn = wns[wns.Count - 1];
        var u = (2.0 / Math.PI) * Asin(finalWn);

        var z = K * u;
        return z;
    }
}
