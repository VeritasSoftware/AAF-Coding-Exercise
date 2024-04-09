using LegacyApp.Abstractions;
using LegacyApp.Models;
using LegacyApp.Services;
using Moq;

namespace LegacyApp.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task AddUserAsync_Pass()
        {
            var client = new Client
            {
                Id = 1,
                ClientStatus = ClientStatus.Titanium,
                Type = ClientType.VeryImportant,
                Name = "Test Client",
            };

            var mockClientRepository = new Mock<IClientRepository>();
            mockClientRepository.Setup(x => x.GetByIdAsync(1))
                                .ReturnsAsync(client);

            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(x => x.AddUserAsync(It.IsAny<User>()))
                              .Returns(Task.CompletedTask);

            var mockUserCreditClient = new Mock<IUserCreditService>();
            mockUserCreditClient.Setup(c => c.GetCreditLimitAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                                .ReturnsAsync(100);

            var userService = new UserService(
                                                mockClientRepository.Object,
                                                mockUserRepository.Object,
                                                mockUserCreditClient.Object
                                             );

            var user = await userService.AddUserAsync("Test", "User", "abc@xyz.com", new DateTime(1975, 1, 1), 1);

            Assert.NotNull(user);
            Assert.Equal("Test", user.Firstname);
            Assert.Equal("User", user.Surname);
            Assert.Equal(new DateTime(1975, 1, 1), user.DateOfBirth);
            Assert.Equal(client.Name, user.Client!.Name);
            Assert.Equal(client.Id, user.Client!.Id);
        }

        [Fact]
        public async Task AddUserAsync_ClientNotFound_Fail()
        {
            var clientId = 1;
            Client? client = null;

            var mockClientRepository = new Mock<IClientRepository>();
            mockClientRepository.Setup(x => x.GetByIdAsync(1))
                                .ReturnsAsync(client);

            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(x => x.AddUserAsync(It.IsAny<User>()))
                              .Returns(Task.CompletedTask);

            var mockUserCreditClient = new Mock<IUserCreditService>();
            mockUserCreditClient.Setup(c => c.GetCreditLimitAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                                .ReturnsAsync(100);

            var userService = new UserService(
                                                mockClientRepository.Object,
                                                mockUserRepository.Object,
                                                mockUserCreditClient.Object
                                             );
            
            try
            {
                var user = await userService.AddUserAsync("Test", "User", "abc@xyz.com", new DateTime(1975, 1, 1), clientId);
            }
            catch (InvalidOperationException ex)
            {
                Assert.Equal($"client with client id {clientId} not found.", ex.Message);
            }
        }
    }
}
