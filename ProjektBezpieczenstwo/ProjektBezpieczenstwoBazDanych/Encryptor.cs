using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.IO;
using Microsoft.Win32;
using System.Windows;

namespace ProjektBezpieczenstwoBazDanych
{
    class Encryptor
    {
        public string ReadTextFromFile()
        {
            string text = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".txt";
            if (openFileDialog.ShowDialog() == true)
                text = File.ReadAllText(openFileDialog.FileName);

            return text;
        }

        public void SaveToFileAsBinaryString(string text)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.DefaultExt = ".txt";
            if (save.ShowDialog() == true)
            {
                using (StreamWriter write = new StreamWriter(File.Create(save.FileName)))
                    write.Write(text);
            }
        }

        public void SaveToFileAsTextString(string text)
        {
            //throw new Exception("This operation is not supported.");
            SaveFileDialog save = new SaveFileDialog();
            save.DefaultExt = ".txt";
            if (save.ShowDialog() == true)
            {
                using (StreamWriter write = new StreamWriter(File.Create(save.FileName)))
                    write.Write(text);
            }
        }

        public void ShowInfoPopUp()
        {
            String infoText = "Szyfrator strumieniowy\n" +
                "Autorzy: Adam Zarzecki && Karol Kwiecień\n\n " +
                "Program szyfruje zadany tekst za pomocą zadanego klucza.\n" +
                "Tekst: Aby wczytać tekst, należy wybrać odpowiedni plik za pomocą przycisku \'Załaduj tekst z pliku\'." +
                " Można też go wpisać w pierwszy polu tekstowym lub załadować jako ciąg bitów (z pliku tekstowego).\n" +
                "Klucz: Klucz musi być wczytany z pliku tekstowego.\n" +
                "UWAGA: Jeżeli klucz jest za krótki, to zostanie powielony tak wiele razy, żeby liczba bitów była równa liczbie bitów zaszyfrowanego tekstu." +
                "Szyfrogram binarnie: Zaszyfrowany tekst zostanie wpisany w tym polu jako ciąg zer i jedynek.\n" +
                "Szyfrogram: Tutaj pojawi się tekst, w którym ciągi ośmiobitowe zostały zamienione na odpowiednie znaki.\n" +
                "Zapis szyfrogramu: Szyfrogram można zapisać jako ciąg bitów albo znaków używając odpowiednio przycisków \'Zapisz szyfrogram jako bity\' lub \'Zapisz szyfrogram\'" +
                "\n\nDESZYFRACJA\n" +
                "Aby deszyfrować tekst należy go wczytać jako ciąg bitów albo tekst i kliknąć przycisk \'Zaszyfruj/Deszyfruj\'.\n\n" +
                "UWAGA: Program obsługuje tylko znaki ASCII.";

            MessageBox.Show(infoText, "Informacje o programie");
        }

        public void ShowErrorPopUp(string message)
        {
            MessageBox.Show(message, "Wykryto Błąd!");
        }

        public void ShowPopUp(string message, string messageBoxTitle)
        {
            MessageBox.Show(message, "Wykryto Błąd!");
        }

        public string ConvertStringToBinaryString(string text)
        {
            string result = "";
            foreach (char letter in text)
                result += Convert.ToString(letter, 2).PadLeft(8, '0');
            return result;
        }

        public bool checkIfStringContainsOnlyAsciiChars(string text)
        {
            foreach (char letter in text)
                if (letter > 127)
                    return false;
            return true;
        }

        public void RepeatKeyToMatchLength(string binaryText, ref string binaryKey)
        {
            int i = 0;
            string binaryKeyCopy = binaryKey;
            int binaryKeyLength = binaryKeyCopy.Length;

            while (binaryKey.Length < binaryText.Length)
            {
                binaryKey += binaryKeyCopy[i];
                if (i == binaryKeyLength - 1)
                    i = 0;
                else
                    i++; 
            }
        }

        public string CipherBinaryStringUsingBinaryString(string binaryText, string binaryKey)
        {
            string result = "";
            for(int i = 0; i < binaryText.Length; i++)
            {
                if (binaryText[i] != binaryKey[i])
                    result += 1;
                else
                    result += 0;
            }
            
            return result;
        }

        public bool checkIfStringIsBinary(string binaryString)
        {
            foreach (char letter in binaryString)
                if (letter != '1' && letter != '0')
                    return false;
            return true;
        }
        /*
        private static Char ConvertToChar(String value)
        {
            int result = 0;

            foreach (Char ch in value)
                result = result * 2 + ch - '0';

            return (Char)result;
        }

        public string convertBitStringToString(string zerosAndOnes)
        {
            StringBuilder Sb = new StringBuilder();

            if (zerosAndOnes.Length % 8 != 0)
            {
                return null;
            }
            else
            {
                for (int i = 0; i < zerosAndOnes.Length / 8; ++i)
                {
                    Sb.Append(ConvertToChar(zerosAndOnes.Substring(8 * i, 8)));
                }

            }
            return Sb.ToString();
        }
        */

        public string convertBitStringToString(string zerosAndOnes)
        {
            var list = new List<Byte>();

            for (int i = 0; i < zerosAndOnes.Length; i += 8)
            {
                string t = zerosAndOnes.Substring(i, 8);

                list.Add(Convert.ToByte(t, 2));
            }
            string text = Encoding.ASCII.GetString(list.ToArray());
            return text;
        }

    }
}
