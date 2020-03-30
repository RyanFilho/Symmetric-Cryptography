using System.Windows;
using System.Windows.Input;

namespace SymmetricEncryptionAlgorithm
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

        public static char ReplacementCipherEncryption(char input, int shift)
        {

            return (char) (input + shift);
        }

        public static char ReplacementCipherDecryption(char input, int shift)
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
                    output = output += ReplacementCipherEncryption(item, shift);
                }

                OutputTextBox.Text = output;
            }
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            var input = InputTextBox.Text;
            var output = string.Empty;
            var shift = int.Parse(ShiftTextBox.Text);

            foreach (var item in input)
            {
                output = output += ReplacementCipherDecryption(item, shift);
            }

            OutputTextBox.Text = output;
        }
    }
}
