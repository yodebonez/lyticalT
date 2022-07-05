using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyticalTest.Models
{
    public class ApplicationUser : IdentityUser<long>
    {

        public ApplicationUser()
        {
            IsDeleted = false;
            CreatedOn = DateTime.Now;
            LastLogOn = DateTime.Now;
        }

        public int TenantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Gender { get; set; }
        public string UserCode { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? LastLogOn { get; set; }
      

    }
}
