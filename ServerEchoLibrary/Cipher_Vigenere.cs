using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerEchoLibrary
{
    class Cipher_Vigenere
    {
        public static string szyfrowanie(string klucz, string tekst_jawny)
        {
            char[] alfabet = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '_' };
            string szyfrogram = "";
            int index = 0;
            int ik = 0;
            List<int> indeksy_klucza = new List<int>();

            for (int i = 0; i < klucz.Length; i++)
            {
                for (int j = 0; j < alfabet.Length; j++)
                {
                    if (klucz[i] == alfabet[j]) indeksy_klucza.Add(j);
                }
            }

            for (int i = 0; i < tekst_jawny.Length; i++)
            {
                for (int j = 0; j < alfabet.Length; j++)
                {
                    if (tekst_jawny[i] == alfabet[j])
                    {
                        index = (indeksy_klucza[ik] + j) % 27;
                        szyfrogram.Append(alfabet[index]);
                    }
                }
                ik++;
                if (ik >= indeksy_klucza.Count) ik = 0;
            }
            return szyfrogram;
        }

        public static string deszyfrowanie(string klucz, string szyfrogram)
        {
            char[] alfabet = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '_' };
            string tekst_jawny = "";
            int index = 0;
            int ik = 0;
            List<int> indeksy_klucza = new List<int>();

            for (int i = 0; i < klucz.Length; i++)
            {
                for (int j = 0; j < alfabet.Length; j++)
                {
                    if (szyfrogram[i] == alfabet[j])
                    {
                        index = (j + 27 - indeksy_klucza[ik]) % 27;
                        tekst_jawny.Append(alfabet[index]);
                    }
                }
                ik++;
                if (ik >= indeksy_klucza.Count) ik = 0;
            }

            return tekst_jawny;
        }
    }
}
