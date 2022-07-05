using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyticalTest.ViewModels.Tenants
{
    public class AddTenantResponse
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DateCreated { get; set; }
        public string TenantCode { get; set; }
    }
}
