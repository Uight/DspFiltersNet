using DspFiltersNet.Filter;
using DspFiltersNet.FilterImplementations;

namespace DspFiltersNet.Tests.FilterImplementations;

[TestFixture]
internal class ButterworthTests
{
    //All testData are created with MatLab
    //The order of poles and zeros in Matlab result might differ but doesn't matter
    public static IEnumerable<TestCaseData> ZpkTestDataButterworth
    {
        get
        {
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 1,
                new Zpk([new(-1.0, 0.0)],
                    [new(0.939062505817492, 0.0)],
                    0.030468747091254)).SetName("(Zpk Test) LowPass: 1st Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 1,
                new Zpk([new(-1.0, 0.0)],
                    [new(0.662767791922216, 0.0)],
                    0.168616104038892)).SetName("(Zpk Test) LowPass: 1st Order, 42.42Hz with 666Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 2,
                new Zpk([new(-1.0, 0.0), new(-1.0, 0.0)],
                    [new(0.955598533713036, 0.0425120825992831), new(0.955598533713036, -0.0425120825992831)],
                    9.446918438401597e-04)).SetName("(Zpk Test) LowPass: 2nd Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 2,
                new Zpk([new(-1.0, 0.0), new(-1.0, 0.0)],
                    [new(0.722062325416860, 0.215987663676567), new(0.722062325416860, -0.215987663676567)],
                    0.030975005453286)).SetName("(Zpk Test) LowPass: 2nd Order, 42.42Hz with 666Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 3,
                new Zpk([new (-1.0, 0.0),
                         new (-1.0, 0.0),
                         new (-1.0, 0.0)],
                        [new (0.967647193429996, 0.0527229347957411),
                         new (0.967647193429996, -0.0527229347957411),
                         new (0.939062505817492, 0.0)],
                    2.914649446569766e-05)).SetName("(Zpk Test) LowPass: 3rd Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 3,
                new Zpk([new (-1.0, 0.0),
                         new (-1.0, 0.0),
                         new (-1.0, 0.0)],
                        [new (0.770825783545074, 0.282394428238312),
                         new (0.770825783545074, -0.282394428238312),
                         new (0.662767791922216, 0.0)],
                    0.005575604877852)).SetName("(Zpk Test) LowPass: 3rd Order, 42.42Hz with 666Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 4,
                new Zpk([new (-1.0, 0.0), new (-1.0, 0.0),
                         new (-1.0, 0.0), new (-1.0, 0.0)],
                        [new (0.974607979012921, 0.0566496475914977), new (0.974607979012921, -0.0566496475914977),
                         new (0.943304791310753, 0.0227113842422826), new (0.943304791310753, -0.0227113842422826)],
                    8.984861463970445e-07)).SetName("(Zpk Test) LowPass: 4th Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 4,
                new Zpk([new (-1.0, 0.0), new (-1.0, 0.0),
                         new (-1.0, 0.0), new (-1.0, 0.0)],
                        [new (0.801486408455084, 0.313242561606614), new (0.801486408455084, -0.313242561606614),
                         new (0.677220940914573, 0.109632495121890), new (0.677220940914573, -0.109632495121890)],
                    9.988492606903899e-04)).SetName("(Zpk Test) LowPass: 4th Order, 42.42Hz with 666Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 10,
                new Zpk([new (-1.0, 0.0), new (-1.0, 0.0),
                         new (-1.0, 0.0), new (-1.0, 0.0),
                         new (-1.0, 0.0), new (-1.0, 0.0),
                         new (-1.0, 0.0), new (-1.0, 0.0),
                         new (-1.0, 0.0), new (-1.0, 0.0)],
                        [new (0.988318866216371, 0.0614142166725119), new (0.988318866216371, -0.0614142166725119),
                         new (0.970365207358179, 0.0543961301877272), new (0.970365207358179, -0.0543961301877272),
                         new (0.955598533713038, 0.0425120825992806), new (0.955598533713038, -0.0425120825992806),
                         new (0.945148717545925, 0.0269959626288111), new (0.945148717545925, -0.0269959626288111),
                         new (0.939746060891149, 0.00924900170173081), new (0.939746060891149, -0.00924900170173081)],
                    7.685849849984824e-16)).SetName("(Zpk Test) LowPass: 10th Order, 10Hz with 1000Hz Sampling");

            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 1,
                new Zpk([new(1.0, 0.0)],
                    [new(0.939062505817492, 0.0)],
                    0.969531252908746)).SetName("(Zpk Test) HighPass: 1st Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 2,
                new Zpk([new(1.0, 0.0), new(1.0, 0.0)],
                    [new(0.955598533713037, -0.042512082599283), new(0.955598533713037, 0.042512082599283)],
                    0.956543225556877)).SetName("(Zpk Test) HighPass: 2nd Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 666, 0, 42.42, 2,
                new Zpk([new(1.0, 0.0), new(1.0, 0.0)],
                    [new(0.722062325416860, -0.215987663676567), new(0.722062325416860, 0.215987663676567)],
                    0.753037330870147)).SetName("(Zpk Test) HighPass: 2nd Order, 42.42Hz with 666Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 3,
                new Zpk([new (1.0, 0.0),
                         new (1.0, 0.0),
                         new (1.0, 0.0)],
                        [new (0.967647193429996, -0.0527229347957411),
                         new (0.967647193429996, 0.0527229347957411),
                         new (0.939062505817492, 0.0)],
                    0.939091652311958)).SetName("(Zpk Test) HighPass: 3rd Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 666, 0, 42.42, 3,
                new Zpk([new (1.0, 0.0),
                         new (1.0, 0.0),
                         new (1.0, 0.0)],
                        [new (0.770825783545074, -0.282394428238312),
                         new (0.770825783545074, 0.282394428238312),
                         new (0.662767791922216, 0.0)],
                    0.668343396800068)).SetName("(Zpk Test) HighPass: 3rd Order, 42.42Hz with 666Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 4,
                new Zpk([new (1.0, 0.0), new (1.0, 0.0),
                         new (1.0, 0.0), new (1.0, 0.0)],
                        [new (0.974607979012921, -0.0566496475914977), new (0.974607979012921, 0.0566496475914977),
                         new (0.943304791310753, -0.0227113842422826), new (0.943304791310753, 0.0227113842422826)],
                    0.921170993499941)).SetName("(Zpk Test) HighPass: 4th Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 6,
                new Zpk([new (1.0, 0.0), new (1.0, 0.0),
                        new (1.0, 0.0), new (1.0, 0.0),
                        new (1.0, 0.0), new (1.0, 0.0)],
                        [new (0.982066785644670, -0.059681084341399), new (0.982066785644670, 0.059681084341399),
                        new (0.955598533713036, -0.042512082599283), new (0.955598533713036, 0.042512082599283),
                        new (0.940956773765582, -0.015322082894543), new (0.940956773765582, 0.015322082894543)],
                    0.885673290152358)).SetName("(Zpk Test) HighPass: 6th Order, 10Hz with 1000Hz Sampling");

