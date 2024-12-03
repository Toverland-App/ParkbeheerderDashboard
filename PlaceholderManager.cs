using System.Windows.Controls;

namespace ParkbeheerderDashboard
{
    public static class PlaceholderManager
    {
        public static void InitializePlaceholders(ComboBox attractieComboBox, ComboBox statusComboBox, TextBox opmerkingenBox)
        {
            SetPlaceholder(attractieComboBox, "Selecteer attractie");
            SetPlaceholder(statusComboBox, "Selecteer status");
            SetPlaceholder(opmerkingenBox, "Voer opmerkingen in...");
        }

        public static void SetPlaceholder(ComboBox comboBox, string placeholder)
        {
            comboBox.Items.Clear();
            comboBox.Items.Add(new ComboBoxItem { Content = placeholder });
            comboBox.SelectedIndex = 0;
        }

        public static void SetPlaceholder(TextBox textBox, string placeholder)
        {
            textBox.Text = placeholder;
        }
    }
}
