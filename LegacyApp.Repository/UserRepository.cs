using LegacyApp.Abstractions;
using LegacyApp.Models;

namespace LegacyApp.Repository
{
    public class UserRepository : IUserRepository
    {
        public async Task AddUserAsync(User user)
        {
            UserDataAccess.AddUser(user);

            await Task.CompletedTask;
        }
    }
}
