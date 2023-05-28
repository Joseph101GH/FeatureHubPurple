using System.Windows;
using System.Windows.Input;
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

        private void Feature1Button_Click(object sender, RoutedEventArgs e)
        {
            SetTitleContent();
            LoadFeature1();
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
    }
}