            yield return new TestCaseData(FrequencyFilterType.BandPass, 1000, 0.1, 10, 1,
                new Zpk([new (1.0, 0.0),
                        new (-1.0, 0.0)],
                        [new (0.999358933617404, 0.0),
                        new (0.940256644192184, 0.0)],
                    0.030173061366710)).SetName("(Zpk Test) BandPass: 1st Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandPass, 1000, 0.1, 10, 2,
                new Zpk([new (1.0, 0.0),
                        new (1.0, 0.0),
                        new (-1.0, 0.0),
                        new (-1.0, 0.0)],
                        [new (0.999555889618897, -0.000453054971183), new (0.956447514042325,  0.042557613240989),
                         new (0.999555889618897,  0.000453054971183), new (0.956447514042325, -0.042557613240989)],
                    9.262918135802441e-04)).SetName("(Zpk Test) BandPass: 2nd Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandPass, 666, 0.2581, 42.42, 2,
                new Zpk([new (1.0, 0.0),
                        new (1.0, 0.0),
                        new (-1.0, 0.0),
                        new (-1.0, 0.0)],
                        [new (0.998278433309316, -0.001739580088654), new (0.724574682214599,  0.216644660192020),
                         new (0.998278433309316,  0.001739580088654), new (0.724574682214599, -0.216644660192020)],
                    0.030642307207318)).SetName("(Zpk Test) BandPass: 2nd Order, Low: 0.2581Hz, High: 42.42Hz, Sampling: 666Hz");
            yield return new TestCaseData(FrequencyFilterType.BandPass, 1000, 0.1, 10, 3,
                new Zpk([new (1.0, 0.0),
                         new (1.0, 0.0),
                         new (1.0, 0.0),
                         new (-1.0, 0.0),
                         new (-1.0, 0.0),
                         new (-1.0, 0.0)],
                        [new (0.999688971571037, -0.000549352054410),
                         new (0.968251690671560, 0.052760582757107),
                         new (0.999688971571037, 0.000549352054410),
                         new (0.968251690671560, -0.052760582757107),
                         new (0.999358933617404, 0),
                         new (0.940256644192183, 0)],
                    2.829804203014804e-05)).SetName("(Zpk Test) BandPass: 3rd Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandPass, 666, 0.2581, 42.42, 3,
                new Zpk([new (1.0, 0.0),
                        new (1.0, 0.0),
                        new (1.0, 0.0),
                        new (-1.0, 0.0),
                        new (-1.0, 0.0),
                        new (-1.0, 0.0)],
                        [new (0.998788439117862, -0.002118761055299),
                        new (0.772679200379900, 0.283012001097383),
                        new (0.998788439117862, 0.002118761055299),
                        new (0.772679200379900, -0.283012001097383),
                        new (0.997538289885068, 0),
                        new (0.666161383050127, 0)],
                    0.005485360217653)).SetName("(Zpk Test) BandPass: 3rd Order, Low: 0.2581Hz, High: 42.42Hz, Sampling: 666Hz");
            yield return new TestCaseData(FrequencyFilterType.BandPass, 1000, 0.1, 10, 4,
                new Zpk([new (1.0, 0.0),
                        new (1.0, 0.0),
                        new (1.0, 0.0),
                        new (1.0, 0.0),
                        new (-1.0, 0.0),
                        new (-1.0, 0.0),
                        new (-1.0, 0.0),
                        new (-1.0, 0.0)],
                        [new (0.999762842558254, -0.000583689099134),
                        new (0.975072934121091, 0.056679613211146),
                        new (0.999762842558254, 0.000583689099134),
                        new (0.975072934121091, -0.056679613211146),
                        new (0.999411366568669, -0.000248832335277),
                        new (0.944408993286020, 0.022745272080304),
                        new (0.999411366568669, 0.000248832335277),
                        new (0.944408993286020, -0.022745272080304)],
                    8.637688106445837e-07)).SetName("(Zpk Test) BandPass: 4th Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");

