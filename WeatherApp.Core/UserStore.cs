namespace WeatherApp
{
    public static class UserStore
    {
        public static List<User> RegisteredUsers { get; set; } = new();

        public static bool AddUser(User user)
        {
            if (RegisteredUsers.Any(u => u.Username == user.Username))
                return false; // Username already exists

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
    }
}
