using LegacyApp.Models;

namespace LegacyApp.Abstractions
{
    public interface IClientRepository
    {
        Task<Client?> GetByIdAsync(global::System.Int32 id);
    }
}