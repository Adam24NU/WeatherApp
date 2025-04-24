using Xunit;
using WeatherApp;

namespace WeatherApp.Tests
{
    public class UserStoreTests
    {
        [Fact]
        public void AddUser_AddsNewUser()
        {
            UserStore.RegisteredUsers.Clear(); // Clear any existing users before the test
            var result = UserStore.AddUser("alice", "password123", "Admin"); // Using three arguments

            Assert.True(result);
            Assert.Single(UserStore.RegisteredUsers);
            Assert.Equal("alice", UserStore.RegisteredUsers[0].Username);
        }

        [Fact]
        public void AddUser_DoesNotAddDuplicateUser()
        {
            UserStore.RegisteredUsers.Clear();
            var user1 = new User { Username = "bob", Password = "1234", Role = "Scientist" };
            UserStore.AddUser("bob", "1234", "Scientist");

            var user2 = new User { Username = "bob", Password = "5678", Role = "Scientist" }; // Try to register same user
            var result = UserStore.AddUser("bob", "5678", "Scientist");

            Assert.False(result); // User with the same username shouldn't be added
            Assert.Single(UserStore.RegisteredUsers); // Only one user should be added
        }

        [Fact]
        public void RemoveUser_RemovesUserSuccessfully()
        {
            UserStore.RegisteredUsers.Clear();
            var user = new User { Username = "carol", Password = "pass", Role = "Operations Manager" };
            UserStore.AddUser("carol", "pass", "Operations Manager");

            var result = UserStore.RemoveUser("carol");

            Assert.True(result); // Should return true since the user exists
            Assert.Empty(UserStore.RegisteredUsers); // The list should be empty after removal
        }

        [Fact]
        public void RemoveUser_ReturnsFalseWhenUserNotFound()
        {
            UserStore.RegisteredUsers.Clear();
            var result = UserStore.RemoveUser("nonexistent");

            Assert.False(result); // Should return false because the user does not exist
        }

        [Fact]
        public void Login_ShouldReturnCorrectUser_ForValidCredentials()
        {
            UserStore.RegisteredUsers.Clear();
            UserStore.AddUser("alice", "password123", "Admin");

            var user = UserStore.Login("alice", "password123");

            Assert.NotNull(user); // User should be found
            Assert.Equal("alice", user.Username); // The username should match
            Assert.Equal("Admin", user.Role); // The role should be "Admin"
        }

        [Fact]
        public void Login_ShouldReturnNull_ForInvalidCredentials()
        {
            UserStore.RegisteredUsers.Clear();
            UserStore.AddUser("bob", "password123", "Scientist");

            var result = UserStore.Login("bob", "wrongpassword");

            Assert.Null(result); // Should return null for wrong credentials
        }
    }
}
