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

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            _creatioService = new CreatioService();

            // Save the original content of MainContent
            _originalMainContent = MainContent.Children.Count > 0 ? MainContent.Children[0] : null;
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
            LoadFeatureControl(new Feature3Control());
        }

        // Method to load a UserControl into the MainContent control
        private void LoadFeatureControl(UserControl featureControl)
        {
            MainContent.Children.Clear();
            MainContent.Children.Add(featureControl);
            MainContent.VerticalAlignment = VerticalAlignment.Center;
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

        #endregion
        // Method to restore the original content of MainContent
        private void RestoreMainContent()
        {
            MainContent.Children.Clear();
            if (_originalMainContent != null)
            {
                // Remove vertical alignment
                MainContent.VerticalAlignment = VerticalAlignment.Top;
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
