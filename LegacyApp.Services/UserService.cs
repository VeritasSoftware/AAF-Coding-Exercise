using LegacyApp.Abstractions;
using LegacyApp.Models;
using System.Net.Mail;

namespace LegacyApp.Services
{
    public class UserService : IUserService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserCreditService _userCreditService;

        public UserService(IClientRepository clientRepository,
                            IUserRepository userRepository,
                            IUserCreditService userCreditService)
        {
            _clientRepository = clientRepository;
            _userCreditService = userCreditService;
            _userRepository = userRepository;
        }

        public async Task<User> AddUserAsync(string firstName, string surname, string email, DateTime dateOfBirth, int clientId)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(surname))
            {
                throw new InvalidOperationException("user firstname / surname is required ");
            }
            
            //if (!email.Contains("@") && !email.Contains("."))
            //{
            //    throw new InvalidOperationException("user email is invalid ");
            //}
            MailAddress address = new MailAddress(email);
            var isValidEmail = (address.Address == email);
            if (!isValidEmail)
            {
                throw new InvalidOperationException("user email is invalid ");
            }

            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;

            if (age < 21)
            {
                throw new InvalidOperationException("user should be older than 21 years");
            }

            //var clientRepository = new ClientRepository();
            var client = await _clientRepository.GetByIdAsync(clientId);

            if (client == null)
            {
                throw new InvalidOperationException($"client with client id {clientId} not found.");
            }

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                Firstname = firstName,
                Surname = surname
            };

            //if (client.Name == "VeryImportantClient")
            if (client.Type == ClientType.VeryImportant)
            {
                // Skip credit check
                user.HasCreditLimit = false;
            }
            //else if (client.Name == "ImportantClient")
            else if (client.Type == ClientType.Important)
            {
                // Do credit check and double credit limit
                user.HasCreditLimit = true;
                //using (var userCreditService = new UserCreditServiceClient())
                //{
                //    var creditLimit = userCreditService.GetCreditLimit(user.Firstname, user.Surname, user.DateOfBirth);
                //    creditLimit = creditLimit * 2;
                //    user.CreditLimit = creditLimit;
                //}
                var creditLimit = await _userCreditService.GetCreditLimitAsync(user.Firstname, user.Surname, user.DateOfBirth);
                creditLimit = creditLimit * 2;
                user.CreditLimit = creditLimit;
            }
            else
            {
                // Do credit check
                user.HasCreditLimit = true;
                //using (var userCreditService = new UserCreditServiceClient())
                //{
                //    var creditLimit = userCreditService.GetCreditLimit(user.Firstname, user.Surname, user.DateOfBirth);
                //    user.CreditLimit = creditLimit;
                //}
                var creditLimit = await _userCreditService.GetCreditLimitAsync(user.Firstname, user.Surname, user.DateOfBirth);
                user.CreditLimit = creditLimit;
            }

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                throw new InvalidOperationException("insufficient credit limit");
            }

            //UserDataAccess.AddUserAsync(user);
            await _userRepository.AddUserAsync(user);

            return user;
        }
    }
}
