using ParkbeheerderDashboard.View;
using System.Windows;

namespace ParkbeheerderDashboard
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginOverlay_LoginSuccessful(object sender, System.EventArgs e)
        {
            LoginOverlay.Visibility = Visibility.Collapsed;
        }

        private void AttractiesButton_Checked(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = new AttractiesControl();
        }

        private void PersoneelButton_Checked(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = new PersoneelControl();
        }

        private void GebiedenButton_Checked(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = new GebiedenControl();
        }

        private void BezoekersButton_Checked(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = new BezoekersControl();
        }

        private void FeedbackButton_Checked(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = new FeedbackControl();
        }

        private void LoginOverlay_Loaded()
        {

        }
    }
}

