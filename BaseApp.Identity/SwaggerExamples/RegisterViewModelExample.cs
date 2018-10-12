using System;
using BaseApp.Identity.ViewModels;
using Swashbuckle.AspNetCore.Examples;

namespace BaseApp.Identity.SwaggerExamples
{
    public class RegisterViewModelExample : IExamplesProvider
    {
        public object GetExamples()
        {

            var randomNumber = new Random().Next(int.MaxValue);
            return new RegisterViewModel()
            {
                FirstName = "Test" + randomNumber.ToString(),
                LastName = "LastNameTest" + randomNumber.ToString(),
                Email =
                    "Test" + randomNumber.ToString() + "." + "LastNameTest" + randomNumber.ToString() + "@gmail.com",
                Password = "123456",
                Gender = Gender.Male,
                Location = "Tehran"

            };

        }
    }
}