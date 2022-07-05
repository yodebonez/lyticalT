using LyticalTest.Interfaces;
using LyticalTest.Models;
using LyticalTest.ViewModels;
using LyticalTest.ViewModels.Tenants;
using LyticalTest.ViewModels.Users;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyticalTest.Services
{
    public class OAuthService
    {

        private readonly Database _db;
        private readonly IConfiguration _config;
        private readonly IIdentity _iIdentity;

        public OAuthService(Database db, IConfiguration config, IIdentity identity)
        {
            _db = db;
            _config = config;
            _iIdentity = identity;
        }



        public async Task<Response<LoginResponse>> LoginAsync(LoginRequest request)
        {
            return await _iIdentity.LoginAsync(request);
        }

        public async Task<Response<AddUserResponse>>
           RegisterAsync(AddUserRequest request)
        {
            return await _iIdentity.RegisterAsync(request);
        }

        public async Task SignOutAsync(string userName)
        {
            await _iIdentity.SignOutAsync(userName);
        }

        public async Task<Response<bool>> AddUserToRoleAsync(AddUserToRoleRequest model)
        {
            return await _iIdentity.AddUserToRoleAsync(model);
        }

       
        public async Task<Response<AddTenantResponse>> CreteTenantAsyc(AddTenantRequest request)
        {
            return await _iIdentity.CreteTenantAsyc(request);
        }





    }
}
