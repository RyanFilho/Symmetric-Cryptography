using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SymmetricCryptographyAlgorithm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            try
            { e.Handled = !(uint.Parse(e.Text) <= 25); }
            catch
            { e.Handled = true; }
        }

        public static char ReplacementCipherEncrypt(char input, int shift)
        {

            return (char)(input + shift);
        }

        public static char ReplacementCipherDecrypt(char input, int shift)
        {

            return (char)(input - shift);
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (ShiftTextBox.Text == string.Empty)
            {
                MessageBox.Show("Shift value invalid!\nMust be a number.", "Attention, please.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                var input = InputTextBox.Text;
                var output = string.Empty;
                var shift = int.Parse(ShiftTextBox.Text);

                foreach (var item in input)
                {
                    output = output += ReplacementCipherEncrypt(item, shift);
                }

                OutputTextBox.Text = output;
            }
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (ShiftTextBox.Text == string.Empty)
            {
                MessageBox.Show("Shift value invalid!\nMust be a number.", "Attention, please.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                var input = InputTextBox.Text;
                var output = string.Empty;
                var shift = int.Parse(ShiftTextBox.Text);

                foreach (var item in input)
                {
                    output = output += ReplacementCipherDecrypt(item, shift);
                }

                OutputTextBox.Text = output;
            }
        }

        private void TranspositionEncryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (TranspositionKeyTextBox.Text == string.Empty)
            {
                MessageBox.Show("Key value invalid!\nCan't be empty.", "Attention, please.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                var input = TranspositionInputTextBox.Text;
                var key = TranspositionKeyTextBox.Text;
                TranspositionOutputTextBox.Text = TranspositionCipherEncrypt(input, key);
            }
        }

        private void TranspositionDecryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (TranspositionKeyTextBox.Text == string.Empty)
            {
                MessageBox.Show("Key value invalid!\nCan't be empty.", "Attention, please.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                var input = TranspositionInputTextBox.Text;
                var key = TranspositionKeyTextBox.Text;
                TranspositionOutputTextBox.Text = TranspositionCipherDecrypt(input, key);
            }
        }

        private static int[] GetShiftIndexes(string key)
        {
            List<KeyValuePair<int, char>> sortedKey = new List<KeyValuePair<int, char>>();

            int keyLength = key.Length;
            int[] indexes = new int[keyLength];


            for (int i = 0; i < keyLength; ++i)
                sortedKey.Add(new KeyValuePair<int, char>(i, key[i]));

            sortedKey.Sort(
                delegate (KeyValuePair<int, char> foo, KeyValuePair<int, char> bar)
                {
                    return foo.Value.CompareTo(bar.Value);
                }
            );

            for (int i = 0; i < keyLength; ++i)
                indexes[sortedKey[i].Key] = i;

            return indexes;
        }

        public static string TranspositionCipherEncrypt(string input, string key)
        {
            char padChar = '|';
            input = (input.Length % key.Length == 0) ? input : input.PadRight(input.Length - (input.Length % key.Length) + key.Length, padChar);
            StringBuilder output = new StringBuilder();
            int totalChars = input.Length;
            int totalColumns = key.Length;
            int totalRows = (int)Math.Ceiling((double)totalChars / totalColumns);
            char[,] rowChars = new char[totalRows, totalColumns];
            char[,] colChars = new char[totalColumns, totalRows];
            char[,] sortedColChars = new char[totalColumns, totalRows];
            int currentRow, currentColumn, i, j;
            int[] shiftIndexes = GetShiftIndexes(key);

            for (i = 0; i < totalChars; ++i)
            {
                currentRow = i / totalColumns;
                currentColumn = i % totalColumns;
                rowChars[currentRow, currentColumn] = input[i];
            }

            for (i = 0; i < totalRows; ++i)
                for (j = 0; j < totalColumns; ++j)
                    colChars[j, i] = rowChars[i, j];

            for (i = 0; i < totalColumns; ++i)
                for (j = 0; j < totalRows; ++j)
                    sortedColChars[shiftIndexes[i], j] = colChars[i, j];

            for (i = 0; i < totalChars; ++i)
            {
                currentRow = i / totalRows;
                currentColumn = i % totalRows;
                output.Append(sortedColChars[currentRow, currentColumn]);
            }

            return output.ToString();
        }

        public static string TranspositionCipherDecrypt(string input, string key)
        {
            StringBuilder output = new StringBuilder();
            int totalChars = input.Length;
            int totalColumns = (int) Math.Ceiling((double)totalChars / key.Length);
            int totalRows = key.Length;
            char[,] rowChars = new char[totalRows, totalColumns];
            char[,] colChars = new char[totalColumns, totalRows];
            char[,] unsortedColChars = new char[totalColumns, totalRows];
            int currentRow, currentColumn, i, j;
            int[] shiftIndexes = GetShiftIndexes(key);

            for (i = 0; i < totalChars; ++i)
            {
                currentRow = i / totalColumns;
                currentColumn = i % totalColumns;
                rowChars[currentRow, currentColumn] = input[i];
            }

            for (i = 0; i < totalRows; ++i)
                for (j = 0; j < totalColumns; ++j)
                    colChars[j, i] = rowChars[i, j];

            for (i = 0; i < totalColumns; ++i)
                for (j = 0; j < totalRows; ++j)
                    unsortedColChars[i, j] = colChars[i, shiftIndexes[j]];

            for (i = 0; i < totalChars; ++i)
            {
                currentRow = i / totalRows;
                currentColumn = i % totalRows;
                output.Append(unsortedColChars[currentRow, currentColumn]);
            }

            return output.ToString();
        }

        private void OneTimePadEncryptButton_Click(object sender, RoutedEventArgs e)
        {
            var inputBytes = Encoding.Unicode.GetBytes(OneTimePadInputTextBox.Text);
            byte[] pad = OneTimePadCipherGeneratePad(size: inputBytes.Length, seed: 1);
            OneTimePadKeyTextBox.Text = Convert.ToBase64String(inArray: pad);
            byte[] encrypted = OneTimePadCipherEncrypt(inputBytes, pad);
            OneTimePadOutputTextBox.Text = Convert.ToBase64String(inArray: encrypted);
        }

        private void OneTimePadDecryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (OneTimePadKeyTextBox.Text == string.Empty)
            {
                MessageBox.Show("Key value invalid!\nCan't be empty.", "Attention, please.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                try
                {
                    var inputBytes = Convert.FromBase64String(OneTimePadInputTextBox.Text);
                    var key = Convert.FromBase64String(OneTimePadKeyTextBox.Text);
                    byte[] output = OneTimePadCipherDecrypt(inputBytes, key);
                    OneTimePadOutputTextBox.Text = Encoding.Unicode.GetString(output);
                }
                catch(Exception exception)
                {
                    MessageBox.Show("Decrypt Error:\n" + exception.Message, "Attention, please.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }
        public static byte[] OneTimePadCipherEncrypt(byte[] data, byte[] pad)
        {
            var result = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                var sum = (int)data[i] + (int)pad[i];
                if (sum > 255)
                    sum -= 255;
                result[i] = (byte)sum;
            }
            return result;
        }

        public static byte[] OneTimePadCipherDecrypt(byte[] encrypted, byte[] pad)
        {
            var result = new byte[encrypted.Length];
            for (int i = 0; i < encrypted.Length; i++)
            {
                var dif = (int)encrypted[i] - (int)pad[i];
                if (dif < 0)
                    dif += 255;
                result[i] = (byte)dif;
            }
            return result;
        }
        public static byte[] OneTimePadCipherGeneratePad(int size, int seed)
        {
            var random = new Random(Seed: seed);
            var bytesBuffel = new byte[size];

            random.NextBytes(bytesBuffel);

            return bytesBuffel;
        }
    }
}
