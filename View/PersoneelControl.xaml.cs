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
    public partial class PersoneelControl : UserControl
    {
        private readonly ApiService _apiService;
        private Employee _currentEmployee;

        public PersoneelControl()
        {
            InitializeComponent();
            _apiService = new ApiService();
            Loaded += PersoneelControl_Loaded;
        }

        private async void PersoneelControl_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadEmployeesAsync();
        }

        private async void ToevoegenPersoneelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var employee = new Employee
                {
                    Name = PersoneelNaamBox.Text,
                    Role = PersoneelRolBox.Text,
                    AreaId = int.Parse(PersoneelAreaIdBox.Text),
                    Email = PersoneelEmailBox.Text,
                    PhoneNumber = PersoneelTelefoonnummerBox.Text,
                    HireDate = DateTime.Parse(PersoneelIndiensttredingBox.Text),
                    IsActive = PersoneelActiefBox.IsChecked ?? false
                };

                var success = await _apiService.CreateEmployeeAsync(employee);

                if (success)
                {
                    MessageBox.Show("Personeel succesvol toegevoegd!");
                    await LoadEmployeesAsync();
                }
                else
                {
                    MessageBox.Show("Er is een fout opgetreden bij het toevoegen van het personeel. Controleer de log voor meer details.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Er is een fout opgetreden: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DeletePersoneel_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                var employee = button.DataContext as Employee;
                if (employee != null)
                {
                    var result = MessageBox.Show($"Weet u zeker dat u het personeel '{employee.Name}' wilt verwijderen?", "Bevestiging", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        var success = await _apiService.DeleteEmployeeAsync(employee.Id);
                        if (success)
                        {
                            MessageBox.Show($"Personeel '{employee.Name}' succesvol verwijderd!");
                            await LoadEmployeesAsync();
                        }
                        else
                        {
                            MessageBox.Show("Er is een fout opgetreden bij het verwijderen van het personeel. Controleer de log voor meer details.");
                        }
                    }
                }
            }
        }

        private void EditPersoneel_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                var employee = button.DataContext as Employee;
                if (employee != null)
                {
                    _currentEmployee = employee;
                    PersoneelNameTextBox.Text = _currentEmployee.Name;
                    PersoneelRoleTextBox.Text = _currentEmployee.Role;
                    PersoneelAreaIdTextBox.Text = _currentEmployee.AreaId.ToString();
                    PersoneelEmailTextBox.Text = _currentEmployee.Email;
                    PersoneelPhoneNumberTextBox.Text = _currentEmployee.PhoneNumber;
                    PersoneelHireDateTextBox.Text = _currentEmployee.HireDate.ToString("yyyy-MM-dd");
                    PersoneelActiveCheckBox.IsChecked = _currentEmployee.IsActive;
                    EditPersoneelSection.Visibility = Visibility.Visible;
                    PersoneelContent.Visibility = Visibility.Collapsed;
                }
            }
        }

        private async void SavePersoneelButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentEmployee != null)
            {
                _currentEmployee.Name = PersoneelNameTextBox.Text;
                _currentEmployee.Role = PersoneelRoleTextBox.Text;
                _currentEmployee.AreaId = int.Parse(PersoneelAreaIdTextBox.Text);
                _currentEmployee.Email = PersoneelEmailTextBox.Text;
                _currentEmployee.PhoneNumber = PersoneelPhoneNumberTextBox.Text;
                _currentEmployee.HireDate = DateTime.Parse(PersoneelHireDateTextBox.Text);
                _currentEmployee.IsActive = PersoneelActiveCheckBox.IsChecked ?? false;

                var success = await _apiService.UpdateEmployeeAsync(_currentEmployee.Id, _currentEmployee);
                if (success)
                {
                    MessageBox.Show("Personeel succesvol bijgewerkt!");
                    EditPersoneelSection.Visibility = Visibility.Collapsed;
                    PersoneelContent.Visibility = Visibility.Visible;
                    await LoadEmployeesAsync();
                }
                else
                {
                    MessageBox.Show("Er is een fout opgetreden bij het bijwerken van het personeel. Controleer de log voor meer details.");
                }
            }
        }

        private void CancelPersoneelButton_Click(object sender, RoutedEventArgs e)
        {
            EditPersoneelSection.Visibility = Visibility.Collapsed;
            PersoneelContent.Visibility = Visibility.Visible;
        }

        private async Task LoadEmployeesAsync()
        {
            try
            {
                var employeeList = await _apiService.GetEmployeesAsync();
                Dispatcher.Invoke(() =>
                {
                    PersoneelListView.ItemsSource = employeeList;
                });
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error fetching personnel: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
