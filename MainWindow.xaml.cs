using ParkbeheerderDashboard.Models;
using ParkbeheerderDashboard.View;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.Linq;

namespace ParkbeheerderDashboard
{
    public partial class MainWindow : Window
    {
        private readonly ApiService _apiService;
        private Attraction _currentAttraction;
        private Area _currentArea;

        public MainWindow()
        {
            InitializeComponent();
            _apiService = new ApiService();
            AttractiesButton.IsChecked = true;
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadAttractionsAsync();
            InitializeStatusComboBox();
            await LoadMaintenancesAsync();
            await LoadAreasAsync();
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

        private void OnderhoudButton_Checked(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = new OnderhoudControl();
        }

        private void FeedbackButton_Checked(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = new FeedbackControl();
        }

        private void InitializeStatusComboBox()
        {
            var statusComboBox = FindName("StatusComboBox") as ComboBox;
            if (statusComboBox != null)
            {
                statusComboBox.Items.Clear();
                statusComboBox.Items.Add(new ComboBoxItem { Content = "Selecteer status" });

                var predefinedStatuses = new List<string> { "Onderhoud", "Storing", "Weersomstandigheden" };

                foreach (var status in predefinedStatuses)
                {
                    statusComboBox.Items.Add(new ComboBoxItem { Content = status });
                }

                statusComboBox.SelectedIndex = 0;
            }
        }

        private async Task LoadAttractionsAsync()
        {
            try
            {
                var attractions = await _apiService.GetAttractionsAsync();
                Dispatcher.Invoke(() =>
                {
                    var attractionsListView = FindName("AttractionsListView") as ListView;
                    var attractieComboBox = FindName("AttractieComboBox") as ComboBox;

                    if (attractionsListView != null)
                    {
                        attractionsListView.ItemsSource = attractions;
                    }

                    if (attractieComboBox != null)
                    {
                        attractieComboBox.Items.Clear();
                        attractieComboBox.Items.Add(new ComboBoxItem { Content = "Selecteer attractie" });
                        foreach (var attraction in attractions)
                        {
                            attractieComboBox.Items.Add(new ComboBoxItem { Content = attraction.Name, Tag = attraction });
                        }
                        attractieComboBox.SelectedIndex = 0;
                    }
                });
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error fetching attractions: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                    var maintenanceListView = FindName("MaintenanceListView") as ListView;
                    if (maintenanceListView != null)
                    {
                        maintenanceListView.ItemsSource = maintenances;
                    }
                });
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error fetching maintenances: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadAreasAsync()
        {
            try
            {
                var areas = await _apiService.GetAreasAsync();
                Dispatcher.Invoke(() =>
                {
                    var gebiedenListView = FindName("GebiedenListView") as ListView;
                    if (gebiedenListView != null)
                    {
                        gebiedenListView.ItemsSource = areas;
                    }
                });
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error fetching areas: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
