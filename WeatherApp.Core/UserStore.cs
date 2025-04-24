using WeatherApp;

namespace WeatherApp
{
    public static class UserStore
    {
        public static List<User> RegisteredUsers { get; set; } = new();

        public static bool AddUser(string username, string password, string role)
        {
            if (RegisteredUsers.Any(u => u.Username == username))
                return false; // Username already exists

            var user = new User { Username = username, Password = password, Role = role };
            RegisteredUsers.Add(user);
            return true;
        }


        public static bool RemoveUser(string username)
        {
            var user = RegisteredUsers.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                RegisteredUsers.Remove(user);
                return true;
            }
            return false;
        }

        // Login method to check the user's credentials
        public static User? Login(string username, string password)
        {
            return RegisteredUsers.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        // Clears all registered users (used in tests)
        public static void ClearAll()
        {
            RegisteredUsers.Clear();
        }
    }
}
