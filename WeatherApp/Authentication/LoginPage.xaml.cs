using WeatherApp.Pages;


namespace WeatherApp.Authentication
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        [Obsolete]
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var username = UsernameEntry.Text?.Trim();
            var password = PasswordEntry.Text;

            var user = UserStore.RegisteredUsers.FirstOrDefault(u =>u.Username == username && u.Password == password);


            if (user == null)
            {
                StatusLabel.Text = "Invalid username or password.";
                return;
            }

            // ✅ Navigate based on role
            switch (user.Role)
            {
                case "Scientist":
                    await Navigation.PushAsync(new MainPage()); // Air Quality or Scientist dashboard
                    break;
                case "Administrator":
                    await Navigation.PushAsync(new AdminPage()); // Placeholder for now
                    break;
                case "Operations Manager":
                    await Navigation.PushAsync(new OpsManagerPage()); // Placeholder for now
                    break;
            }
        }
    }
}
