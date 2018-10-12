using System.Collections.Generic;
using BaseApp.Identity.ViewModels;
using Swashbuckle.AspNetCore.Examples;

namespace BaseApp.Identity.SwaggerExamples
{
    public class UserInRolesViewModelExample : IExamplesProvider
    {
        public object GetExamples()
        {

            return new UserInRolesViewModel()
            {
                 RoleNames = new List<string>(){ "user",},
                 UserName = "Pedram.gilaniniya@mail.com"
                
            };
        }

    }
}