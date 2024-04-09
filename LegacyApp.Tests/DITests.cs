using LegacyApp.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace LegacyApp.Tests
{
    public class DITests
    {
        [Fact]
        public void ClientRepository_Instantiation()
        {
            var services = new ServiceCollection();
            services.AddLegacyApp();

            var serviceProvider = services.BuildServiceProvider();

            var clientRepository = serviceProvider.GetRequiredService<IClientRepository>();

            Assert.NotNull(clientRepository);                                   
        }

        [Fact]
        public void UserRepository_Instantiation()
        {
            var services = new ServiceCollection();
            services.AddLegacyApp();

            var serviceProvider = services.BuildServiceProvider();

            var userRepository = serviceProvider.GetRequiredService<IUserRepository>();

            Assert.NotNull(userRepository);
        }
    }
}