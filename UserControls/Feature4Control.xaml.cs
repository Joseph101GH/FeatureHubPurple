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
using System.Collections.ObjectModel;
using FeatureHubPurple.Models;
using System.ComponentModel;

namespace FeatureHubPurple.UserControls
{
    /// <summary>
    /// Interaction logic for Feature4Control.xaml
    /// </summary>
    public partial class Feature4Control : UserControl
    {
        private ICollectionView _collectionView;

        public Feature4Control()
        {
            InitializeComponent();
            GenerateTimeEntries();
        }

        public void GenerateTimeEntries()
        {
            // Generate time entries
            var timeEntries = new ObservableCollection<TimeEntry>();
            for (int i = 15; i <= 480; i += 15)
            {
                var hours = i / 60;
                var minutes = i % 60;
                var time = $"{hours.ToString("D2")}:{minutes.ToString("D2")}";
                timeEntries.Add(new TimeEntry { Time = time, TotalMinutes = i, IsActive = true });
            }

            // Set the ItemsSource of the ListView to the time entries
            TimersListView.ItemsSource = timeEntries;

            // Create a CollectionView for the time entries
            _collectionView = CollectionViewSource.GetDefaultView(timeEntries);

            // Set the Filter property to a method that filters the items based on the search input
            _collectionView.Filter = item => FilterTimeEntry((TimeEntry)item);
        }

        private bool FilterTimeEntry(TimeEntry timeEntry)
        {
            // If the search box is empty, include all items
            if (string.IsNullOrEmpty(textBoxSearch.Text))
                return true;

            // Include the item if its Time property contains the search text
            return timeEntry.Time.Contains(textBoxSearch.Text);
        }

        private void TimersListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var listViewWidth = TimersListView.ActualWidth;
            var gridView = ((GridView)TimersListView.View);

            gridView.Columns[0].Width = listViewWidth * 0.30; // 45% of the ListView width
            gridView.Columns[1].Width = listViewWidth * 0.30; // 45% of the ListView width
            gridView.Columns[2].Width = listViewWidth * 0.2;  // 10% of the ListView width
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the TimeEntry object associated with the clicked button
            var timeEntry = (TimeEntry)((Button)sender).Tag;

            // Copy the TotalMinutes property to the clipboard
            Clipboard.SetText(timeEntry.TotalMinutes.ToString());
        }

        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Refresh the CollectionView to apply the filter
            _collectionView.Refresh();
        }
    }
}
