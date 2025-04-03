namespace WeatherApp
{
    public static class UserStore
    {
        public static List<User> RegisteredUsers { get; set; } = new();

        public static void RegisterUser(string username, string password, string role)
        {
            if (!RegisteredUsers.Any(u => u.Username == username))
            {
                RegisteredUsers.Add(new User { Username = username, Password = password, Role = role });
            }
        }

        public static User? Login(string username, string password)
        {
            return RegisteredUsers.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public static void ClearAll()
        {
            RegisteredUsers.Clear();
        }
    }
}
