using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Numerics;
using System.IO;
using Microsoft.Win32;

namespace ProjektBezpieczenstwoBazDanych
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LFSR lfsr;
        DES desik;
        DES odszyfrowanie;
        public string pliksciezkades;
        public string pliksciezkadesklucz;

        public char[] Alphabet { get; set; }
        public MainWindow()
        {
            encryptor = new Encryptor();
            InitializeComponent();
            Alphabet = new char[] { 'A', 'Ą', 'B', 'C', 'Ć', 'D', 'E', 'Ę', 'F', 'G', 'H', 'I', 'J', 'K',
                'L', 'Ł', 'M', 'N', 'Ń', 'O', 'Ó', 'P', 'R', 'S', 'Ś', 'T', 'U', 'W', 'Y', 'Z', 'Ź', 'Ż' };
        }
        #region RAIL FENCE
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            RailFence rail = new RailFence();
            string input = textBox1.Text;
            string output;
            int N = int.Parse(textBox2.Text);
            output = rail.Szyfrowanie(N, input);

            textBox3.Text = output;
        }
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            RailFence rail = new RailFence();
            int klucz;
            if (int.TryParse(textBox5.Text, out klucz))
            {
                if (String.IsNullOrEmpty(textBox4.Text) || klucz == 1)
                    textBox6.Text = textBox4.Text;
                else textBox6.Text = rail.Deszyfrowanie(textBox4.Text, klucz);
            }
            else MessageBox.Show("Niepoprawny klucz");
        }
        #endregion
        #region PRZESTAWIENIA MACIERZOWE
        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string input = textBox9.Text.Replace(" ", "");
                string output = "";
                string[] sKey = textBox8.Text.Split('-');
                int[] iKey = new int[sKey.Length];
                for (int i = 0; i < sKey.Length; i++)
                {
                    iKey[i] = int.Parse(sKey[i]);
                }
                double derivation = Convert.ToDouble(input.Length) / Convert.ToDouble(iKey.Length);
                int itemsLength = Convert.ToInt32(Math.Ceiling(derivation));
                string[] items = new string[itemsLength];

                int position = 0;
                for (int i = 0; i < itemsLength; i++)
                {
                    if (position + iKey.Length <= input.Length - 1)
                    {
                        items[i] = input.Substring(position, iKey.Length);
                    }
                    else
                    {
                        items[i] = input.Substring(position);
                    }
                    position += iKey.Length;
                }

                foreach (var item in items)
                {
                    for (int i = 0; i < iKey.Length; i++)
                    {
                        if (iKey[i] <= item.Length)
                            output += item[iKey[i] - 1];
                    }
                }

                textBox7.Text = output;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }


        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            string input = textBox12.Text.Replace(" ", "");
            string output = "";
            string[] sKey = textBox11.Text.Split('-');
            int[] iKey = new int[sKey.Length];
            for (int i = 0; i < sKey.Length; i++)
            {
                iKey[i] = int.Parse(sKey[i]);
            }

            double derivation = Convert.ToDouble(input.Length) / Convert.ToDouble(iKey.Length);
            int itemsLength = Convert.ToInt32(Math.Ceiling(derivation));
            string[] items = new string[itemsLength];

            int position = 0;
            for (int i = 0; i < itemsLength; i++)
            {
                if (position + iKey.Length <= input.Length - 1)
                {
                    items[i] = input.Substring(position, iKey.Length);
                }
                else
                {
                    items[i] = input.Substring(position);
                }
                position += iKey.Length;
            }

            foreach (var item in items)
            {
                int index = 0;
                char[] chars = new char[item.Length];
                for (int i = 0; i < iKey.Length; i++)
                {
                    if (iKey[i] - 1 >= chars.Length) continue;
                    chars[iKey[i] - 1] = item[index];
                    index++;
                }
                output += new string(chars);
            }

            textBox10.Text = output;
        }
        #endregion
        #region CEZAR
        private void Button6_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int k1, k0;

                int.TryParse(textBox17.Text, out k1);
                int.TryParse(textBox19.Text, out k0);

                var charactersToEncrypt = textBox18.Text.Trim().ToUpper().ToCharArray();
                textBox16.Text = "";

                foreach (var character in charactersToEncrypt)
                {
                    var letterIndex = Alphabet.Select((s, i) => new { i, s })
                        .Where(t => t.s == character)
                        .Select(t => t.i)
                        .FirstOrDefault();

                    var c = (letterIndex * k1 + k0) % Alphabet.Length;
                    textBox16.Text += Alphabet[c];
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił błąd: " + ex.Message);
            }
        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int k1, k0;

                int.TryParse(textBox20.Text, out k1);
                int.TryParse(textBox14.Text, out k0);

                var charactersToDecrypt = textBox15.Text.ToUpper().ToCharArray();
                textBox13.Text = "";

                foreach (var character in charactersToDecrypt)
                {
                    var letterIndex = Alphabet.Select((s, i) => new { i, s })
                        .Where(t => t.s == character)
                        .Select(t => t.i)
                        .FirstOrDefault();

                    BigInteger a = ((letterIndex + (((100 * Alphabet.Length) - k0) % Alphabet.Length)) * Poteguj(k1, (int)EulerTot(Alphabet.Length) - 1));
                    a = BigInteger.Remainder(a, BigInteger.Parse(Alphabet.Length.ToString()));
                    textBox13.Text += Alphabet[(int)a];
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił błąd: " + ex.Message);
            }
        }
        private float EulerTot(int n)
        {
            float result = n;
            for (int i = 2; i <= n / 2; i++)
            {
                if (n % i == 0 && isPrime(i))
                    result *= (1 - (1 / (float)i));
            }
            return result;
        }

        private bool isPrime(int n)
        {
            for (int i = 2; i <= Math.Sqrt(n); i++)
            {
                if (n % i == 0)
                    return false;
            }
            return true;
        }

        private BigInteger Poteguj(int a, int n)
        {
            BigInteger wynik = a;
            for (int i = 1; i < n; i++)
            {
                wynik *= a;
            }

            return wynik;
        }
        #endregion
        #region VIGENERE
        private void Button8_Click(object sender, RoutedEventArgs e)
        {
            char[] alphabet = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

            char[][] table = new char[alphabet.Length][];
            for (int r = 0; r < alphabet.Length; r++)
            {
                table[r] = new char[alphabet.Length];

                for (int c = 0; c < alphabet.Length; c++)
                {
                    table[r][c] = alphabet[(r + c) % alphabet.Length];
                }
            }

            try
            {
                var charactersToEncrypt = textBox26.Text.Trim().ToLower().ToCharArray();
                var keyLetters = textBox25.Text.Trim().ToLower().ToCharArray();
                textBox24.Text = "";

                if (charactersToEncrypt.Length != keyLetters.Length)
                {
                    int temp = 0;
                    while (keyLetters.Length < charactersToEncrypt.Length)
                    {
                        Array.Resize(ref keyLetters, keyLetters.Length + 1);
                        keyLetters[keyLetters.Length - 1] = keyLetters[temp];
                        temp++;
                    }

                }

                for (int i = 0; i < charactersToEncrypt.Length; i++)
                {
                    var letterIndex = alphabet.Select((s, j) => new { j, s })
                        .Where(t => t.s == charactersToEncrypt[i])
                        .Select(t => t.j)
                        .FirstOrDefault();

                    var keyIndex = alphabet.Select((s, j) => new { j, s })
                        .Where(t => t.s == keyLetters[i])
                        .Select(t => t.j)
                        .FirstOrDefault();

                    textBox24.Text += table[letterIndex][keyIndex];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił błąd: " + ex.Message);
            }
        }

        private void Button7_Click(object sender, RoutedEventArgs e)
        {
            char[] alphabet = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

            char[][] table = new char[alphabet.Length][];
            for (int r = 0; r < alphabet.Length; r++)
            {
                table[r] = new char[alphabet.Length];

                for (int c = 0; c < alphabet.Length; c++)
                {
                    table[r][c] = alphabet[(r + c) % alphabet.Length];
                }
            }

            try
            {
                var charactersToDecrypt = textBox23.Text.Trim().ToLower().ToCharArray();
                var keyLetters = textBox22.Text.Trim().ToLower().ToCharArray();
                textBox21.Text = "";

                if (charactersToDecrypt.Length != keyLetters.Length)
                {
                    int temp = 0;
                    while (keyLetters.Length < charactersToDecrypt.Length)
                    {
                        Array.Resize(ref keyLetters, keyLetters.Length + 1);
                        keyLetters[keyLetters.Length - 1] = keyLetters[temp];
                        temp++;
                    }
                }

                for (int i = 0; i < charactersToDecrypt.Length; i++)
                {
                    var keyIndex = alphabet.Select((s, j) => new { j, s })
                        .Where(t => t.s == keyLetters[i])
                        .Select(t => t.j)
                        .FirstOrDefault();

                    for (int x = 0; x < alphabet.Length; x++)
                    {
                        if (table[x][keyIndex] == charactersToDecrypt[i])
                        {
                            textBox21.Text += alphabet[x];
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił błąd: " + ex.Message);
            }
        }

        #endregion
        #region LFSR
        private void Button9_Click(object sender, RoutedEventArgs e)
        {
            lfsr = new LFSR(ziarno.Text, wielomian.Text);

            for (int i = 0; i < 10; i++)
            {
                lfsr.iteracja();
                listBox1.Items.Add(lfsr.wypisz());
            }
        }
        #endregion
        #region DES
        //zaszyfrowanie
        private void Button11_Click(object sender, EventArgs e)
        {
            StreamReader s = new StreamReader(pliksciezkades);
            String tekstDes = s.ReadToEnd();
            StreamReader k = new StreamReader(pliksciezkadesklucz);
            String tekstDesKlucz = k.ReadToEnd();
            desik = new DES();
            using (StreamWriter writer = new StreamWriter("zaszyfrowane.txt"))
            {
                writer.Write(desik.Encrypt(tekstDes, tekstDesKlucz));
            }
            MessageBox.Show("Plik został zaszyfrowany w zaszyfrowane.txt");
        }

        // plik z tekstem jawnym 
        private void Button12_Click(object sender, EventArgs e)
        {
            OpenFileDialog okienkoDes = new OpenFileDialog();
            okienkoDes.ShowDialog();
            if (okienkoDes.FileName != null)
            {
                MessageBox.Show("Wybrano plik: " + okienkoDes.FileName);
                pliksciezkades = okienkoDes.FileName;
            }

        }
        // odszyfrowanie 
        private void Button10_Click(object sender, EventArgs e)
        {
            StreamReader s = new StreamReader(pliksciezkades);
            // plikdes.Text = s.ReadToEnd();
            String tekstDes = s.ReadToEnd();
            StreamReader k = new StreamReader(pliksciezkadesklucz);
            odszyfrowanie = new DES();
            String tekstDesKlucz = k.ReadToEnd();

            using (StreamWriter writer = new StreamWriter("odszyfrowane.txt"))
            {
                writer.Write(odszyfrowanie.Decrypt(tekstDes, tekstDesKlucz));
            }
            MessageBox.Show("Plik został odszyfrowany w odszyfrowane.txt");
        }
        // plik z kluczem
        private void Button13_Click(object sender, EventArgs e)
        {
            OpenFileDialog okienkoDesKlucz = new OpenFileDialog();
            okienkoDesKlucz.ShowDialog();
            if (okienkoDesKlucz.FileName != null)
            {
                MessageBox.Show("Wybrano plik: " + okienkoDesKlucz.FileName);
                pliksciezkadesklucz = okienkoDesKlucz.FileName;
            }
        }




        #endregion
        #region strumieniowy
        Encryptor encryptor = null;
        private void Button_LoadFile_Click(object sender, RoutedEventArgs e)
        {
            string plainText = encryptor.ReadTextFromFile();
            /*
            if (encryptor.checkIfStringContainsOnlyAsciiChars(plainText) == false)
            {
                encryptor.ShowErrorPopUp("Wczytany plik zawiera niedozwolony znak spoza znaków ASCII.");
                return;
            }
            */
           TextBox_PlainText.Text = plainText;
           string binaryText = encryptor.ConvertStringToBinaryString(plainText);
           TextBox_BinaryText.Text = binaryText;
       }

       private void Button_LoadBinaryText_Click(object sender, RoutedEventArgs e)
       {
           string binaryText = encryptor.ReadTextFromFile();
           if (encryptor.checkIfStringIsBinary(binaryText) == false)
           {
               encryptor.ShowErrorPopUp("Wczytany plik zawiera niedozwolony znak (inny niż 0 lub 1).");
               return;
           }
           else
           {
               TextBox_BinaryText.Text = binaryText;
               string plainText = encryptor.convertBitStringToString(binaryText);
               TextBox_PlainText.Text = plainText;
           }
       }

       private void Button_Info_Click(object sender, RoutedEventArgs e)
       {
           encryptor.ShowInfoPopUp();
       }

       private void Button_LoadKey_Click(object sender, RoutedEventArgs e)
       {
           string keyText = encryptor.ReadTextFromFile();
           if (encryptor.checkIfStringIsBinary(keyText) == false)
           {
               encryptor.ShowErrorPopUp("Wczytany plik zawiera niedozwolony znak (inny niż 0 lub 1).");
               return;
           }
           TextBox_CipherKey.Text = keyText;
       }

       private void Button_CipherOrDecipher1_Click(object sender, RoutedEventArgs e)
       {
           if (TextBox_BinaryText.Text.Length == 0)
               encryptor.ShowErrorPopUp("Pole \"Tekst binarnie\" jest puste.");
           else if (encryptor.checkIfStringIsBinary(TextBox_BinaryText.Text) == false)
               encryptor.ShowErrorPopUp("Pole \"Tekst binarnie\" zawiera niedozwolony znak (inny niż 0 lub 1).");
           else if (TextBox_CipherKey.Text.Length == 0)
               encryptor.ShowErrorPopUp("Pole \"Klucz szyfrowania\" jest puste.");
           else if (encryptor.checkIfStringIsBinary(TextBox_CipherKey.Text) == false)
               encryptor.ShowErrorPopUp("Pole \"Klucz szyfrowania\" zawiera niedozwolony znak (inny niż 0 lub 1).");
           else
           {
               string binaryText = TextBox_BinaryText.Text;
               string binaryKey = TextBox_CipherKey.Text;
               string cipheredText = "";

               if (binaryText.Length > binaryKey.Length)
               {
                   encryptor.ShowPopUp("Klucz jest zbyt krótki, dlatego zostanie powielony do odpowiedniej długości.", "Za krótki klucz!");
                   encryptor.RepeatKeyToMatchLength(binaryText, ref binaryKey);
                   TextBox_CipherKey.Text = binaryKey;
               }

               cipheredText = encryptor.CipherBinaryStringUsingBinaryString(binaryText, binaryKey);
               TextBox_CipheredText.Text = cipheredText;
               TextBox_CipheredTextAsChars.Text = encryptor.convertBitStringToString(cipheredText);
           }
       }

       private void TextBox_PlainText_TextChanged(object sender, TextChangedEventArgs e)
       {
           Label_PlainTextLength.Content = "Długość tekstu do zaszyfrowania: " + TextBox_PlainText.Text.Length;
           TextBox_BinaryText.Text = encryptor.ConvertStringToBinaryString(TextBox_PlainText.Text);
       }

       private void TextBox_BinaryText_TextChanged(object sender, TextChangedEventArgs e)
       {
           Label_BinaryTextLength.Content = "Liczba bitów do zaszyfrowania: " + TextBox_BinaryText.Text.Length;
       }

       private void TextBox_CipherKey_TextChanged(object sender, TextChangedEventArgs e)
       {
           Label_CipherKeyLength.Content = "Liczba bitów klucza szyfrowania: " + TextBox_CipherKey.Text.Length;
       }

       private void TextBox_CipheredText_TextChanged(object sender, TextChangedEventArgs e)
       {
           Label_CipheredTextLength.Content = "Liczba bitów szyfrogramu: " + TextBox_CipheredText.Text.Length;
       }

       private void TextBox_CipheredTextAsChars_TextChanged(object sender, TextChangedEventArgs e)
       {
           Label_CipheredTextAsCharsLength.Content = "Długość szyfrogramu: " + TextBox_CipheredTextAsChars.Text.Length;
       }

       private void Button_ClearAllFields_Click(object sender, RoutedEventArgs e)
       {
           TextBox_PlainText.Text = "";
           TextBox_BinaryText.Text = "";
           TextBox_CipherKey.Text = "";
           TextBox_CipheredText.Text = "";
           TextBox_CipheredTextAsChars.Text = "";
       }

       private void Button_SaveCipherAsChars_Click(object sender, RoutedEventArgs e)
       {
           encryptor.SaveToFileAsTextString(TextBox_CipheredTextAsChars.Text);
       }

       private void Button_SaveCipherBinary_Click(object sender, RoutedEventArgs e)
       {
           encryptor.SaveToFileAsTextString(TextBox_CipheredText.Text);
       }
        #endregion
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
