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
using System.Windows.Threading;

namespace FeatureHubPurple.UserControls
{
    /// <summary>
    /// Interaction logic for Feature2Control.xaml
    /// </summary>
    public partial class Feature2Control : UserControl
    {
        private readonly DispatcherTimer _timer;
        private TimeZoneInfo _selectedTimeZone;

        public Feature2Control()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();

            _selectedTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"); // Default timezone to Brussels
            TimeZoneComboBox.SelectedIndex = 0; // No timezone selected by default
            DisplayTime();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DisplayTime();
        }

        private void DisplayTime()
        {
            DateTime currentTime = DateTime.UtcNow;
            if (_selectedTimeZone != null)
            {
                currentTime = TimeZoneInfo.ConvertTimeFromUtc(currentTime, _selectedTimeZone);
            }
            TimeDisplay.Text = currentTime.ToString("HH:mm:ss");
        }

        private void TimeZoneComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TimeZoneComboBox.SelectedIndex == 0) // Brussels
            {
                _selectedTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"); // Default timezone to Brussels
            }
            else if (TimeZoneComboBox.SelectedIndex == 1) // UTC
            {
                _selectedTimeZone = TimeZoneInfo.FindSystemTimeZoneById("UTC");
            }
            else if (TimeZoneComboBox.SelectedIndex == 2) // CEST
            {
                _selectedTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            }
        }
    }
}
