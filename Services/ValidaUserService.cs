using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using teste.ViewModel;

namespace teste.Services
{
    public class ValidaUserService
    {
        public static bool isValidModel(CreateUserViewModel model)
        {
            if(model.Role != "manager" && model.Role != "employee")
                return false;
            return true;
        }
    }
}