            yield return new TestCaseData(FrequencyFilterType.BandStop, 1000, 0.1, 10, 1,
                new Zpk([new (0.999980254488989, 0.006284157233677),
                        new (0.999980254488989, -0.006284157233677)],
                        [new (0.999358933617404, 0.0),
                        new (0.940256644192184, 0.0)],
                    0.96982693863329017)).SetName("(Zpk Test) BandStop: 1st Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandStop, 1000, 0.1, 10, 2,
                new Zpk([new (0.999980254488989, 0.006284157233677),
                        new (0.999980254488989, -0.006284157233677),
                        new (0.999980254488989, 0.006284157233677),
                        new (0.999980254488989, -0.006284157233677)],
                        [new (0.999555889618897, 0.000453054971183), new (0.956447514042325,  -0.042557613240989),
                         new (0.999555889618897,  -0.000453054971183), new (0.956447514042325, 0.042557613240989)],
                    0.9569683185241783)).SetName("(Zpk Test) BandStop: 2nd Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandStop, 666, 0.2581, 42.42, 2,
                new Zpk([new (0.999506276184116, 0.031419800581176),
                        new (0.999506276184116, -0.031419800581176),
                        new (0.999506276184116, 0.031419800581176),
                        new (0.999506276184116, -0.031419800581176)],
                        [new (0.998278433309316, 0.001739580088654), new (0.724574682214599, -0.216644660192020),
                         new (0.998278433309316, -0.001739580088654), new (0.724574682214599, 0.216644660192020)],
                    0.75434645652128662)).SetName("(Zpk Test) BandStop: 2nd Order, Low: 0.2581Hz, High: 42.42Hz, Sampling: 666Hz");
            yield return new TestCaseData(FrequencyFilterType.BandStop, 1000, 0.1, 10, 3,
                new Zpk([new (0.999980254488989, 0.006284157233677),
                         new (0.999980254488989, -0.006284157233677),
                         new (0.999980254488989, 0.006284157233677),
                         new (0.999980254488989, -0.006284157233677),
                         new (0.999980254488989, 0.006284157233677),
                         new (0.999980254488989, -0.006284157233677)],
                        [new (0.999688971571037, 0.000549352054410),
                         new (0.968251690671560, -0.052760582757107),
                         new (0.999688971571037, -0.000549352054410),
                         new (0.968251690671560, 0.052760582757107),
                         new (0.999358933617404, 0),
                         new (0.940256644192183, 0)],
                    0.93968217530861076)).SetName("(Zpk Test) BandStop: 3rd Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");


