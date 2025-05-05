using WeatherApp.Models;

namespace WeatherApp.Pages
{
    public partial class UserBackupPage : ContentPage
    {
        public UserBackupPage(List<User> users)
        {
            InitializeComponent();
            UserBackupListView.ItemsSource = users;
        }
    }
}
