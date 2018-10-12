using BaseApp.Identity.ViewModels;
using Swashbuckle.AspNetCore.Examples;
using Swashbuckle.AspNetCore.Swagger;

namespace BaseApp.Identity.SwaggerExamples
{
    public class CredentialsViewModelExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new CredentialsViewModel()
            {
              UserName = "Pedram.gilaniniya@mail.com",
                Password = "123456"

            };
            
        }

    }
}