using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ParkbeheerderDashboard
{
    public partial class MainWindow : Window
    {
        private readonly ApiService _apiService;
        private List<AttractionStatus> _attractionStatuses = new List<AttractionStatus>();

        public MainWindow()
        {
            InitializeComponent();
            _apiService = new ApiService();
            AttractiesButton.IsChecked = true; // Default to Attracties
            Loaded += MainWindow_Loaded; // Correctly call the async method after the window is loaded
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadAttractionsAsync();
            InitializeStatusComboBox();
            LoadStatusListView();
        }

        private async Task LoadAttractionsAsync()
        {
            try
            {
                var attractions = await _apiService.GetAttractionsAsync();
                Dispatcher.Invoke(() =>
                {
                    AttractieComboBox.Items.Clear();
                    AttractieComboBox.Items.Add(new ComboBoxItem { Content = "Selecteer attractie" });
                    foreach (var attraction in attractions)
                    {
                        AttractieComboBox.Items.Add(new ComboBoxItem { Content = attraction.Name });
                    }
                    AttractieComboBox.SelectedIndex = 0;
                });
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error fetching attractions: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitializeStatusComboBox()
        {
            StatusComboBox.Items.Clear();
            StatusComboBox.Items.Add(new ComboBoxItem { Content = "Selecteer status" });

            // Add predefined statuses
            var predefinedStatuses = new List<string> { "Operational", "Maintenance", "Closed" };

            foreach (var status in predefinedStatuses)
            {
                StatusComboBox.Items.Add(new ComboBoxItem { Content = status });
            }

            StatusComboBox.SelectedIndex = 0;
        }

        private void InitializePlaceholders()
        {
            // Initialize placeholders for ComboBoxes
            PlaceholderManager.InitializePlaceholders(AttractieComboBox, StatusComboBox, OpmerkingenBox);
        }

        private async void PostButton_Click(object sender, RoutedEventArgs e)
        {
            string attractie = AttractieComboBox.Text;
            string status = StatusComboBox.Text;
            string opmerkingen = OpmerkingenBox.Text;

            if (attractie == "Selecteer attractie" || status == "Selecteer status" || opmerkingen == "Voer opmerkingen in...")
            {
                MessageBox.Show("Vul alle velden in voordat u post.", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("Statusupdate doorgevoerd.", "Informatie", MessageBoxButton.OK, MessageBoxImage.Information);

                // Voeg de nieuwe status toe aan de lijst
                _attractionStatuses.Add(new AttractionStatus { Name = attractie, Status = status, Opmerkingen = opmerkingen });

                // Reset velden naar hun neutrale positie
                await LoadAttractionsAsync();
                InitializeStatusComboBox();
                SetOpmerkingenBoxPlaceholder();
                LoadStatusListView();
            }
        }

        private void LoadStatusListView()
        {
            StatusListView.ItemsSource = null;
            StatusListView.ItemsSource = _attractionStatuses;
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

        private void OpmerkingenBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (OpmerkingenBox.Text == "Voer opmerkingen in...")
            {
                OpmerkingenBox.Text = "";
                OpmerkingenBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void OpmerkingenBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(OpmerkingenBox.Text))
            {
                SetOpmerkingenBoxPlaceholder();
            }
        }

        private void SetOpmerkingenBoxPlaceholder()
        {
            OpmerkingenBox.Text = "Voer opmerkingen in...";
            OpmerkingenBox.Foreground = System.Windows.Media.Brushes.Gray;
        }
    }

    public class AttractionStatus
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Opmerkingen { get; set; }
    }
}