            yield return new TestCaseData(FrequencyFilterType.BandStop, 1000, 0.1, 10, 4,
                new Zpk([new (0.999980254488989, 0.006284157233677),
                        new (0.999980254488989, -0.006284157233677),
                        new (0.999980254488989, 0.006284157233677),
                        new (0.999980254488989, -0.006284157233677),
                        new (0.999980254488989, 0.006284157233677),
                        new (0.999980254488989, -0.006284157233677),
                        new (0.999980254488989, 0.006284157233677),
                        new (0.999980254488989, -0.006284157233677)],

                        [new (0.999762842558254, 0.000583689099134),
                        new (0.975072934121091, -0.056679613211146),
                        new (0.999762842558254, -0.000583689099134),
                        new (0.975072934121091, 0.056679613211146),
                        new (0.999411366568669, 0.000248832335277),
                        new (0.944408993286020, -0.022745272080304),
                        new (0.999411366568669, -0.000248832335277),
                        new (0.944408993286020, 0.022745272080304)],
                    0.92192796149812739)).SetName("(Zpk Test) BandStop: 4th Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
        }
    }

    //All testData are created with MatLab
    public static IEnumerable<TestCaseData> TfTestDataButterworth
    {
        get
        {
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 1,
                new TransferFunction([0.030468747091254, 0.030468747091254],
                    [1, -0.939062505817492]),
                1.0e-15).SetName("(Tf Test) LowPass: 1st Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 1,
                new TransferFunction([0.168616104038892, 0.168616104038892],
                    [1, -0.662767791922216]),
                1.0e-15).SetName("(Tf Test) LowPass: 1st Order, 42.42Hz with 666Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 2,
                new TransferFunction([9.446918438401597e-04, 0.001889383687680, 9.446918438401597e-04],
                    [1, -1.911197067426073, 0.914975834801433]),
                1.0e-15).SetName("(Tf Test) LowPass: 2nd Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 2,
                new TransferFunction([0.030975005453286, 0.061950010906573, 0.030975005453286],
                    [1, -1.444124650833721, 0.568024672646866]),
                1.0e-15).SetName("(Tf Test) LowPass: 2nd Order, 42.42Hz with 666Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 3,
                new TransferFunction([2.914649446569766e-05, 8.743948339709297e-05, 8.743948339709297e-05, 2.914649446569766e-05],
                    [1, -2.874356892677485, 2.756483195225695, -0.881893130592486]),
                1.0e-15).SetName("(Tf Test) LowPass: 3rd Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 3,
                new TransferFunction([0.005575604877852, 0.016726814633557, 0.016726814633557, 0.005575604877852],
                    [1, -2.204419359012364, 1.695676006711682, -0.446651808676499]),
                1.0e-15).SetName("(Tf Test) LowPass: 3rd Order, 42.42Hz with 666Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 4,
                new TransferFunction([8.984861463970445e-07, 3.593944585588178e-06, 5.390916878382267e-06, 3.593944585588178e-06, 8.984861463970445e-07],
                    [1, -3.835825540647349, 5.520819136622230, -3.533535219463015, 0.848555999266477]),
                1.0e-14).SetName("(Tf Test) LowPass: 4th Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 4,
                new TransferFunction([9.988492606903899e-04, 0.003995397042762, 0.005993095564142, 0.003995397042762, 9.988492606903899e-04],
                    [1, -2.957414698739313, 3.382282370796745, -1.757401190455578, 0.348515106569192]),
                1.0e-15).SetName("(Tf Test) LowPass: 4th Order, 42.42Hz with 666Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 10,
                new TransferFunction([7.68584984998482e-16, 7.68584984998482e-15, 3.45863243249317e-14, 9.22301981998179e-14, 1.61402846849681e-13, 1.93683416219618e-13, 1.61402846849681e-13, 9.22301981998179e-14, 3.45863243249317e-14, 7.68584984998482e-15, 7.68584984998482e-16],
                    [1, -9.59835477144932, 41.4655792756444, -106.173354913648, 178.440055558469, -205.679548276819, 164.666485668553, -90.4147875793786, 32.5851033631510, -6.96033549559011, 0.669157171068017]),
                1.0e-12).SetName("(Tf Test) LowPass: 10th Order, 10Hz with 1000Hz Sampling");

            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 1,
                new TransferFunction([0.969531252908746, -0.969531252908746],
                    [1, -0.939062505817492]),
                1.0e-15).SetName("(Tf Test) HighPass: 1st Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 2,
                new TransferFunction([0.956543225556877, -1.913086451113754, 0.956543225556877],
                    [1, -1.911197067426073, 0.914975834801434]),
                1.0e-15).SetName("(Tf Test) HighPass: 2nd Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 666, 0, 42.42, 2,
                new TransferFunction([0.753037330870147, -1.506074661740293, 0.753037330870147],
                    [1, -1.444124650833721, 0.568024672646866]),
                1.0e-15).SetName("(Tf Test) HighPass: 2nd Order, 42.42Hz with 666Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 3,
                new TransferFunction([0.939091652311958, -2.817274956935875, 2.817274956935875, -0.939091652311958],
                    [1, -2.874356892677485, 2.756483195225695, -0.881893130592486]),
                1.0e-15).SetName("(Tf Test) HighPass: 3rd Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 666, 0, 42.42, 3,
                new TransferFunction([0.668343396800068, -2.005030190400204, 2.005030190400204, -0.668343396800068],
                    [1, -2.204419359012364, 1.695676006711682, -0.446651808676499]),
                1.0e-15).SetName("(Tf Test) HighPass: 3rd Order, 42.42Hz with 666Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 4,
                new TransferFunction([0.921170993499941, -3.684683973999764, 5.527025960999646, -3.684683973999764, 0.921170993499941],
                    [1, -3.835825540647349, 5.520819136622230, -3.533535219463015, 0.848555999266477]),
                1.0e-14).SetName("(Tf Test) HighPass: 4th Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 6,
                new TransferFunction([0.885673290152358, -5.314039740914147, 13.285099352285368, -17.713465803047157, 13.285099352285368, -5.314039740914147, 0.885673290152358],
                    [1, -5.757244186246575, 13.815510806058022, -17.687376179894024, 12.741617329229229, -4.896924891433742, 0.784417176889303]),
                1.0e-13).SetName("(Tf Test) HighPass: 6th Order, 10Hz with 1000Hz Sampling");

            yield return new TestCaseData(FrequencyFilterType.BandPass, 1000, 0.1, 10, 1,
                new TransferFunction([0.030173061366710, 0, -0.030173061366710],
                    [1, -1.939615577809589, 0.939653877266580]),
                1.0e-15).SetName("(Tf Test) BandPass: 1st Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandPass, 1000, 0.1, 10, 2,
                new TransferFunction([9.262918135802441e-04, 0, -0.001852583627160, 0, 9.262918135802441e-04],
                    [1, -3.912006807322443, 5.739806162382575, -3.743588574243218, 0.915789220675518]),
                1.0e-15).SetName("(Tf Test) BandPass: 2nd Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandPass, 666, 0.2581, 42.42, 2,
                new TransferFunction([0.030642307207318, 0, -0.061284614414636, 0, 0.030642307207318],
                    [1, -3.445706231047832, 4.461815349752635, -2.586085910634371, 0.569977527457210]),
                1.0e-14).SetName("(Tf Test) BandPass: 2nd Order, Low: 0.2581Hz, High: 42.42Hz, Sampling: 666Hz");
            yield return new TestCaseData(FrequencyFilterType.BandPass, 1000, 0.1, 10, 3,
                new TransferFunction([2.829804203014804e-05, 0, -8.489412609044412e-05, 0, 8.489412609044412e-05, 0, -2.829804203014804e-05],
                    [1, -5.875496902294781, 14.385226111375008, -18.785699218122680, 13.800710301273888, -5.407742882023321, 0.883002589791942]),
                1.0e-13).SetName("(Tf Test) BandPass: 3rd Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandPass, 666, 0.2581, 42.42, 3,
                new TransferFunction([0.005485360217653, 0, -0.016456080652960, 0, 0.016456080652960, 0, -0.005485360217653],
                    [1, -5.206634951930720, 11.320585736404603, -13.170608835270183, 8.654879882935717, -3.047100917415440, 0.448879085921117]),
                1.0e-13).SetName("(Tf Test) BandPass: 3rd Order, Low: 0.2581Hz, High: 42.42Hz, Sampling: 666Hz");
            yield return new TestCaseData(FrequencyFilterType.BandPass, 1000, 0.1, 10, 4,
                new TransferFunction([8.637688106445837e-07, 0, -3.455075242578335e-06, 0, 5.182612863867502e-06, 0, -3.455075242578335e-06, 0, 8.637688106445837e-07],
                    [1, -7.837312273068069, 26.874437766916110, -52.662441077811536, 64.501583293299400, -50.564748152342660, 24.776146706053960, -6.937617429240048, 0.849951166192838]),
                1.0e-13).SetName("(Tf Test) BandPass: 4th Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandPass, 1000, 0.1, 10, 6,
                new TransferFunction([8.04177329323391e-10, 0, -4.82506397594035e-09, 0, 1.20626599398509e-08, 0, -1.60835465864678e-08, 0, 1.20626599398509e-08, 0, -4.82506397594035e-09, 0, 8.04177329323391e-10],
                    [1, -11.7594393896669, 63.3828205899279, -207.056829190520, 456.592147549707, -716.014798130393, 818.765781725918, -687.893505030881, 421.431558060682, -183.606464164708, 53.9970886401915, -9.62468561325294, 0.786324952996040]),
                1.0e-12).SetName("(Tf Test) BandPass: 6th Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");

            yield return new TestCaseData(FrequencyFilterType.BandStop, 1000, 0.1, 10, 1,
                new TransferFunction([0.969826938633303, -1.939615577809614, 0.969826938633303],
                    [1, -1.939615577809589, 0.939653877266580]),
                1.0e-13).SetName("(Tf Test) BandStop: 1st Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandStop, 1000, 0.1, 10, 2,
                new TransferFunction([0.956968318523830, -3.827797690781438, 5.741658746007646, -3.827797690781438, 0.956968318523830],
                    [1, -3.912006807322443, 5.739806162382575, -3.743588574243218, 0.915789220675518]),
                1.0e-11).SetName("(Tf Test) BandStop: 2nd Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandStop, 666, 0.2581, 42.42, 2,
                new TransferFunction([0.754346456521240, -3.015896070840912, 4.523099964166987, -3.015896070840912, 0.754346456521240],
                    [1, -3.445706231047832, 4.461815349752635, -2.586085910634371, 0.569977527457210]),
                1.0e-12).SetName("(Tf Test) BandStop: 2nd Order, Low: 0.2581Hz, High: 42.42Hz, Sampling: 666Hz");
            yield return new TestCaseData(FrequencyFilterType.BandStop, 1000, 0.1, 10, 3,
                new TransferFunction([0.939682175308134, -5.637981724820364, 14.094787325904669, -18.792975552784820, 14.094787325904669, -5.637981724820364, 0.939682175308134],
                    [1, -5.875496902294781, 14.385226111375008, -18.785699218122680, 13.800710301273888, -5.407742882023321, 0.883002589791942]),
                1.0e-11).SetName("(Tf Test) BandStop: 3rd Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandStop, 666, 0.2581, 42.42, 3,
                new TransferFunction([0.670006847052918, -4.018056292294336, 10.042165505577291, -13.388232120026656, 10.042165505577291, -4.018056292294336, 0.670006847052918],
                    [1, -5.206634951930720, 11.320585736404603, -13.170608835270183, 8.654879882935717, -3.047100917415440, 0.448879085921117]),
                1.0e-12).SetName("(Tf Test) BandStop: 3rd Order, Low: 0.2581Hz, High: 42.42Hz, Sampling: 666Hz");
            yield return new TestCaseData(FrequencyFilterType.BandStop, 1000, 0.1, 10, 4,
                new TransferFunction([0.921927961495561, -7.375278060454769, 25.813109141444090, -51.625781405612190, 64.532044726254610, -51.625781405612190, 25.813109141444090, -7.375278060454770, 0.921927961495561],
                    [1, -7.837312273068069, 26.874437766916110, -52.662441077811536, 64.501583293299400, -50.564748152342660, 24.776146706053960, -6.937617429240048, 0.849951166192838]),
                1.0e-9).SetName("(Tf Test) BandStop: 4th Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandStop, 1000, 0.1, 10, 6,
                new TransferFunction([0.886749656320719, -10.6407857639474, 58.5233762188989, -195.075469520952, 438.915867031428, -702.261605468388, 819.303735693279, -702.261605468388, 438.915867031428, -195.075469520952, 58.5233762188989, -10.6407857639474, 0.886749656320719],
                    [1, -11.7594393896669, 63.3828205899279, -207.056829190520, 456.592147549707, -716.014798130393, 818.765781725918, -687.893505030881, 421.431558060682, -183.606464164708, 53.9970886401915, -9.62468561325294, 0.786324952996040]),
                1.0e-8).SetName("(Tf Test) BandStop: 6th Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
        }
    }

    [TestCaseSource(nameof(ZpkTestDataButterworth))]
    public void CalcZpkTestsButterworth(FrequencyFilterType frequencyFilterType, double sourceFrequency, double lowCutOff, double highCutoff, int order, Zpk expectedZpk)
    {
        //Arrange + Act
        var zpk = Butterworth.CalcZpk(frequencyFilterType, sourceFrequency, lowCutOff, highCutoff, order);

        //Assert
        TestHelper.CompareZpk(zpk, expectedZpk);
    }

    [TestCaseSource(nameof(TfTestDataButterworth))]
    public void CalcTransferFunctionTestsButterworth(FrequencyFilterType frequencyFilterType, double sourceFrequency, double lowCutOff, double highCutoff, int order, TransferFunction expectedTf, double tolerance)
    {
        //Arrange + Act
        var tf = Butterworth.CalcTransferFunction(frequencyFilterType, sourceFrequency, lowCutOff, highCutoff, order);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(tf.A, Is.EqualTo(expectedTf.A).Within(tolerance), "A incorrect");
            Assert.That(tf.B, Is.EqualTo(expectedTf.B).Within(tolerance), "B incorrect");
        });
    }

    [TestCase(-1)]
    [TestCase(0)]
    [TestCase(17)]
    public void CalcButterworthZpkOrderOutOfRange(int order)
    {
        //Arrange

        //Act + Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => Butterworth.CalcZpk(FrequencyFilterType.LowPass, 1000, 10, 0, order));
    }
}
