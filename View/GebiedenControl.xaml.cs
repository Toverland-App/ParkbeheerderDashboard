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
    public partial class GebiedenControl : UserControl
    {
        private readonly ApiService _apiService;
        private Area _currentArea;

        public GebiedenControl()
        {
            InitializeComponent();
            _apiService = new ApiService();
            Loaded += GebiedenControl_Loaded;
        }

        private async void GebiedenControl_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadAreasAsync();
        }

        private async void ToevoegenGebiedButton_Click(object sender, RoutedEventArgs e)
        {
            var gebied = new
            {
                id = 0,
                name = GebiedNaamBox.Text,
                size = 0
            };

            var success = await _apiService.CreateAreaAsync(gebied);

            if (success)
            {
                MessageBox.Show("Gebied succesvol toegevoegd!");
                await LoadAreasAsync();
            }
            else
            {
                MessageBox.Show("Er is een fout opgetreden bij het toevoegen van het gebied.");
            }
        }

        private async Task LoadAreasAsync()
        {
            try
            {
                var areas = await _apiService.GetAreasAsync();
                Dispatcher.Invoke(() =>
                {
                    GebiedenListView.ItemsSource = areas;
                });
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error fetching areas: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DeleteGebied_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                var area = button.DataContext as Area;
                if (area != null)
                {
                    var result = MessageBox.Show($"Weet u zeker dat u het gebied '{area.Name}' wilt verwijderen?", "Bevestiging", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        var success = await _apiService.DeleteAreaAsync(area.Id);
                        if (success)
                        {
                            MessageBox.Show($"Gebied '{area.Name}' succesvol verwijderd!");
                            await LoadAreasAsync();
                        }
                        else
                        {
                            MessageBox.Show("Er is een fout opgetreden bij het verwijderen van het gebied. Controleer de log voor meer details.");
                        }
                    }
                }
            }
        }

        private void EditGebied_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                var area = button.DataContext as Area;
                if (area != null)
                {
                    _currentArea = area;
                    GebiedNameTextBox.Text = _currentArea.Name;
                    GebiedenContent.Visibility = Visibility.Collapsed;
                    EditGebiedSection.Visibility = Visibility.Visible;
                }
            }
        }

        private async void SaveGebiedButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentArea != null)
            {
                _currentArea.Name = GebiedNameTextBox.Text;

                var success = await _apiService.UpdateAreaAsync(_currentArea.Id, _currentArea);
                if (success)
                {
                    MessageBox.Show("Gebied succesvol bijgewerkt!");
                    EditGebiedSection.Visibility = Visibility.Collapsed;
                    GebiedenContent.Visibility = Visibility.Visible;
                    await LoadAreasAsync();
                }
                else
                {
                    MessageBox.Show("Er is een fout opgetreden bij het bijwerken van het gebied. Controleer de log voor meer details.");
                }
            }
        }

        private void CancelGebiedButton_Click(object sender, RoutedEventArgs e)
        {
            EditGebiedSection.Visibility = Visibility.Collapsed;
            GebiedenContent.Visibility = Visibility.Visible;
        }
    }
}
