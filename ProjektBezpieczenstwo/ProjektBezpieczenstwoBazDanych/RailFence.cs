using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektBezpieczenstwoBazDanych
{
   public class RailFence
    {
        public string Szyfrowanie(int N, string input)
        {
            input = input.Replace(" ", "");
            if (N == 1) return input;
            List<string> plotek = new List<string>();
            int n = 0;
            int inc = 1;
            for (int i = 0; i < N; i++)
            {
                plotek.Add("");
            }

            foreach (char c in input)
            {
                if (n + inc == N)
                {
                    inc = -1;
                }
                else if (n + inc == -1)
                {
                    inc = 1;
                }
                plotek[n] += c;
                n += inc;
            }

            string output = "";
            foreach (string s in plotek)
            {
                output += s;
            }
            return output;
        }
        public string Deszyfrowanie(string input, int key)
        {
            input = input.Replace(" ", "");

            List<List<int>> railFence = new List<List<int>>();
            for (int i = 0; i < key; i++) railFence.Add(new List<int>());

            int factor = 1;
            int index = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (index + factor == key)
                    factor = -1;
                else if (index + factor == -1)
                    factor = 1;

                railFence[index].Add(i);
                index += factor;
            }

            char[] result = new char[input.Length];
            int position = 0;
            foreach (var row in railFence)
            {
                foreach (var i in row)
                {
                    result[i] = input[position];
                    position++;
                }
            }

            return new string(result);
        }
    }
}
