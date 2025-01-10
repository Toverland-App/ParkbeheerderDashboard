using ParkbeheerderDashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ParkbeheerderDashboard.View
{
    public partial class AttractiesControl : UserControl
    {
        private readonly ApiService _apiService;
        private Attraction _currentAttraction;

        public AttractiesControl()
        {
            InitializeComponent();
            _apiService = new ApiService();
            Loaded += AttractiesControl_Loaded;
        }

        private async void AttractiesControl_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadAreasAsync();
            await LoadAttractionsAsync();
            InitializeStatusComboBox();
            await LoadMaintenancesAsync();
        }

        private async Task LoadAreasAsync()
        {
            try
            {
                var areas = await _apiService.GetAreasAsync();
                Dispatcher.Invoke(() =>
                {
                    AreaIdComboBox.ItemsSource = areas;
                    AreaIdComboBox.DisplayMemberPath = "Name";
                    AreaIdComboBox.SelectedValuePath = "Id";

                    AreaIdEditComboBox.ItemsSource = areas;
                    AreaIdEditComboBox.DisplayMemberPath = "Name";
                    AreaIdEditComboBox.SelectedValuePath = "Id";
                });
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error fetching areas: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ToevoegenButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateAttractionInputs())
            {
                return;
            }

            try
            {
                var attraction = new Attraction
                {
                    Name = AttractieNaamBox.Text,
                    MinHeight = double.Parse(MinHeightBox.Text),
                    AreaId = (int)AreaIdComboBox.SelectedValue,
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
                    await LoadAttractionsAsync();
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
                            await LoadAttractionsAsync();
                        }
                        else
                        {
                            MessageBox.Show("Er is een fout opgetreden bij het verwijderen van de attractie. Controleer de log voor meer details.");
                        }
                    }
                }
            }
        }

        private void EditAttraction_Click(object sender, RoutedEventArgs e)
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
                    AreaIdEditComboBox.SelectedValue = _currentAttraction.AreaId;
                    DescriptionTextBox.Text = _currentAttraction.Description;
                    OpeningTimeTextBox.Text = _currentAttraction.OpeningTime;
                    ClosingTimeTextBox.Text = _currentAttraction.ClosingTime;
                    CapacityTextBox.Text = _currentAttraction.Capacity.ToString();
                    QueueSpeedTextBox.Text = _currentAttraction.QueueSpeed.ToString();
                    QueueLengthTextBox.Text = _currentAttraction.QueueLength.ToString();
                    EditSection.Visibility = Visibility.Visible;
                    AttractiesContent.Visibility = Visibility.Collapsed;
                }
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateAttractionInputs(true))
            {
                return;
            }

            if (_currentAttraction != null)
            {
                _currentAttraction.Name = NameTextBox.Text;
                _currentAttraction.MinHeight = double.Parse(MinHeightTextBox.Text);
                _currentAttraction.AreaId = (int)AreaIdEditComboBox.SelectedValue;
                _currentAttraction.Description = DescriptionTextBox.Text;
                _currentAttraction.OpeningTime = OpeningTimeTextBox.Text;
                _currentAttraction.ClosingTime = ClosingTimeTextBox.Text;
                _currentAttraction.Capacity = int.Parse(CapacityTextBox.Text);
                _currentAttraction.QueueSpeed = int.Parse(QueueSpeedTextBox.Text);
                _currentAttraction.QueueLength = int.Parse(QueueLengthTextBox.Text);

                var success = await _apiService.UpdateAttractionAsync(_currentAttraction.Id, _currentAttraction);
                if (success)
                {
                    MessageBox.Show("Attractie succesvol bijgewerkt!");
                    EditSection.Visibility = Visibility.Collapsed;
                    AttractiesContent.Visibility = Visibility.Visible;
                    await LoadAttractionsAsync();
                }
                else
                {
                    MessageBox.Show("Er vindt een error plaats bij het bijwerken van de attractie.");
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            EditSection.Visibility = Visibility.Collapsed;
            AttractiesContent.Visibility = Visibility.Visible;
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

            var predefinedStatuses = new List<string> { "Onderhoud", "Storing", "Weersomstandigheden" };

            foreach (var status in predefinedStatuses)
            {
                StatusComboBox.Items.Add(new ComboBoxItem { Content = status });
            }

            StatusComboBox.SelectedIndex = 0;
        }

        private async Task LoadMaintenancesAsync()
        {
            try
            {
                var maintenances = await _apiService.GetAllMaintenancesAsync();
                var attractions = await _apiService.GetAttractionsAsync();

                foreach (var maintenance in maintenances)
                {
                    var attraction = attractions.FirstOrDefault(a => a.Id == maintenance.AttractionId);
                    if (attraction != null)
                    {
                        maintenance.AttractionName = attraction.Name;
                    }
                }

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

        private void OpmerkingenBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (OpmerkingenBox.Text == "Voer opmerkingen in...")
            {
                OpmerkingenBox.Text = "";
                OpmerkingenBox.Foreground = Brushes.Black;
            }
        }

        private void OpmerkingenBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(OpmerkingenBox.Text))
            {
                OpmerkingenBox.Text = "Voer opmerkingen in...";
                OpmerkingenBox.Foreground = Brushes.Gray;
            }
        }

        private async void PostButton_Click(object sender, RoutedEventArgs e)
        {
            string attractie = AttractieComboBox.Text;
            string status = StatusComboBox.Text;
            string opmerkingen = OpmerkingenBox.Text;

            if (attractie == "Selecteer attractie" || status == "Selecteer status" || opmerkingen == "Voer opmerkingen in...")
            {
                MessageBox.Show("Vul alle velden in voordat u toevoegd.", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                var selectedAttraction = (ComboBoxItem)AttractieComboBox.SelectedItem;
                int attractionId = selectedAttraction != null && selectedAttraction.Tag != null ? ((Attraction)selectedAttraction.Tag).Id : 0;

                bool success = await _apiService.AddMaintenanceAsync(attractionId, attractie, status, opmerkingen);

                if (success)
                {
                    MessageBox.Show("Statusupdate doorgevoerd.", "Informatie", MessageBoxButton.OK, MessageBoxImage.Information);

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

        private void SetOpmerkingenBoxPlaceholder()
        {
            OpmerkingenBox.Text = "Voer opmerkingen in...";
            OpmerkingenBox.Foreground = Brushes.Gray;
        }

        private async void DeleteMaintenance_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                var maintenance = button.DataContext as Maintenance;
                if (maintenance != null)
                {
                    var result = MessageBox.Show($"Weet u zeker dat u de status '{maintenance.Status}' voor attractie '{maintenance.AttractionName}' wilt verwijderen?", "Bevestiging", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        var success = await _apiService.DeleteMaintenanceAsync(maintenance.Id);
                        if (success)
                        {
                            MessageBox.Show($"Status '{maintenance.Status}' voor attractie '{maintenance.AttractionName}' succesvol verwijderd!");
                            await LoadMaintenancesAsync();
                        }
                        else
                        {
                            MessageBox.Show("Er is een fout opgetreden bij het verwijderen van de status. Controleer de log voor meer details.");
                        }
                    }
                }
            }
        }

        private bool ValidateAttractionInputs(bool isEdit = false)
        {
            if (isEdit)
            {
                if (string.IsNullOrWhiteSpace(NameTextBox.Text))
                {
                    MessageBox.Show("Naam van de attractie is verplicht.", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (!double.TryParse(MinHeightTextBox.Text, out _))
                {
                    MessageBox.Show("Min Height van de attractie moet een geldig getal zijn.", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (AreaIdEditComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Selecteer een geldige AreaId voor de attractie.", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
                {
                    MessageBox.Show("Description van de attractie is verplicht.", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(OpeningTimeTextBox.Text))
                {
                    MessageBox.Show("Opening Time van de attractie is verplicht.", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(ClosingTimeTextBox.Text))
                {
                    MessageBox.Show("Closing Time van de attractie is verplicht.", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (!int.TryParse(CapacityTextBox.Text, out _))
                {
                    MessageBox.Show("Capacity van de attractie moet een geldig getal zijn.", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (!int.TryParse(QueueSpeedTextBox.Text, out _))
                {
                    MessageBox.Show("Queue Speed van de attractie moet een geldig getal zijn.", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (!int.TryParse(QueueLengthTextBox.Text, out _))
                {
                    MessageBox.Show("Queue Length van de attractie moet een geldig getal zijn.", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(AttractieNaamBox.Text))
                {
                    MessageBox.Show("Naam van de attractie is verplicht.", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (!double.TryParse(MinHeightBox.Text, out _))
                {
                    MessageBox.Show("Min Height van de attractie moet een geldig getal zijn.", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (AreaIdComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Selecteer een geldige AreaId voor de attractie.", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(DescriptionBox.Text))
                {
                    MessageBox.Show("Description van de attractie is verplicht.", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(OpeningTimeBox.Text))
                {
                    MessageBox.Show("Opening Time van de attractie is verplicht.", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(ClosingTimeBox.Text))
                {
                    MessageBox.Show("Closing Time van de attractie is verplicht.", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (!int.TryParse(CapacityBox.Text, out _))
                {
                    MessageBox.Show("Capacity van de attractie moet een geldig getal zijn.", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (!int.TryParse(QueueSpeedBox.Text, out _))
                {
                    MessageBox.Show("Queue Speed van de attractie moet een geldig getal zijn.", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (!int.TryParse(QueueLengthBox.Text, out _))
                {
                    MessageBox.Show("Queue Length van de attractie moet een geldig getal zijn.", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }

            return true;
        }
    }
}

