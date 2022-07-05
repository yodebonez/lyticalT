using LyticalTest.Services;
using LyticalTest.ViewModels.Tenants;
using LyticalTest.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyticalTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly OAuthService _service;

        public AuthController(OAuthService service)
        {
            _service = service;
        }




        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("createtenant")]
        public async Task<IActionResult> CreateTenantAsync(AddTenantRequest model)
        {

            var response = await _service.CreteTenantAsyc(model);
            return await Task.FromResult(new JsonResult(response));
        }



        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register(AddUserRequest model)
        {
           
            var response = await _service.RegisterAsync(model);
            return await Task.FromResult(new JsonResult(response));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequest model)
        {
           
            var response = await _service.LoginAsync(model); 
            return await Task.FromResult(new JsonResult(response));
        }

        [HttpPost]
        [Route("signOut")]
        public async Task<IActionResult> SignOutAsync(string userName)
        {
           
            await _service.SignOutAsync(userName);
            return await Task.FromResult(new JsonResult(true));
        }

        [HttpPost]
        [Route("addUserToRole")]
        public async Task<IActionResult> AddUserToRoleAsync(AddUserToRoleRequest model)
        {
         
            var response = await _service.AddUserToRoleAsync(model);
            return await Task.FromResult(new JsonResult(response));
        }


       







    }
}
