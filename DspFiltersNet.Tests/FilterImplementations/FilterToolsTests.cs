using DspFiltersNet.Filter;
using DspFiltersNet.FilterImplementations;

namespace DspFiltersNet.Tests.FilterImplementations;

//This Test checks the calculations for the Filters. The Bessel and Butterworth classes are not directly testes but indirectly by using testing the FilterTool class which uses the other classes.
//The Test for the filter instance is in the test for the library function "FilterCurve" at the moment
[TestFixture]
internal class FilterToolsTests
{
    //All testData are created with MatLab
    //The order of poles and zeros in Matlab result might differ but doesn't matter
    public static IEnumerable<TestCaseData> ZpkTestDataButterworth
    {
        get
        {
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 1, 
                new Zpk([new (-1.0, 0.0)], 
                    [new (0.939062505817492, 0.0)],
                    0.030468747091254)).SetName("LowPass: 1st Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 1, 
                new Zpk([new (-1.0, 0.0)], 
                    [new (0.662767791922216, 0.0)],
                    0.168616104038892)).SetName("LowPass: 1st Order, 42.42Hz with 666Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 2, 
                new Zpk([new (-1.0, 0.0), new (-1.0, 0.0)], 
                    [new (0.955598533713036, 0.0425120825992831), new (0.955598533713036, -0.0425120825992831)],
                    9.446918438401597e-04)).SetName("LowPass: 2nd Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 2, 
                new Zpk([new (-1.0, 0.0), new (-1.0, 0.0)], 
                    [new (0.722062325416860, 0.215987663676567), new (0.722062325416860, -0.215987663676567)],
                    0.030975005453286)).SetName("LowPass: 2nd Order, 42.42Hz with 666Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 3, 
                new Zpk([new (-1.0, 0.0), 
                         new (-1.0, 0.0), 
                         new (-1.0, 0.0)], 
                        [new (0.967647193429996, 0.0527229347957411), 
                         new (0.967647193429996, -0.0527229347957411), 
                         new (0.939062505817492, 0.0)],
                    2.914649446569766e-05)).SetName("LowPass: 3rd Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 3, 
                new Zpk([new (-1.0, 0.0), 
                         new (-1.0, 0.0), 
                         new (-1.0, 0.0)], 
                        [new (0.770825783545074, 0.282394428238312),
                         new (0.770825783545074, -0.282394428238312), 
                         new (0.662767791922216, 0.0)],
                    0.005575604877852)).SetName("LowPass: 3rd Order, 42.42Hz with 666Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 4, 
                new Zpk([new (-1.0, 0.0), new (-1.0, 0.0), 
                         new (-1.0, 0.0), new (-1.0, 0.0)], 
                        [new (0.974607979012921, 0.0566496475914977), new (0.974607979012921, -0.0566496475914977), 
                         new (0.943304791310753, 0.0227113842422826), new (0.943304791310753, -0.0227113842422826)],
                    8.984861463970445e-07)).SetName("LowPass: 4th Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 4, 
                new Zpk([new (-1.0, 0.0), new (-1.0, 0.0), 
                         new (-1.0, 0.0), new (-1.0, 0.0)], 
                        [new (0.801486408455084, 0.313242561606614), new (0.801486408455084, -0.313242561606614), 
                         new (0.677220940914573, 0.109632495121890), new (0.677220940914573, -0.109632495121890)],
                    9.988492606903899e-04)).SetName("LowPass: 4th Order, 42.42Hz with 666Hz Sampling");
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
                    7.685849849984824e-16)).SetName("LowPass: 10th Order, 10Hz with 1000Hz Sampling");
            
            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 1, 
                new Zpk([new (1.0, 0.0)], 
                    [new (0.939062505817492, 0.0)],
                    0.969531252908746)).SetName("HighPass: 1st Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 2, 
                new Zpk([new (1.0, 0.0), new (1.0, 0.0)], 
                    [new (0.955598533713037, -0.042512082599283), new (0.955598533713037, 0.042512082599283)],
                    0.956543225556877)).SetName("HighPass: 2nd Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 666, 0, 42.42, 2, 
                new Zpk([new (1.0, 0.0), new (1.0, 0.0)], 
                    [new (0.722062325416860, -0.215987663676567), new (0.722062325416860, 0.215987663676567)],
                    0.753037330870147)).SetName("HighPass: 2nd Order, 42.42Hz with 666Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 3, 
                new Zpk([new (1.0, 0.0), 
                         new (1.0, 0.0), 
                         new (1.0, 0.0)], 
                        [new (0.967647193429996, -0.0527229347957411), 
                         new (0.967647193429996, 0.0527229347957411), 
                         new (0.939062505817492, 0.0)],
                    0.939091652311958)).SetName("HighPass: 3rd Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 666, 0, 42.42, 3, 
                new Zpk([new (1.0, 0.0), 
                         new (1.0, 0.0), 
                         new (1.0, 0.0)], 
                        [new (0.770825783545074, -0.282394428238312),
                         new (0.770825783545074, 0.282394428238312), 
                         new (0.662767791922216, 0.0)],
                    0.668343396800068)).SetName("HighPass: 3rd Order, 42.42Hz with 666Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 4, 
                new Zpk([new (1.0, 0.0), new (1.0, 0.0), 
                         new (1.0, 0.0), new (1.0, 0.0)], 
                        [new (0.974607979012921, -0.0566496475914977), new (0.974607979012921, 0.0566496475914977), 
                         new (0.943304791310753, -0.0227113842422826), new (0.943304791310753, 0.0227113842422826)],
                    0.921170993499941)).SetName("HighPass: 4th Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 6, 
                new Zpk([new (1.0, 0.0), new (1.0, 0.0), 
                        new (1.0, 0.0), new (1.0, 0.0),
                        new (1.0, 0.0), new (1.0, 0.0)], 
                        [new (0.982066785644670, -0.059681084341399), new (0.982066785644670, 0.059681084341399), 
                        new (0.955598533713036, -0.042512082599283), new (0.955598533713036, 0.042512082599283),
                        new (0.940956773765582, -0.015322082894543), new (0.940956773765582, 0.015322082894543)],
                    0.885673290152358)).SetName("HighPass: 6th Order, 10Hz with 1000Hz Sampling");
            
            yield return new TestCaseData(FrequencyFilterType.BandPass, 1000, 0.1, 10, 1, 
                new Zpk([new (1.0, 0.0),
                        new (-1.0, 0.0)], 
                        [new (0.999358933617404, 0.0),
                        new (0.940256644192184, 0.0)],
                    0.030173061366710)).SetName("BandPass: 1st Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandPass, 1000, 0.1, 10, 2, 
                new Zpk([new (1.0, 0.0), 
                        new (1.0, 0.0),
                        new (-1.0, 0.0), 
                        new (-1.0, 0.0)], 
                        [new (0.999555889618897, -0.000453054971183), new (0.956447514042325,  0.042557613240989), 
                         new (0.999555889618897,  0.000453054971183), new (0.956447514042325, -0.042557613240989)],
                    9.262918135802441e-04)).SetName("BandPass: 2nd Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandPass, 666, 0.2581, 42.42, 2, 
                new Zpk([new (1.0, 0.0), 
                        new (1.0, 0.0),
                        new (-1.0, 0.0), 
                        new (-1.0, 0.0)], 
                        [new (0.998278433309316, -0.001739580088654), new (0.724574682214599,  0.216644660192020), 
                         new (0.998278433309316,  0.001739580088654), new (0.724574682214599, -0.216644660192020)],
                    0.030642307207318)).SetName("BandPass: 2nd Order, Low: 0.2581Hz, High: 42.42Hz, Sampling: 666Hz");
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
                    2.829804203014804e-05)).SetName("BandPass: 3rd Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
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
                    0.005485360217653)).SetName("BandPass: 3rd Order, Low: 0.2581Hz, High: 42.42Hz, Sampling: 666Hz");
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
                    8.637688106445837e-07)).SetName("BandPass: 4th Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            
            yield return new TestCaseData(FrequencyFilterType.BandStop, 1000, 0.1, 10, 1, 
                new Zpk([new (0.999980254488989, 0.006284157233677),
                        new (0.999980254488989, -0.006284157233677)], 
                        [new (0.999358933617404, 0.0),
                        new (0.940256644192184, 0.0)],
                    0.96982693863329017)).SetName("BandStop: 1st Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandStop, 1000, 0.1, 10, 2, 
                new Zpk([new (0.999980254488989, 0.006284157233677), 
                        new (0.999980254488989, -0.006284157233677),
                        new (0.999980254488989, 0.006284157233677), 
                        new (0.999980254488989, -0.006284157233677)], 
                        [new (0.999555889618897, 0.000453054971183), new (0.956447514042325,  -0.042557613240989), 
                         new (0.999555889618897,  -0.000453054971183), new (0.956447514042325, 0.042557613240989)],
                    0.9569683185241783)).SetName("BandStop: 2nd Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandStop, 666, 0.2581, 42.42, 2, 
                new Zpk([new (0.999506276184116, 0.031419800581176), 
                        new (0.999506276184116, -0.031419800581176),
                        new (0.999506276184116, 0.031419800581176), 
                        new (0.999506276184116, -0.031419800581176)], 
                        [new (0.998278433309316, 0.001739580088654), new (0.724574682214599, -0.216644660192020), 
                         new (0.998278433309316, -0.001739580088654), new (0.724574682214599, 0.216644660192020)],
                    0.75434645652128662)).SetName("BandStop: 2nd Order, Low: 0.2581Hz, High: 42.42Hz, Sampling: 666Hz");
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
                    0.93968217530861076)).SetName("BandStop: 3rd Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            
            
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
                    0.92192796149812739)).SetName("BandStop: 4th Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
        }
    }
    
    //All testData are created with SciPy
    //The order of poles and zeros in SciPy result might differ but doesn't matter
    //The tests are not as extensive as for the Butterworth Filter but as only the internally called LowPassPrototype function is different this is ok
    public static IEnumerable<TestCaseData> ZpkTestDataBessel
    {
        get
        {
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 2, 
                new Zpk([new (-1.0, 0.0), new (-1.0, 0.0)], 
                    [new (0.9465547965606139, 0.02977609003147858), new (0.9465547965606139, -0.02977609003147858)],
                    0.0009357513270600223)).SetName("LowPass: 2nd Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 3, 
                new Zpk([new (-1.0, 0.0), 
                         new (-1.0, 0.0), 
                         new (-1.0, 0.0)], 
                        [new (0.7104146724701721, 0.2143540530450776),
                         new (0.7104146724701721, -0.2143540530450776), 
                         new (0.6793041959000489, 0.0)],
                    0.005203582937450832)).SetName("LowPass: 3rd Order, 42.42Hz with 666Hz Sampling");
            
            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 2, 
                new Zpk([new (1.0, 0.0), new (1.0, 0.0)], 
                    [new (0.9465547965606139, -0.029776090031478592), new (0.9465547965606139, 0.029776090031478592)],
                    0.9474905478876746)).SetName("HighPass: 2nd Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 666, 0, 42.42, 3, 
                new Zpk([new (1.0, 0.0), 
                         new (1.0, 0.0), 
                         new (1.0, 0.0)], 
                        [new (0.7262969509908046, -0.20528479165595986),
                         new (0.7262969509908046, 0.20528479165595986), 
                         new (0.6455586740081618, 0.0)],
                    0.6216597747378438)).SetName("HighPass: 3rd Order, 42.42Hz with 666Hz Sampling");
            
            yield return new TestCaseData(FrequencyFilterType.BandPass, 1000, 0.1, 10, 2, 
                new Zpk([new (1.0, 0.0), 
                        new (1.0, 0.0),
                        new (-1.0, 0.0), 
                        new (-1.0, 0.0)], 
                        [new (0.9994505814507483, -0.0003236912046215028), new (0.9475908107123208,  0.02981701542245074), 
                         new (0.9994505814507483,  0.0003236912046215028), new (0.9475908107123208, -0.02981701542245074)],
                    0.0009176085114121442)).SetName("BandPass: 2nd Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandPass, 666, 0.2581, 42.42, 3, 
                new Zpk([new (1.0, 0.0), 
                        new (1.0, 0.0), 
                        new (1.0, 0.0),
                        new (-1.0, 0.0), 
                        new (-1.0, 0.0), 
                        new (-1.0, 0.0)], 
                        [new (0.9982892319685228, -0.0016481989407214756), 
                        new (0.7129720125320612, 0.21494339447695793), 
                        new (0.9982892319685228, 0.0016481989407214756),
                        new (0.7129720125320612, -0.21494339447695793),
                        new (0.9973837394530889, 0),
                        new (0.6827747121334972, 0)],
                    0.005121030868663763)).SetName("BandPass: 3rd Order, Low: 0.2581Hz, High: 42.42Hz, Sampling: 666Hz");
            
            yield return new TestCaseData(FrequencyFilterType.BandStop, 1000, 0.1, 10, 2, 
                new Zpk([new (0.9999802544889885, 0.006284157233677085), 
                        new (0.9999802544889885, -0.006284157233677085),
                        new (0.9999802544889885, 0.006284157233677085), 
                        new (0.9999802544889885, -0.006284157233677085)], 
                        [new (0.9994505814507483, 0.0003236912046215028), new (0.9475908107123208,  -0.029817015422450747), 
                         new (0.9994505814507483,  -0.0003236912046215028), new (0.9475908107123208, 0.029817015422450747)],
                    0.9479974467608678)).SetName("BandStop: 2nd Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandStop, 666, 0.2581, 42.42, 3, 
                new Zpk([new (0.9995062761841162, 0.03141980058117618), 
                         new (0.9995062761841162, -0.03141980058117618), 
                         new (0.9995062761841162, 0.03141980058117618),
                         new (0.9995062761841162, -0.03141980058117618), 
                         new (0.9995062761841162, 0.03141980058117618), 
                         new (0.9995062761841162, -0.03141980058117618)],
                        [new (0.9981843743159441, 0.0017516038265944186), 
                         new (0.7288680547781037, -0.20600205727233625), 
                         new (0.9981843743159441, -0.0017516038265944186),
                         new (0.7288680547781037, 0.20600205727233625),
                         new (0.9976835095973491, 0),
                         new (0.6488860208193175, 0)],
                    0.6234143718105545)).SetName("BandStop: 3rd Order, Low: 0.2581Hz, High: 42.42Hz, Sampling: 666Hz");
        }
    }
    
    //All testData are created with MatLab
    public static IEnumerable<TestCaseData> TfTestDataButterworth
    {
        get
        {
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 1, 
                new TransferFunction([0.030468747091254,0.030468747091254], 
                    [1,-0.939062505817492]),
                1.0e-15).SetName("LowPass: 1st Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 1, 
                new TransferFunction([0.168616104038892,0.168616104038892], 
                    [1,-0.662767791922216]),
                1.0e-15).SetName("LowPass: 1st Order, 42.42Hz with 666Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 2, 
                new TransferFunction([9.446918438401597e-04,0.001889383687680,9.446918438401597e-04], 
                    [1,-1.911197067426073,0.914975834801433]),
                1.0e-15).SetName("LowPass: 2nd Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 2, 
                new TransferFunction([0.030975005453286,0.061950010906573,0.030975005453286], 
                    [1,-1.444124650833721,0.568024672646866]),
                1.0e-15).SetName("LowPass: 2nd Order, 42.42Hz with 666Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 3, 
                new TransferFunction([2.914649446569766e-05,8.743948339709297e-05,8.743948339709297e-05,2.914649446569766e-05], 
                    [1,-2.874356892677485,2.756483195225695,-0.881893130592486]),
                1.0e-15).SetName("LowPass: 3rd Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 3, 
                new TransferFunction([0.005575604877852,0.016726814633557,0.016726814633557,0.005575604877852], 
                    [1,-2.204419359012364,1.695676006711682,-0.446651808676499]),
                1.0e-15).SetName("LowPass: 3rd Order, 42.42Hz with 666Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 4, 
                new TransferFunction([8.984861463970445e-07,3.593944585588178e-06,5.390916878382267e-06,3.593944585588178e-06,8.984861463970445e-07], 
                    [1,-3.835825540647349,5.520819136622230,-3.533535219463015,0.848555999266477]),
                1.0e-14).SetName("LowPass: 4th Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 4, 
                new TransferFunction([9.988492606903899e-04,0.003995397042762,0.005993095564142,0.003995397042762,9.988492606903899e-04], 
                    [1,-2.957414698739313,3.382282370796745,-1.757401190455578,0.348515106569192]),
                1.0e-15).SetName("LowPass: 4th Order, 42.42Hz with 666Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 10, 
                new TransferFunction([7.68584984998482e-16,7.68584984998482e-15,3.45863243249317e-14,9.22301981998179e-14,1.61402846849681e-13,1.93683416219618e-13,1.61402846849681e-13,9.22301981998179e-14,3.45863243249317e-14,7.68584984998482e-15,7.68584984998482e-16], 
                    [1,-9.59835477144932,41.4655792756444,-106.173354913648,178.440055558469,-205.679548276819,164.666485668553,-90.4147875793786,32.5851033631510,-6.96033549559011,0.669157171068017]),
                1.0e-12).SetName("LowPass: 10th Order, 10Hz with 1000Hz Sampling");
            
            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 1, 
                new TransferFunction([0.969531252908746,-0.969531252908746], 
                    [1,-0.939062505817492]),
                1.0e-15).SetName("HighPass: 1st Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 2, 
                new TransferFunction([0.956543225556877,-1.913086451113754,0.956543225556877], 
                    [1,-1.911197067426073,0.914975834801434]),
                1.0e-15).SetName("HighPass: 2nd Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 666, 0, 42.42, 2, 
                new TransferFunction([0.753037330870147,-1.506074661740293,0.753037330870147], 
                    [1,-1.444124650833721,0.568024672646866]),
                1.0e-15).SetName("HighPass: 2nd Order, 42.42Hz with 666Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 3, 
                new TransferFunction([0.939091652311958,-2.817274956935875,2.817274956935875,-0.939091652311958], 
                    [1,-2.874356892677485,2.756483195225695,-0.881893130592486]),
                1.0e-15).SetName("HighPass: 3rd Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 666, 0, 42.42, 3, 
                new TransferFunction([0.668343396800068,-2.005030190400204,2.005030190400204,-0.668343396800068], 
                    [1,-2.204419359012364,1.695676006711682,-0.446651808676499]),
                1.0e-15).SetName("HighPass: 3rd Order, 42.42Hz with 666Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 4, 
                new TransferFunction([0.921170993499941,-3.684683973999764,5.527025960999646,-3.684683973999764,0.921170993499941], 
                    [1,-3.835825540647349,5.520819136622230,-3.533535219463015,0.848555999266477]),
                1.0e-14).SetName("HighPass: 4th Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 6, 
                new TransferFunction([0.885673290152358,-5.314039740914147,13.285099352285368,-17.713465803047157,13.285099352285368,-5.314039740914147,0.885673290152358], 
                    [1,-5.757244186246575,13.815510806058022,-17.687376179894024,12.741617329229229,-4.896924891433742,0.784417176889303]),
                1.0e-13).SetName("HighPass: 6th Order, 10Hz with 1000Hz Sampling");
            
            yield return new TestCaseData(FrequencyFilterType.BandPass, 1000, 0.1, 10, 1, 
                new TransferFunction([0.030173061366710,0,-0.030173061366710], 
                    [1,-1.939615577809589,0.939653877266580]),
                1.0e-15).SetName("BandPass: 1st Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandPass, 1000, 0.1, 10, 2, 
                new TransferFunction([9.262918135802441e-04,0,-0.001852583627160,0,9.262918135802441e-04], 
                    [1,-3.912006807322443,5.739806162382575,-3.743588574243218,0.915789220675518]),
                1.0e-15).SetName("BandPass: 2nd Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandPass, 666, 0.2581, 42.42, 2, 
                new TransferFunction([0.030642307207318,0,-0.061284614414636,0,0.030642307207318], 
                    [1,-3.445706231047832,4.461815349752635,-2.586085910634371,0.569977527457210]),
                1.0e-14).SetName("BandPass: 2nd Order, Low: 0.2581Hz, High: 42.42Hz, Sampling: 666Hz");
            yield return new TestCaseData(FrequencyFilterType.BandPass, 1000, 0.1, 10, 3, 
                new TransferFunction([2.829804203014804e-05,0,-8.489412609044412e-05,0,8.489412609044412e-05,0,-2.829804203014804e-05], 
                    [1,-5.875496902294781,14.385226111375008,-18.785699218122680,13.800710301273888,-5.407742882023321,0.883002589791942]),
                1.0e-13).SetName("BandPass: 3rd Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandPass, 666, 0.2581, 42.42, 3, 
                new TransferFunction([0.005485360217653,0,-0.016456080652960,0,0.016456080652960,0,-0.005485360217653], 
                    [1,-5.206634951930720,11.320585736404603,-13.170608835270183,8.654879882935717,-3.047100917415440,0.448879085921117]),
                1.0e-13).SetName("BandPass: 3rd Order, Low: 0.2581Hz, High: 42.42Hz, Sampling: 666Hz");
            yield return new TestCaseData(FrequencyFilterType.BandPass, 1000, 0.1, 10, 4, 
                new TransferFunction([8.637688106445837e-07,0,-3.455075242578335e-06,0,5.182612863867502e-06,0,-3.455075242578335e-06,0,8.637688106445837e-07], 
                    [1,-7.837312273068069,26.874437766916110,-52.662441077811536,64.501583293299400,-50.564748152342660,24.776146706053960,-6.937617429240048,0.849951166192838]),
                1.0e-13).SetName("BandPass: 4th Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandPass, 1000, 0.1, 10, 6, 
                new TransferFunction([8.04177329323391e-10,0,-4.82506397594035e-09,0,1.20626599398509e-08,0,-1.60835465864678e-08,0,1.20626599398509e-08,0,-4.82506397594035e-09,0,8.04177329323391e-10], 
                    [1,-11.7594393896669,63.3828205899279,-207.056829190520,456.592147549707,-716.014798130393,818.765781725918,-687.893505030881,421.431558060682,-183.606464164708,53.9970886401915,-9.62468561325294,0.786324952996040]),
                1.0e-12).SetName("BandPass: 6th Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            
            yield return new TestCaseData(FrequencyFilterType.BandStop, 1000, 0.1, 10, 1, 
                new TransferFunction([0.969826938633303,-1.939615577809614,0.969826938633303], 
                    [1,-1.939615577809589,0.939653877266580]),
                1.0e-13).SetName("BandStop: 1st Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandStop, 1000, 0.1, 10, 2, 
                new TransferFunction([0.956968318523830,-3.827797690781438,5.741658746007646,-3.827797690781438,0.956968318523830], 
                    [1,-3.912006807322443,5.739806162382575,-3.743588574243218,0.915789220675518]),
                1.0e-11).SetName("BandStop: 2nd Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandStop, 666, 0.2581, 42.42, 2, 
                new TransferFunction([0.754346456521240,-3.015896070840912,4.523099964166987,-3.015896070840912,0.754346456521240], 
                    [1,-3.445706231047832,4.461815349752635,-2.586085910634371,0.569977527457210]),
                1.0e-12).SetName("BandStop: 2nd Order, Low: 0.2581Hz, High: 42.42Hz, Sampling: 666Hz");
            yield return new TestCaseData(FrequencyFilterType.BandStop, 1000, 0.1, 10, 3, 
                new TransferFunction([0.939682175308134,-5.637981724820364,14.094787325904669,-18.792975552784820,14.094787325904669,-5.637981724820364,0.939682175308134], 
                    [1,-5.875496902294781,14.385226111375008,-18.785699218122680,13.800710301273888,-5.407742882023321,0.883002589791942]),
                1.0e-11).SetName("BandStop: 3rd Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandStop, 666, 0.2581, 42.42, 3, 
                new TransferFunction([0.670006847052918,-4.018056292294336,10.042165505577291,-13.388232120026656,10.042165505577291,-4.018056292294336,0.670006847052918], 
                    [1,-5.206634951930720,11.320585736404603,-13.170608835270183,8.654879882935717,-3.047100917415440,0.448879085921117]),
                1.0e-12).SetName("BandStop: 3rd Order, Low: 0.2581Hz, High: 42.42Hz, Sampling: 666Hz");
            yield return new TestCaseData(FrequencyFilterType.BandStop, 1000, 0.1, 10, 4, 
                new TransferFunction([0.921927961495561,-7.375278060454769,25.813109141444090,-51.625781405612190,64.532044726254610,-51.625781405612190,25.813109141444090,-7.375278060454770,0.921927961495561], 
                    [1,-7.837312273068069,26.874437766916110,-52.662441077811536,64.501583293299400,-50.564748152342660,24.776146706053960,-6.937617429240048,0.849951166192838]),
                1.0e-9).SetName("BandStop: 4th Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandStop, 1000, 0.1, 10, 6, 
                new TransferFunction([0.886749656320719,-10.6407857639474,58.5233762188989,-195.075469520952,438.915867031428,-702.261605468388,819.303735693279,-702.261605468388,438.915867031428,-195.075469520952,58.5233762188989,-10.6407857639474,0.886749656320719], 
                    [1,-11.7594393896669,63.3828205899279,-207.056829190520,456.592147549707,-716.014798130393,818.765781725918,-687.893505030881,421.431558060682,-183.606464164708,53.9970886401915,-9.62468561325294,0.786324952996040]),
                1.0e-8).SetName("BandStop: 6th Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
        }
    }
    
    //All testData are created with SciPy
    public static IEnumerable<TestCaseData> TfTestDataBessel
    {
        get
        {
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 1, 
                new TransferFunction([0.03046874709125383,0.03046874709125383], 
                    [1,-0.9390625058174923]),
                1.0e-15).SetName("LowPass: 1st Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 2, 
                new TransferFunction([0.0009357513270600223,0.0018715026541200445,0.0009357513270600223], 
                    [1,-1.8931095931212278,0.896852598429468]),
                1.0e-15).SetName("LowPass: 2nd Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 666, 42.42, 0, 3, 
                new TransferFunction([0.005203582937450832,0.015610748812352497,0.015610748812352497,0.005203582937450832], 
                    [1,-2.100133540840393,1.5158120025936475,-0.37404979825364776]),
                1.0e-15).SetName("LowPass: 3rd Order, 42.42Hz with 666Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 4, 
                new TransferFunction([8.846036313160676e-07,3.5384145252642704e-06,5.307621787896406e-06,3.5384145252642704e-06,8.846036313160676e-07], 
                    [1,-3.805643184431058,5.4337661221168725,-3.4498535363729785,0.821744752345264]),
                1.0e-14).SetName("LowPass: 4th Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 6, 
                new TransferFunction([8.367960734579576e-10,5.020776440747746e-09,1.2551941101869365e-08,1.673592146915915e-08,1.2551941101869365e-08,5.020776440747746e-09,8.367960734579576e-10], 
                    [1,-5.719477889709566,13.634615252897994,-17.340737492937027,12.409444495524522,-4.7377439340260725,0.7538996218050958]),
                1.0e-14).SetName("LowPass: 6th Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.LowPass, 1000, 10, 0, 8, 
                new TransferFunction([7.917182989393493e-13,6.333746391514794e-12,2.2168112370301782e-11,4.4336224740603563e-11,5.542028092575445e-11,4.4336224740603563e-11,2.2168112370301782e-11,6.333746391514794e-12,7.917182989393493e-13], 
                    [1,-7.633683992720539,25.50068955150787,-48.68958172113381,58.11704573517522,-44.4071889723683,21.212131266066173,-5.791319289443753,0.6919074231198276]),
                1.0e-13).SetName("LowPass: 8th Order, 10Hz with 1000Hz Sampling");
            
            yield return new TestCaseData(FrequencyFilterType.HighPass, 1000, 0, 10, 2, 
                new TransferFunction([0.9474905478876746,-1.8949810957753492,0.9474905478876746], 
                    [1,-1.8931095931212278,0.896852598429468]),
                1.0e-14).SetName("HighPass: 2nd Order, 10Hz with 1000Hz Sampling");
            yield return new TestCaseData(FrequencyFilterType.HighPass, 666, 0, 42.42, 3, 
                new TransferFunction([0.6216597747378438,-1.8649793242135315,1.8649793242135315,-0.6216597747378438], 
                    [1,-2.0981525759897712,1.5073836999393595,-0.36774192197361966]),
                1.0e-14).SetName("HighPass: 3rd Order, 42.42Hz with 666Hz Sampling");
            
            yield return new TestCaseData(FrequencyFilterType.BandPass, 1000, 0.1, 10, 2, 
                new TransferFunction([0.0009176085114121442,0,-0.0018352170228242883,0,0.0009176085114121442], 
                    [1,-3.894082784326138,5.68599971546863,-3.6897470402086108,0.8978301105445591]),
                1.0e-15).SetName("BandPass: 2nd Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandPass, 666, 0.2581, 42.42, 3, 
                new TransferFunction([0.005121030868663763,0,-0.015363092605991288,0,0.015363092605991288,0,-0.005121030868663763], 
                    [1,-5.102680940587755,10.829491473162264,-12.248476931257136,7.795542193957259,-2.6502141839302022,0.3763383892578121]),
                1.0e-14).SetName("BandPass: 3rd Order, Low: 0.2581Hz, High: 42.42Hz, Sampling: 666Hz");
            
            yield return new TestCaseData(FrequencyFilterType.BandStop, 1000, 0.1, 10, 2, 
                new TransferFunction([0.9479974467608678,-3.791914912267376,5.687834932491456,-3.791914912267375,0.9479974467608675], 
                    [1,-3.894082784326138,5.68599971546863,-3.6897470402086108,0.8978301105445591]),
                1.0e-15).SetName("BandStop: 2nd Order, Low: 0.1Hz, High: 10Hz, Sampling: 1000Hz");
            yield return new TestCaseData(FrequencyFilterType.BandStop, 666, 0.2581, 42.42, 3, 
                new TransferFunction([0.6234143718105545,-3.738639463728164,9.3438303322035,-12.45721047997155,9.3438303322035,-3.7386394637281635,0.6234143718105541], 
                    [1,-5.100674388604762,10.815046111927547,-12.21089383229871,7.749395392409139,-2.6229211865244104,0.3700479036914302]),
                1.0e-14).SetName("BandStop: 3rd Order, Low: 0.2581Hz, High: 42.42Hz, Sampling: 666Hz");
        }
    }
    
    [TestCaseSource(nameof(ZpkTestDataButterworth))]
    public void CalcZpkTestsButterworth(FrequencyFilterType frequencyFilterType, double sourceFrequency, double lowCutOff, double highCutoff, int order, Zpk expectedZpk)
    {
        //Arrange + Act
        var zpk = FilterTools.CalcZpk(FrequencyFilterDesignType.Butterworth, frequencyFilterType, sourceFrequency, lowCutOff, highCutoff, order);

        //Assert
        CompareZpk(zpk, expectedZpk);
    }
    
    [TestCaseSource(nameof(TfTestDataButterworth))]
    public void CalcTransferFunctionTestsButterworth(FrequencyFilterType frequencyFilterType, double sourceFrequency, double lowCutOff, double highCutoff, int order, TransferFunction expectedTf, double tolerance)
    {
        //Arrange + Act
        var tf = FilterTools.CalcTransferFunction(FrequencyFilterDesignType.Butterworth, frequencyFilterType, sourceFrequency, lowCutOff, highCutoff, order);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(tf.A, Is.EqualTo(expectedTf.A).Within(tolerance), "A incorrect");
            Assert.That(tf.B, Is.EqualTo(expectedTf.B).Within(tolerance), "B incorrect");
        });
    }
    
    [TestCaseSource(nameof(ZpkTestDataBessel))]
    public void CalcZpkTestsBessel(FrequencyFilterType frequencyFilterType, double sourceFrequency, double lowCutOff, double highCutoff, int order, Zpk expectedZpk)
    {
        //Arrange + Act
        var zpk = FilterTools.CalcZpk(FrequencyFilterDesignType.Bessel, frequencyFilterType, sourceFrequency, lowCutOff, highCutoff, order);

        //Assert
        CompareZpk(zpk, expectedZpk);
    }
    
    [TestCaseSource(nameof(TfTestDataBessel))]
    public void CalcTransferFunctionTestsBessel(FrequencyFilterType frequencyFilterType, double sourceFrequency, double lowCutOff, double highCutoff, int order, TransferFunction expectedTf, double tolerance)
    {
        //Arrange + Act
        var tf = FilterTools.CalcTransferFunction(FrequencyFilterDesignType.Bessel, frequencyFilterType, sourceFrequency, lowCutOff, highCutoff, order);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(tf.A, Is.EqualTo(expectedTf.A).Within(tolerance), "A incorrect");
            Assert.That(tf.B, Is.EqualTo(expectedTf.B).Within(tolerance), "B incorrect");
        });
    }
    
    [TestCase(12)]
    public void CalcButterworthZpkOrderOutOfRange(int order)
    {
        //Arrange

        //Act + Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => FilterTools.CalcZpk(FrequencyFilterDesignType.Butterworth, FrequencyFilterType.LowPass, 1000, 10, 0, order));
    }
    
    [TestCase(12)]
    public void CalcButterworthTransferFunctionOrderOutOfRange(int order)
    {
        //Arrange

        //Act + Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => FilterTools.CalcTransferFunction(FrequencyFilterDesignType.Butterworth, FrequencyFilterType.LowPass, 1000, 10, 0, order));
    }
    
    [TestCase(10)]
    public void CalcBesselZpkOrderOutOfRange(int order)
    {
        //Arrange

        //Act + Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => FilterTools.CalcZpk(FrequencyFilterDesignType.Bessel, FrequencyFilterType.LowPass, 1000, 10, 0, order));
    }
    
    [TestCase(10)]
    public void CalcBesselTransferFunctionOrderOutOfRange(int order)
    {
        //Arrange

        //Act + Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => FilterTools.CalcTransferFunction(FrequencyFilterDesignType.Bessel, FrequencyFilterType.LowPass, 1000, 10, 0, order));
    }
    
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
        Assert.Throws<ArgumentOutOfRangeException>(() => FilterTools.CalcTransferFunction(FrequencyFilterDesignType.Butterworth, frequencyFilterType, sourceFrequency, lowCutOff, highCutoff, 2));
    }

    private static void CompareZpk(Zpk actual, Zpk expected)
    {
        Assert.Multiple(() =>
        {
            Assert.That(actual.K, Is.EqualTo(expected.K).Within(1.0e-14), "Gain incorrect");

            Assert.That(actual.Z, Has.Length.EqualTo(expected.Z.Length), "Zeros length not matching");
            for (var i = 0; i < actual.Z.Length; i++)
            {
                Assert.That(actual.Z[i].Real, Is.EqualTo(expected.Z[i].Real).Within(1.0e-14), "Real of zero does not match");
                Assert.That(actual.Z[i].Imaginary, Is.EqualTo(expected.Z[i].Imaginary).Within(1.0e-14), "Imag of zero does not match");
            }
        
            Assert.That(actual.P, Has.Length.EqualTo(expected.P.Length), "Poles length not matching");
            for (var i = 0; i < actual.P.Length; i++)
            {
                Assert.That(actual.P[i].Real, Is.EqualTo(expected.P[i].Real).Within(1.0e-14), "Real of pole does not match");
                Assert.That(actual.P[i].Imaginary, Is.EqualTo(expected.P[i].Imaginary).Within(1.0e-14), "Imag of pole does not match");
            }
        });
    }
}