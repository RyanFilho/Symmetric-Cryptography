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

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (ShiftTextBox.Text == string.Empty)
            {
                MessageBox.Show("Shift value invalid!\nMust be a number.", "Attention, please.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                var inputValue = InputTextBox.Text;
                var outputValue = string.Empty;
                foreach (var item in inputValue)
                {
                    outputValue = outputValue += ReplacementCipher(item, int.Parse(ShiftTextBox.Text));
                }
                OutputTextBox.Text = outputValue;
            }
        }

        public static char ReplacementCipher(char ch, int key)
        {
            char initialIndex = char.IsUpper(ch) ? 'A' : 'a';
            return (char)((((ch + key) - initialIndex) % 26) + initialIndex);
        }
    }
}
