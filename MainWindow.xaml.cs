using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ParkbeheerderDashboard
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AttractiesButton.IsChecked = true; // Default to Attracties
            Loaded += MainWindow_Loaded; // Correctly call the async method after the window is loaded
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Load attractions or other initialization logic here
        }

        private void InitializePlaceholders()
        {
            // Initialize placeholders for ComboBoxes
            PlaceholderManager.InitializePlaceholders(AttractieComboBox, StatusComboBox, OpmerkingenBox);
        }

        private void PostButton_Click(object sender, RoutedEventArgs e)
        {
            // Implement the logic for the Post button click event
            string attractie = AttractieComboBox.Text;
            string status = StatusComboBox.Text;
            string opmerkingen = OpmerkingenBox.Text;

            if (attractie == "Selecteer attractie" || status == "Selecteer status" || opmerkingen == "Voer opmerkingen in...")
            {
                MessageBox.Show("Vul alle velden in voordat u post.", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                // Logic to handle the post action
                MessageBox.Show($"Attractie: {attractie}\nStatus: {status}\nOpmerkingen: {opmerkingen}", "Gegevens gepost", MessageBoxButton.OK, MessageBoxImage.Information);

                // Reset fields to their neutral position
                PlaceholderManager.SetPlaceholder(AttractieComboBox, "Selecteer attractie");
                PlaceholderManager.SetPlaceholder(StatusComboBox, "Selecteer status");
                PlaceholderManager.SetPlaceholder(OpmerkingenBox, "Voer opmerkingen in...");
            }
        }

        private void InformatiebordButton_Checked(object sender, RoutedEventArgs e)
        {
            ContentManager.HideAllContent(InformatiebordContent, AttractiesContent, PersoneelContent, GebiedenContent, BezoekersContent, OnderhoudContent, FeedbackContent);
            ContentManager.ShowContent(InformatiebordContent);
        }

        private void AttractiesButton_Checked(object sender, RoutedEventArgs e)
        {
            ContentManager.HideAllContent(InformatiebordContent, AttractiesContent, PersoneelContent, GebiedenContent, BezoekersContent, OnderhoudContent, FeedbackContent);
            ContentManager.ShowContent(AttractiesContent);
            InitializePlaceholders();
        }

        private void PersoneelButton_Checked(object sender, RoutedEventArgs e)
        {
            ContentManager.HideAllContent(InformatiebordContent, AttractiesContent, PersoneelContent, GebiedenContent, BezoekersContent, OnderhoudContent, FeedbackContent);
            ContentManager.ShowContent(PersoneelContent);
        }

        private void GebiedenButton_Checked(object sender, RoutedEventArgs e)
        {
            ContentManager.HideAllContent(InformatiebordContent, AttractiesContent, PersoneelContent, GebiedenContent, BezoekersContent, OnderhoudContent, FeedbackContent);
            ContentManager.ShowContent(GebiedenContent);
        }

        private void BezoekersButton_Checked(object sender, RoutedEventArgs e)
        {
            ContentManager.HideAllContent(InformatiebordContent, AttractiesContent, PersoneelContent, GebiedenContent, BezoekersContent, OnderhoudContent, FeedbackContent);
            ContentManager.ShowContent(BezoekersContent);
        }

        private void OnderhoudButton_Checked(object sender, RoutedEventArgs e)
        {
            ContentManager.HideAllContent(InformatiebordContent, AttractiesContent, PersoneelContent, GebiedenContent, BezoekersContent, OnderhoudContent, FeedbackContent);
            ContentManager.ShowContent(OnderhoudContent);
        }

        private void FeedbackButton_Checked(object sender, RoutedEventArgs e)
        {
            ContentManager.HideAllContent(InformatiebordContent, AttractiesContent, PersoneelContent, GebiedenContent, BezoekersContent, OnderhoudContent, FeedbackContent);
            ContentManager.ShowContent(FeedbackContent);
        }
    }
}
