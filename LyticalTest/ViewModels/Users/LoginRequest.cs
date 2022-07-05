using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyticalTest.ViewModels.Users
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse : UserModel
    {
        //public bool FirstTimeLogin { get; set; }
        public long UserId { get; set; }
        public TokenData Token { get; set; }
    }

    public class TokenData
    {
        public string Token { get; set; }
        public DateTime ExpireFrom { get; set; }
        public DateTime ExpireTo { get; set; }
        public string ExpireTimeTo { get; set; }

        
    }


}
