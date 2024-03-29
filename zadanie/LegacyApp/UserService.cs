using System;

namespace LegacyApp
{
    public class UserService
    {
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {

            if (!doesNameExist(firstName, lastName)) return false;

            if (!emailValidation(email)) return false;

            if (!ageValidation(dateOfBirth)) return false;
            

            var client = ClientCreatrion(firstName, lastName, email, dateOfBirth, clientId, out var user);

            ClientImportanceAndCreditLimitSet(client, user);

            if (!DoesUserHasCreditLimit(user)) return false;

            UserDataAccess.AddUser(user);
            return true;
            
        }

        private Client ClientCreatrion(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId,
            out User user)
        {
            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);

            user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };
            return client;
        }

        private bool DoesUserHasCreditLimit(User user)
        {
            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            return true;
        }


        private void ClientImportanceAndCreditLimitSet(Client client, User user)
        {
            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (client.Type == "ImportantClient")
            {
                using (var userCreditService = new UserCreditService())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    creditLimit = creditLimit * 2;
                    user.CreditLimit = creditLimit;
                }
            }
            else
            {
                user.HasCreditLimit = true;
                using (var userCreditService = new UserCreditService())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    user.CreditLimit = creditLimit;
                }
            }
        }

        private bool doesNameExist(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return false;
            }
            return true;
        }

        private bool emailValidation(String email)
        {
            if (!email.Contains("@") && !email.Contains("."))
            {
                return false;
            }

            return true;
        }

        private bool ageValidation(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;

            if (age < 21)
            {
                return false;
            }

            return true;
        }

    }
}