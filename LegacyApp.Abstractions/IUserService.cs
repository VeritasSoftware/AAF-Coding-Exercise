using LegacyApp.Models;

namespace LegacyApp.Abstractions
{
    public interface IUserService
    {
        Task<User> AddUser(global::System.String firstName, global::System.String surname, global::System.String email, DateTime dateOfBirth, global::System.Int32 clientId);
    }
}