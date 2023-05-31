using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using FeatureHubPurple.Models;
using Newtonsoft.Json;

namespace FeatureHubPurple.UserControls
{
    public partial class Feature3Control : UserControl
    {
        // Timer items collection
        private ObservableCollection<TimerItem> _timerItems;
        // Timer for updating the end time and duration of active timers
        private readonly DispatcherTimer _timer;
        // Index for the timer items
        private int _timerIndex;

        public Feature3Control()
        {
            InitializeComponent();
            this.DataContext = this;
            this.VerticalAlignment = VerticalAlignment.Stretch;
            // Initialize the timer items collection
            LoadTimeEntries();
            // Initialize the timer
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(60)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        // Event handler for the timer tick event
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update the end time and duration of each active timer
            foreach (var timerItem in _timerItems.Where(ti => ti.IsActive && !ti.IsPaused))
            {
                UpdateEndTimeAndDuration(timerItem);
            }
            TimersListView.Items.Refresh();
        }



        // Event handler for the start time text box lost focus event
        private void StartTimeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((FrameworkElement)sender).DataContext is TimerItem timerItem)
            {
                timerItem.StartTime = ((TextBox)sender).Text;
                UpdateEndTimeAndDuration(timerItem);
            }
        }
        
        // Event handler for the end time text box lost focus event
        private void EndTimeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((FrameworkElement)sender).DataContext is TimerItem timerItem && !timerItem.IsActive)
            {
                timerItem.EndTime = ((TextBox)sender).Text;
                UpdateEndTimeAndDuration(timerItem);
            }
        }


        // Event handler for the description text box text changed event
        private void DescriptionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            StartButton.IsEnabled = !string.IsNullOrEmpty(DescriptionTextBox.Text);
        }

        // Helper method to round up a time span to the nearest interval
        private static TimeSpan RoundUpToNearest(TimeSpan value, TimeSpan roundingInterval)
        {
            var ticks = (value.Ticks + roundingInterval.Ticks - 1) / roundingInterval.Ticks;
            return new TimeSpan(ticks * roundingInterval.Ticks);
        }

        // Event handler for the timers list view size changed event
        private void TimersListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var listViewWidth = TimersListView.ActualWidth;
            var gridView = (GridView)TimersListView.View;
            // Set the width of each column
            gridView.Columns[0].Width = listViewWidth * 0.05; // Index
            gridView.Columns[1].Width = listViewWidth * 0.30; // Description
            gridView.Columns[2].Width = listViewWidth * 0.10; // Start time
            gridView.Columns[3].Width = listViewWidth * 0.10; // End time
            gridView.Columns[4].Width = listViewWidth * 0.10; // Duration
            gridView.Columns[5].Width = listViewWidth * 0.10; // Total minutes
            gridView.Columns[6].Width = listViewWidth * 0.08; // Pause button
            gridView.Columns[7].Width = listViewWidth * 0.08; // Stop button
            gridView.Columns[8].Width = listViewWidth * 0.08; // Delete button
        }

        // Method to update the end time and duration of a timer item
        private void UpdateEndTimeAndDuration(TimerItem item)
        {
            if (item.IsActive)
            {
                item.EndTime = DateTime.Now.ToString("HH:mm");
                DateTime startTime = DateTime.ParseExact(item.StartTime, "HH:mm", CultureInfo.InvariantCulture);
                DateTime endTime = DateTime.ParseExact(item.EndTime, "HH:mm", CultureInfo.InvariantCulture);
                TimeSpan duration = endTime - startTime;
                TimeSpan roundedDuration = RoundUpToNearest(duration, TimeSpan.FromMinutes(15));
                item.Duration = $"{roundedDuration:hh\\:mm}";
                item.TotalMinutes = (int)roundedDuration.TotalMinutes;
            }
        }

        // Event handler for the description text box key down event
        private void DescriptionTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                StartButton_Click(sender, e);
            }
        }

        // Event handler for the save timers button click event
        private void SaveTimersButton_Click(object sender, RoutedEventArgs e)
        {
            var json = JsonConvert.SerializeObject(_timerItems);
            File.WriteAllText("timers.json", json);

            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.SetIsSaved(true);
        }

        // Method to load the timer entries from the JSON file
        private void LoadTimeEntries()
        {
            if (File.Exists("timers.json"))
            {
                var json = File.ReadAllText("timers.json");
                _timerItems = JsonConvert.DeserializeObject<ObservableCollection<TimerItem>>(json);
            }
            else
            {
                _timerItems = new ObservableCollection<TimerItem>();
            }
            TimersListView.ItemsSource = _timerItems;
        }

        // Event handler for the start button click event
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _timerIndex++;
            TimerItem newItem = new TimerItem
            {
                Index = _timerIndex,
                Description = DescriptionTextBox.Text,
                StartTime = DateTime.Now.ToString("HH:mm"),
                EndTime = "00:00",
                Duration = "00:15",
                IsActive = true // Add this line to set IsActive to true when a new timer is started
            };
            _timerItems.Add(newItem);
            DescriptionTextBox.Clear();
            DescriptionTextBox.Focus(); // Set focus to the TextBox after the Start button is clicked
            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.SetIsSaved(false);
        }

        // Event handler for the pause button click event
        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Tag is TimerItem timerItem && timerItem.IsActive)
            {
                timerItem.IsPaused = !timerItem.IsPaused;
            }
        }
        // Event handler for the stop button click event
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

        // Event handler for the clear timers button click event
        private void ClearTimersButton_Click(object sender, RoutedEventArgs e)
        {
            // Rename the existing file
            if (File.Exists("timers.json"))
            {
                string dateTime = DateTime.Now.ToString("ddMMyyyy_HHmm");
                File.Move("timers.json", $"timers_Old_{dateTime}.json");
            }
            // Clear the timer items
            _timerItems.Clear();
        }
        // Event handler for the delete timer button click event
        private void DeleteTimerButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the TimerItem object associated with the clicked button
            var timerItem = (TimerItem)((Button)sender).Tag;
            // Remove the TimerItem from the collection
            _timerItems.Remove(timerItem);
            // Save the updated timer items collection to the JSON file
            SaveTimersButton_Click(sender, e);
        }
    }
}
