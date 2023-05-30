using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using FeatureHubPurple.Services;
using FeatureHubPurple.Models;

namespace FeatureHubPurple.UserControls
{
    /// <summary>
    /// Interaction logic for Feature2Control.xaml
    /// </summary>
    public partial class Feature2Control : UserControl
    {
        private readonly DispatcherTimer _updateTimer;
        private TimeZoneInfo _currentSelectedTimeZone;
        private readonly TimeZoneService _timeZoneService;

        public Feature2Control()
        {
            InitializeComponent();

            // Initialize the timer for updating the time display
            _updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _updateTimer.Tick += UpdateTimeDisplay;
            _updateTimer.Start();

            // Initialize the time zone service
            _timeZoneService = new TimeZoneService();

            // Populate the time zone list
            var timeZoneModels = _timeZoneService.GetTimeZonesWithCurrentTimes();
            foreach (var timeZoneModel in timeZoneModels)
            {
                TimeZoneList.Items.Add(timeZoneModel);
            }

            // Set the default time zone to Brussels
            _currentSelectedTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            TimeZoneComboBox.SelectedIndex = 0;

            // Display the current time
            UpdateTimeDisplay(null, null);
        }

        private void UpdateTimeDisplay(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.UtcNow;
            if (_currentSelectedTimeZone != null)
            {
                currentTime = TimeZoneInfo.ConvertTimeFromUtc(currentTime, _currentSelectedTimeZone);
            }
            TimeDisplay.Text = currentTime.ToString("HH:mm:ss");
        }

        private void TimeZoneComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (TimeZoneComboBox.SelectedIndex)
            {
                case 0: // Brussels
                    _currentSelectedTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                    break;
                case 1: // UTC
                    _currentSelectedTimeZone = TimeZoneInfo.FindSystemTimeZoneById("UTC");
                    break;
                case 2: // CEST
                    _currentSelectedTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                    break;
            }
        }
    }
}
