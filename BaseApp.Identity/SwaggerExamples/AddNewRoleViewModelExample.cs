﻿using System;
using System.Collections.Generic;
using BaseApp.Identity.ViewModels;
using Swashbuckle.AspNetCore.Examples;

namespace BaseApp.Identity.SwaggerExamples
{
    public class AddNewRoleViewModelExample : IExamplesProvider
    {
        public Object GetExamples()
        {
            return new AddNewRoleViewModel()
            {
                RoleName = "Admin",
                Actions = new List<Actions>()
                {
                    new Actions(){
                    ActionName = "God",
                    ControllerName = "God"
                    }
                }
                
            };

        }

    }
}