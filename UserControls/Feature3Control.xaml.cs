using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using FeatureHubPurple.Models;

namespace FeatureHubPurple.UserControls
{
    public partial class Feature3Control : UserControl
    {
        private readonly ObservableCollection<TimerItem> _timerItems;
        private readonly DispatcherTimer _timer;

        public Feature3Control()
        {
            InitializeComponent();
            this.DataContext = this;
            this.VerticalAlignment = VerticalAlignment.Stretch;
            _timerItems = new ObservableCollection<TimerItem>();
            TimersListView.ItemsSource = _timerItems;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(60);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            foreach (var timerItem in _timerItems)
            {
                if (!timerItem.IsPaused)
                {
                    UpdateEndTimeAndDuration(timerItem);
                }
            }
            TimersListView.Items.Refresh();
        }



        private void StartTimeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (((FrameworkElement)sender).DataContext is TimerItem timerItem)
            {
                timerItem.StartTime = textBox.Text;
                UpdateEndTimeAndDuration(timerItem);
            }
        }

        private void EndTimeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (((FrameworkElement)sender).DataContext is TimerItem timerItem && !timerItem.IsActive)
            {
                timerItem.EndTime = textBox.Text;
                UpdateEndTimeAndDuration(timerItem);
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            TimerItem newItem = new TimerItem
            {
                Description = DescriptionTextBox.Text,
                StartTime = DateTime.Now.ToString("HH:mm"),
                EndTime = "00:00",
                Duration = "00:15",
                IsActive = true // Add this line to set IsActive to true when a new timer is started
            };
            _timerItems.Add(newItem);
            DescriptionTextBox.Clear();
            DescriptionTextBox.Focus(); // Set focus to the TextBox after the Start button is clicked
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Tag is TimerItem timerItem && timerItem.IsActive)
            {
                timerItem.IsPaused = !timerItem.IsPaused;
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (((FrameworkElement)sender).Tag is TimerItem timerItem && timerItem.IsActive)
            {
                timerItem.IsActive = false;
                UpdateEndTimeAndDuration(timerItem);

                // Calculate total duration of all stopped timers
                var totalDuration = _timerItems.Where(ti => !ti.IsActive).Sum(ti => ti.TotalMinutes);
                TotalTimeTextBlock.Text = $"Total: {TimeSpan.FromMinutes(totalDuration):hh\\:mm}";
            }
        }

        private void DescriptionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            StartButton.IsEnabled = !string.IsNullOrEmpty(DescriptionTextBox.Text);
        }

        private TimeSpan RoundUpToNearest(TimeSpan value, TimeSpan roundingInterval)
        {
            var ticks = (value.Ticks + roundingInterval.Ticks - 1) / roundingInterval.Ticks;
            return new TimeSpan(ticks * roundingInterval.Ticks);
        }

        private void TimersListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var listViewWidth = TimersListView.ActualWidth;
            var gridView = ((GridView)TimersListView.View);

            gridView.Columns[0].Width = listViewWidth * 0.35; // 25% of the ListView width
            gridView.Columns[1].Width = listViewWidth * 0.10; // 15% of the ListView width
            gridView.Columns[2].Width = listViewWidth * 0.10; // 15% of the ListView width
            gridView.Columns[3].Width = listViewWidth * 0.10; // 15% of the ListView width
            gridView.Columns[4].Width = listViewWidth * 0.10; // 15% of the ListView width
            gridView.Columns[5].Width = listViewWidth * 0.1;  // 10% of the ListView width
            gridView.Columns[6].Width = listViewWidth * 0.1;  // 10% of the ListView width
        }


        private void UpdateEndTimeAndDuration(TimerItem item)
        {
            if (item.IsActive)
            {
                item.EndTime = DateTime.Now.ToString("HH:mm");
                DateTime startTime = DateTime.ParseExact(item.StartTime, "HH:mm", CultureInfo.InvariantCulture);
                DateTime endTime = DateTime.ParseExact(item.EndTime, "HH:mm", CultureInfo.InvariantCulture);
                TimeSpan duration = endTime - startTime;
                TimeSpan roundedDuration = RoundUpToNearest(duration, TimeSpan.FromMinutes(15));
                item.Duration = $"{roundedDuration:h\\:mm}";
                item.TotalMinutes = (int)roundedDuration.TotalMinutes;
            }
        }


    }
}
