using ParkbeheerderDashboard.Models;
using System;
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
        private Attraction _currentAttraction;
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
            await LoadMaintenancesAsync();
        }

        private async void ToevoegenButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var attraction = new Attraction
                {
                    Name = AttractieNaamBox.Text,
                    MinHeight = double.Parse(MinHeightBox.Text),
                    AreaId = int.Parse(AreaIdBox.Text),
                    Description = DescriptionBox.Text,
                    OpeningTime = OpeningTimeBox.Text,
                    ClosingTime = ClosingTimeBox.Text,
                    Capacity = int.Parse(CapacityBox.Text),
                    QueueSpeed = int.Parse(QueueSpeedBox.Text),
                    QueueLength = int.Parse(QueueLengthBox.Text)
                };

                var success = await _apiService.CreateAttractionAsync(attraction);

                if (success)
                {
                    MessageBox.Show("Attractie succesvol toegevoegd!");
                    await LoadAttractionsAsync(); // Refresh the list after addition
                }
                else
                {
                    MessageBox.Show("Er is een fout opgetreden bij het toevoegen van de attractie. Controleer de log voor meer details.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Er is een fout opgetreden: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DeleteAttraction_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                var attraction = button.DataContext as Attraction;
                if (attraction != null)
                {
                    var result = MessageBox.Show($"Weet u zeker dat u de attractie '{attraction.Name}' wilt verwijderen?", "Bevestiging", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        var success = await _apiService.DeleteAttractionAsync(attraction.Id);
                        if (success)
                        {
                            MessageBox.Show($"Attractie '{attraction.Name}' succesvol verwijderd!");
                            await LoadAttractionsAsync(); // Refresh the list after deletion
                        }
                        else
                        {
                            MessageBox.Show("Er is een fout opgetreden bij het verwijderen van de attractie. Controleer de log voor meer details.");
                        }
                    }
                }
            }
        }


        private async void EditAttraction_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                var attraction = button.DataContext as Attraction;
                if (attraction != null)
                {
                    _currentAttraction = attraction;
                    NameTextBox.Text = _currentAttraction.Name;
                    MinHeightTextBox.Text = _currentAttraction.MinHeight.ToString();
                    AreaIdTextBox.Text = _currentAttraction.AreaId.ToString();
                    DescriptionTextBox.Text = _currentAttraction.Description;
                    OpeningTimeTextBox.Text = _currentAttraction.OpeningTime;
                    ClosingTimeTextBox.Text = _currentAttraction.ClosingTime;
                    CapacityTextBox.Text = _currentAttraction.Capacity.ToString();
                    QueueSpeedTextBox.Text = _currentAttraction.QueueSpeed.ToString();
                    QueueLengthTextBox.Text = _currentAttraction.QueueLength.ToString();
                    EditSection.Visibility = Visibility.Visible;
                }
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentAttraction != null)
            {
                _currentAttraction.Name = NameTextBox.Text;
                _currentAttraction.MinHeight = double.Parse(MinHeightTextBox.Text);
                _currentAttraction.AreaId = int.Parse(AreaIdTextBox.Text);
                _currentAttraction.Description = DescriptionTextBox.Text;
                _currentAttraction.OpeningTime = OpeningTimeTextBox.Text;
                _currentAttraction.ClosingTime = ClosingTimeTextBox.Text;
                _currentAttraction.Capacity = int.Parse(CapacityTextBox.Text);
                _currentAttraction.QueueSpeed = int.Parse(QueueSpeedTextBox.Text);
                _currentAttraction.QueueLength = int.Parse(QueueLengthTextBox.Text);

                var success = await _apiService.UpdateAttractionAsync(_currentAttraction.Id, _currentAttraction);
                if (success)
                {
                    MessageBox.Show("Attraction successfully updated!");
                    EditSection.Visibility = Visibility.Collapsed;
                    await LoadAttractionsAsync(); // Refresh the list after update
                }
                else
                {
                    MessageBox.Show("An error occurred while updating the attraction. Check the log for more details.");
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            EditSection.Visibility = Visibility.Collapsed;
        }


        private async Task LoadAttractionsAsync()
        {
            try
            {
                var attractions = await _apiService.GetAttractionsAsync();
                Dispatcher.Invoke(() =>
                {
                    AttractionsListView.ItemsSource = attractions;
                    AttractieComboBox.Items.Clear();
                    AttractieComboBox.Items.Add(new ComboBoxItem { Content = "Selecteer attractie" });
                    foreach (var attraction in attractions)
                    {
                        AttractieComboBox.Items.Add(new ComboBoxItem { Content = attraction.Name, Tag = attraction });
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
            var predefinedStatuses = new List<string> { "Onderhoud", "Storing", "Weersomstandigheden" };

            foreach (var status in predefinedStatuses)
            {
                StatusComboBox.Items.Add(new ComboBoxItem { Content = status });
            }

            StatusComboBox.SelectedIndex = 0;
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
                // Stuur de gegevens naar de API
                var selectedAttraction = (ComboBoxItem)AttractieComboBox.SelectedItem;
                int attractionId = selectedAttraction != null && selectedAttraction.Tag != null ? ((Attraction)selectedAttraction.Tag).Id : 0;

                bool success = await _apiService.AddMaintenanceAsync(attractionId, attractie, status, opmerkingen);

                if (success)
                {
                    MessageBox.Show("Statusupdate doorgevoerd.", "Informatie", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Reset velden naar hun neutrale positie
                    await LoadAttractionsAsync();
                    InitializeStatusComboBox();
                    SetOpmerkingenBoxPlaceholder();
                    await LoadMaintenancesAsync();
                }
                else
                {
                    MessageBox.Show("Fout bij het doorvoeren van de statusupdate.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task LoadMaintenancesAsync()
        {
            try
            {
                var maintenances = await _apiService.GetAllMaintenancesAsync();
                Dispatcher.Invoke(() =>
                {
                    MaintenanceListView.ItemsSource = maintenances;
                });
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error fetching maintenances: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetOpmerkingenBoxPlaceholder()
        {
            OpmerkingenBox.Text = "Voer opmerkingen in...";
            OpmerkingenBox.Foreground = System.Windows.Media.Brushes.Gray;
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

        private void InformatiebordButton_Checked(object sender, RoutedEventArgs e)
        {
            ContentManager.HideAllContent(InformatiebordContent, AttractiesContent, PersoneelContent, GebiedenContent, BezoekersContent, OnderhoudContent, FeedbackContent);
            ContentManager.ShowContent(InformatiebordContent);
        }

        private void AttractiesButton_Checked(object sender, RoutedEventArgs e)
        {
            ContentManager.HideAllContent(InformatiebordContent, AttractiesContent, PersoneelContent, GebiedenContent, BezoekersContent, OnderhoudContent, FeedbackContent);
            ContentManager.ShowContent(AttractiesContent);
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

    public class AttractionStatus
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Opmerkingen { get; set; }
    }
}
