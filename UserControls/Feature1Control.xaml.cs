using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Converters;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FeatureHubPurple.UserControls
{
    /// <summary>
    /// Interaction logic for Feature1Control.xaml
    /// </summary>
    public partial class Feature1Control : UserControl
    {
        public Feature1Control()
        {
            InitializeComponent();
        }

        private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsGuid(InputTextBox.Text))
            {
                //CORRECT
              MainWindow.Instance.mainContentBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4caf50"));
            }
            else
            {
                //WRONG
                MainWindow.Instance.mainContentBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f44336"));
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            InputTextBox.Clear();
            MainWindow.Instance.mainContentBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#243771"));
        }

        private void CopyClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                InputTextBox.Text = Clipboard.GetText();
            }
        }

        private void guidExample_Click(object sender, RoutedEventArgs e)
        {
            InputTextBox.Text = "6B29FC40-CA47-1067-B31D-00DD010662DA";

        }

        private bool IsGuid(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            Regex guidPattern = new Regex(@"^(\{0[xX][0-9a-fA-F]{8}\,(\s)?[xX][0-9a-fA-F]{4}\,(\s)?[xX][0-9a-fA-F]{4}\,(\s)?\{[0-9a-fA-F]{2}(\s)?,[0-9a-fA-F]{2}(\s)?,[0-9a-fA-F]{2}(\s)?,[0-9a-fA-F]{2}(\s)?,[0-9a-fA-F]{2}(\s)?,[0-9a-fA-F]{2}(\s)?,[0-9a-fA-F]{2}(\s)?,[0-9a-fA-F]{2}(\s)?\}\})$|^(?i)[0-9a-fA-F]{8}[-][0-9a-fA-F]{4}[-][0-9a-fA-F]{4}[-][0-9a-fA-F]{4}[-][0-9a-fA-F]{12}$");
            return guidPattern.IsMatch(input);
        }


    }
}
