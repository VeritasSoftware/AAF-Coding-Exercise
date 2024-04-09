using LegacyApp.Models;

namespace LegacyApp.Abstractions
{
    public interface IUserRepository
    {
        public Task AddUserAsync(User user);
    }
}
