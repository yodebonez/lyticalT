using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyticalTest.ViewModels.Users
{
    public class AddUserToRoleRequest
    {

        public string Email { get; set; }
        public string Role { get; set; }
    }

    public class AddUserToRoleResponse
    {

    }
}
