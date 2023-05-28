using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FeatureHubPurple.Services;
using FeatureHubPurple.UserControls;

namespace FeatureHubPurple
{
    public partial class MainWindow : Window
    {
        
        public static MainWindow Instance
        {
            get; private set;
        }

        public MainWindow()
        {
           
            InitializeComponent();
            Instance = this;
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

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

        private void GuidCheckerButton_Click(object sender, RoutedEventArgs e)
        {
            SetActiveButton(GuidCheckerButton);

            LoadFeature1();
            SetTitleContent();

        }

        private void Feature2Button_Click(object sender, RoutedEventArgs e)
        {
            LoadFeature2();
        }

        private void Feature3Button_Click(object sender, RoutedEventArgs e)
        {
            LoadFeature3();
        }

        private void LoadFeature1()
        {
            var content = new Feature1Control();
            MainContent.Children.Clear();
            MainContent.Children.Add(content);
        }

        private void SetActiveButton(Button activeButton)
        {
            // Reset all button styles to "menuButton"
            DashboardButton.Style = (Style)FindResource("menuButton");
            GuidCheckerButton.Style = (Style)FindResource("menuButton");
            // Add more buttons here if you have them...

            // Set the clicked button's style to "menuButtonActive"
            activeButton.Style = (Style)FindResource("menuButtonActive");
        }


        private void SetTitleContent()
        {
            pageTitle.Text = "GUID-Checker";
        }

        private void LoadFeature2()
        {
            var content = new Feature2Control();
            MainContent.Children.Clear();
            MainContent.Children.Add(content);
        }

        private void LoadFeature3()
        {
            var content = new Feature3Control();
            MainContent.Children.Clear();
            MainContent.Children.Add(content);
        }

        private void Feature4Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Feature5Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear the content of the MainContent Grid
            MainContent.Children.Clear();
            SetActiveButton(DashboardButton);

        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
