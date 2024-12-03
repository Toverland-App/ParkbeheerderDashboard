using System.Windows;
using System.Windows.Controls;

namespace ParkbeheerderDashboard
{
    public static class ContentManager
    {
        public static void HideAllContent(params UIElement[] elements)
        {
            foreach (var element in elements)
            {
                element.Visibility = Visibility.Collapsed;
            }
        }

        public static void ShowContent(UIElement element)
        {
            element.Visibility = Visibility.Visible;
        }
    }
}
