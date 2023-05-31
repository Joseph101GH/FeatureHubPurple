using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FeatureHubPurple.Services;
using FeatureHubPurple.UserControls;

namespace FeatureHubPurple
{
    public partial class MainWindow : Window
    {
        // Singleton instance of MainWindow
        public static MainWindow Instance
        {
            get; private set;
        }

        // Original content of the MainContent control
        private UIElement _originalMainContent;
        private CreatioService _creatioService;
        // Instance of Feature3Control
        private Feature3Control _feature3Control;
        public bool IsSaved = true;
        public void SetIsSaved(bool value)
        {
            IsSaved = value;
        }

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;

            _creatioService = new CreatioService();

            // Save the original content of MainContent
            _originalMainContent = MainContent.Children.Count > 0 ? MainContent.Children[0] : null;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!IsSaved) // You need to implement this property to track whether the data is saved
            {
                MessageBoxResult result = MessageBox.Show("Do you want to discard your changes?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        // Handle mouse events for moving the window
        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        // Button click event handlers
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Feature2Button_Click(object sender, RoutedEventArgs e)
        {
            LoadFeatureControl(new Feature2Control());
        }

        private void Feature3Button_Click(object sender, RoutedEventArgs e)
        {
            if (_feature3Control != null)
                LoadFeatureControl(_feature3Control);
            LoadFeatureControl(new Feature3Control());
        }

        // Method to load a UserControl into the MainContent control
        private void LoadFeatureControl(UserControl featureControl)
        {
            MainContent.Children.Clear();
            MainContent.Children.Add(featureControl);

        }

        // Method to set the active button style
        private void SetActiveButton(Button activeButton)
        {
            // Reset all button styles to "menuButton"
            foreach (var child in SidebarButtonsPanel.Children)
            {
                if (child is Button button)
                {
                    button.Style = (Style)FindResource("menuButton");
                }
            }

            // Set the clicked button's style to "menuButtonActive"
            activeButton.Style = (Style)FindResource("menuButtonActive");
        }


        // Method to set the title content
        private void SetTitleContent(string title)
        {
            pageTitle.Text = title;
        }
        #region Sidebar buttons
        // Button click handler for returning to the Dashboard
        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            RestoreMainContent();
            SetActiveButton(DashboardButton);
        }

        // Button click handlers for loading different features
        private void GuidCheckerButton_Click(object sender, RoutedEventArgs e)
        {
            SetActiveButton(GuidCheckerButton);
            LoadFeatureControl(new Feature1Control());
            SetTitleContent("GUID-Checker");
        }

        private void TimeZonesButton_Click(object sender, RoutedEventArgs e)
        {
            SetActiveButton(TimeZonesButton);
            LoadFeatureControl(new Feature2Control());
            SetTitleContent("Timezones");

        }

        private void WorkTimersButton_Click(object sender, RoutedEventArgs e)
        {
            SetActiveButton(WorkTimersButton);
            SetTitleContent("WorkTimers");

            if (_feature3Control != null)
            {
                LoadFeatureControl(_feature3Control);
            }
            else
            {
                // Initialize Feature3Control
                _feature3Control = new Feature3Control();

                LoadFeatureControl(_feature3Control);
            }

        }

        private void HourConverterButton_Click(object sender, RoutedEventArgs e)
        {
            SetActiveButton(HourConverterButton);
            LoadFeatureControl(new Feature4Control());
            SetTitleContent("Hours converter");
        }
        #endregion


        // Method to restore the original content of MainContent
        private void RestoreMainContent()
        {
            MainContent.Children.Clear();
            if (_originalMainContent != null)
            {
                // Remove vertical alignment
                MainContent.VerticalAlignment = VerticalAlignment.Stretch;
                MainContent.Children.Add(_originalMainContent);
            }
        }

        // Login button click handler
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userName = await _creatioService.TryLoginAsync();
                if (!string.IsNullOrEmpty(userName))
                {
                    UserName.Text = userName;
                    MessageBox.Show("Login successful");

                    // Fetch the total time for today
                    TimeSpan totalTime = await _creatioService.GetTotalTimeForToday();

                    // Updatethe InfoCard with the total hours today
                    TotalHoursTodayCard.Number = $"{totalTime.TotalHours}h";
                }
                else
                {
                    MessageBox.Show("Unable to retrieve user information");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Refresh button click handler
        private async void refreshDashboardsButton_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan totalTime = await _creatioService.GetTotalTimeForToday();
            TotalHoursTodayCard.Number = $"{totalTime.TotalHours}h";
        }

    }
}
