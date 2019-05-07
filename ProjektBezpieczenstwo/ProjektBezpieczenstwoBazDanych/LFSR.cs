using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektBezpieczenstwoBazDanych
{
    public class LFSR
    {
        List<int> generator;
        string[] wKey;
        int[] iKey;
        public LFSR(string s, string w)
        {
            string wiel_bez_spacji = w.Replace(" ", "");
            string output = "";
            wKey = wiel_bez_spacji.Split('+');
            iKey = new int[wKey.Length];
            for (int i = 0; i < wKey.Length; i++)
            {

                iKey[i] = int.Parse(wKey[i]);
            }
            generator = new List<int>();
            string tmp = "";
            for (int i = 0; i < s.Length; i++)
            {
                tmp += s[i];
                generator.Add(int.Parse(tmp));
                tmp = "";
            }

        }

        public string wypisz()
        {
            string wyjscie = "";


            for (int i = 0; i < generator.Count; i++)
            {
                wyjscie += generator[i];

            }

            int number = Convert.ToInt32(wyjscie, 2);
            wyjscie += " - " + number;

            return wyjscie;

        }
        public int xor(int a, int b)
        {
            if (a == b) return 0;
            else return 1;
        }
        public void iteracja()
        {
            int liczba = generator[iKey[0] - 1];
            for (int i = 1; i < iKey.Length; i++)
            {
                liczba = xor(liczba, generator[iKey[i] - 1]);
            }
            for (int i = generator.Count() - 1; i > 0; i--)
            {
                generator[i] = generator[i - 1];
            }
            generator[0] = liczba;
        }
    }

    public class LFSR2
    {
        public string zwrocWynik()
        {
            return wynik;
        }
        public string seed;
        string wiel;
        public string wynik;
        string ziarno;
        List<int> generator;
        public LFSR2(string s, string w, string z)
        {
            wynik = "";
            seed = s;
            wiel = "";
            for (int i = w.Length - 1; i >= 0; i--)
            {
                wiel += w[i];
            }
            ziarno = z;
            generator = new List<int>();
            string tmp = "";
            for (int i = s.Length - 1; i >= 0; i--)
            {
                tmp += s[i];
                generator.Add(int.Parse(tmp));
                tmp = "";
            }
        }

        public string wypisz()
        {
            string wyjscie = "";
            for (int i = generator.Count() - 1; i >= 0; i--)
            {
                wyjscie += generator[i];
            }
            return wyjscie;

        }

        public int xor(int a, int b)
        {
            if (a == b) return 0;
            else return 1;

        }

        public void iteracja()
        {
            string tmp = "";
            for (int j = ziarno.Length - 1; j >= 0; j--)
            {
                tmp += ziarno[j];

                int liczba = 0;
                for (int i = 0; i < wiel.Length; i++)
                {
                    if (wiel[i] == '1')
                    {
                        liczba = xor(liczba, generator[i]);
                    }

                }

                wynik += xor(liczba, int.Parse(tmp));
                for (int i = 0; i < generator.Count - 1; i++)
                {
                    generator[i] = generator[i + 1];
                }
                generator[generator.Count - 1] = liczba;

                tmp = "";
            }

            for (int i = wynik.Length - 1; i >= 0; i--)
            {
                tmp += wynik[i];
            }

            wynik = tmp;
            seed = "";

            for (int i = generator.Count - 1; i >= 0; i--)
            {
                seed += generator[i];
            }
        }


    }
}
