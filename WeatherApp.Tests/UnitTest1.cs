using Xunit;
using WeatherApp; // Adjust if your UserStore is under a namespace



public class UserStoreTests
{
    [Fact]
    public void RegisterUser_AddsNewUser()
    {
            UserStore.ClearAll();
            UserStore.RegisterUser("alice", "1234", "Admin");

        Assert.Single(UserStore.RegisteredUsers);
        Assert.Equal("alice", UserStore.RegisteredUsers[0].Username);
    }

    [Fact]
    public void RegisterUser_DoesNotAddDuplicateUser()
    {
        UserStore.ClearAll();
        UserStore.RegisterUser("bob", "1234", "Scientist");
        UserStore.RegisterUser("bob", "5678", "Scientist"); // Try to register same user

        Assert.Single(UserStore.RegisteredUsers);
        Assert.Equal("1234", UserStore.RegisteredUsers[0].Password); // Keeps the original
    }

    [Fact]
    public void Login_ReturnsCorrectUser()
    {
        UserStore.ClearAll();
        UserStore.RegisterUser("carol", "pass", "Operations Manager");

        var user = UserStore.Login("carol", "pass");

        Assert.NotNull(user);
        Assert.Equal("Operations Manager", user.Role);
    }

    [Fact]
    public void Login_ReturnsNullForInvalidCredentials()
    {
        UserStore.ClearAll();
        UserStore.RegisterUser("dave", "pass", "Admin");

        var result = UserStore.Login("dave", "wrongpass");

        Assert.Null(result);
    }
}
