using LyticalTest.ViewModels;
using LyticalTest.ViewModels.Tenants;
using LyticalTest.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyticalTest.Interfaces
{
     public  interface IIdentity
    {
        Task<Response<AddTenantResponse>> CreteTenantAsyc(AddTenantRequest request);
        Task<Response<AddUserResponse>> RegisterAsync(AddUserRequest request);
        Task SignOutAsync(string userName);
        Task<Response<bool>> AddUserToRoleAsync(AddUserToRoleRequest request);
        Task<Response<LoginResponse>> LoginAsync(LoginRequest request);
    }
}
