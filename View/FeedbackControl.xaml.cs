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
    public partial class FeedbackControl : UserControl
    {
        private readonly ApiService _apiService;

        public FeedbackControl()
        {
            InitializeComponent();
            _apiService = new ApiService();
            Loaded += FeedbackControl_Loaded;
        }

        private async void FeedbackControl_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadFeedbackAsync();
        }

        private async Task LoadFeedbackAsync()
        {
            try
            {
                var feedbackList = await _apiService.GetFeedbackAsync();
                var groupedFeedback = feedbackList.GroupBy(f => f.Rating)
                                                  .Select(g => new { Rating = g.Key, Count = g.Count() })
                                                  .ToDictionary(g => g.Rating, g => g.Count);

                Dispatcher.Invoke(() =>
                {
                    Smiley1Counter.Text = groupedFeedback.ContainsKey(1) ? groupedFeedback[1].ToString() : "0";
                    Smiley2Counter.Text = groupedFeedback.ContainsKey(2) ? groupedFeedback[2].ToString() : "0";
                    Smiley3Counter.Text = groupedFeedback.ContainsKey(3) ? groupedFeedback[3].ToString() : "0";
                    Smiley4Counter.Text = groupedFeedback.ContainsKey(4) ? groupedFeedback[4].ToString() : "0";
                    Smiley5Counter.Text = groupedFeedback.ContainsKey(5) ? groupedFeedback[5].ToString() : "0";
                });
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error fetching feedback: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
