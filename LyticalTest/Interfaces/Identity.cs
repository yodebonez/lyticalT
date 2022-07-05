using AutoMapper;
using LyticalTest.Extensions;
using LyticalTest.Models;
using LyticalTest.ViewModels;
using LyticalTest.ViewModels.Tenants;
using LyticalTest.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyticalTest.Interfaces
{
    public class Identity : IIdentity
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly Database _db;
        SignInManager<ApplicationUser> _signInManager;
        private RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly IDateTime _dateTime;
        private readonly IMapper _mapper;

        public Identity(IMapper mapper ,Database db
            , SignInManager<ApplicationUser> signInManager
            , UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager, IConfiguration config, IDateTime dateTime)
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _dateTime = dateTime;
            _mapper = mapper;

        }



        private JwtSecurityToken GenerateJSONWebToken(ApplicationUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: _dateTime.Now.AddMinutes(int.Parse(_config["Jwt:ExpireIn"])),
              signingCredentials: credentials);

            return token;
            
        }


        private string RetrieveErrors(IdentityResult result)
        {
            var error = "";

            //return result.Errors.SelectMany(x => x.Description);
            foreach (var item in result.Errors)
            {
                error += item.Description + "<br />";
            }

            return error;
        }


        public async Task<Response<bool>> ValidateRegisterAsync(AddUserRequest model)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user == null)
                return await Task.FromResult(new Response<bool>
                {
                    Message = "Request Successful",
                    Success = true,
                    Data = true
                });

            if (await _db.Users.AnyAsync(x => !x.IsDeleted && x.UserName == model.UserName))
                return await Task.FromResult(new Response<bool>
                {
                    Message = "Request Successful",
                    Success = true,
                    Data = true
                });

            return await Task.FromResult(new Response<bool>
            {
                Message = $"Declined  user with email {model.Email} already exist",
            });
        }




        private async Task<ApplicationUser> GetUserAsync(GetUserRequest model)
        {
            ApplicationUser user = null;

            if (!string.IsNullOrEmpty(model.Email))
            {
                user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                    return await Task.FromResult(user);
            }

           
            if (user == null)
                return await Task.FromResult(user);

         
            return user;
        }



        private async Task<Response<bool>> AddUserRoleAsync(string userName, string roleName)
        {
            var user = await GetUserAsync(new GetUserRequest { Email = userName });

            if (user == null)
            {
                return new Response<bool>
                {
                    Message = "User does not exist"
                };
            }

            var roles = _roleManager.Roles;

            IdentityResult addToRolesResult;
            if (await _roleManager.RoleExistsAsync(roleName.Trim()))
            {
                addToRolesResult = await _userManager.AddToRoleAsync(user, roleName);

                if (!addToRolesResult.Succeeded)
                {
                    return new Response<bool> { Message = addToRolesResult.Errors.IdentityErrorsToList() };
                }
            }
            else
            {
                var creareRoleResult = await _roleManager.CreateAsync(new ApplicationRole { Name = roleName });
                if (creareRoleResult.Succeeded)
                    addToRolesResult = await _userManager.AddToRoleAsync(user, roleName);

            }

            return new Response<bool> { Success = true, Data = true };
        }



        private async Task<UserModel> BuildUserModelAsync(ApplicationUser user)
        {
            try
            {
              
                var userRoleNames = await (from roles in _db.Roles
                                     join userRole in _db.UserRoles
                                     on roles.Id equals userRole.RoleId
                                     where userRole.UserId == user.Id
                                     select roles.Name
                              ).ToListAsync();

                var userModel = new UserModel
                {

                TenantId  = user.TenantId,
                FirstName  = user.FirstName,
                LastName   = user.LastName,
                MiddleName  = user.MiddleName,
                Gender  = user.Gender,
                 UserCode  = user.UserCode,
                 Roles  = userRoleNames,
                 Email = user.Email,


                };

                return userModel;
            }
            catch (Exception ex)
            {
                //Log.Error(ex, ex.Message);
                throw;
            }
        }



        public async Task<Response<bool>> AddUserToRoleAsync(AddUserToRoleRequest request)
        {
            var user = await _db.Users.AnyAsync(x => !x.IsDeleted && x.Email == request.Email);

            if (await _db.Users.AnyAsync(x => !x.IsDeleted && x.Email == request.Email) == false)
                return await Task.FromResult(new Response<bool>
                {
                    Message = $"Decline - User does not exist."
                });

            return await AddUserRoleAsync(request.Email, request.Role);
        }

        public async Task<Response<AddTenantResponse>> CreteTenantAsyc(AddTenantRequest request)
        {
            try
            {
                

                var ExistingTenant = await _db.Tenants.FirstOrDefaultAsync(x => x.Email == request.Email);
                if (ExistingTenant != null)
                    return await Task.FromResult(new Response<AddTenantResponse>
                    {
                        Message = "Declined - Tenant Existed already",
                    });


                Random rx = new Random();
                int rand = rx.Next(1000, 2000);
                string tenantCode = rand.ToString();

                Tenant tenant = new Tenant
                    {
                        Name = request.Name,
                        Email = request.Email,
                        TennantCode = tenantCode
                    };
                AddTenantResponse response = new AddTenantResponse
                {
                    Name = request.Name,
                    Email = request.Email,
                    DateCreated = DateTime.Now,
                    TenantCode = tenantCode
                    
                };

                await _db.Tenants.AddAsync(tenant);
                await _db.SaveChangesAsync();

                return await Task.FromResult(new Response<AddTenantResponse>
                {
                    Success = true,
                    Message = " Request Successful",
                    Data = response
                });


            }
            catch(Exception ex)
            {
                // log the error
                throw new Exception();
            }
        }

        public async Task<Response<AddUserResponse>> RegisterAsync(AddUserRequest request)
        {
            string myplainCode = "";
            try
            {
                var validateResponse = await ValidateRegisterAsync(request);

                if (!validateResponse.Success)
                    return await Task.FromResult(new Response<AddUserResponse>
                    {
                        Message = validateResponse.Message
                    });

                var tenantId = await _db.Tenants.FirstOrDefaultAsync(x => x.Id == request.TenantId);

                if (tenantId == null)
                    return await Task.FromResult(new Response<AddUserResponse>
                    {
                        Message = "Declined - Tenant is not available",
                    });



                Random rx = new Random();
                int rand = rx.Next(1000, 2000);
                myplainCode = rand.ToString();

                var user = new ApplicationUser()
                {
                    TenantId = request.TenantId,
                    UserName = !string.IsNullOrEmpty(request.UserName) ? request.UserName : request.Email,
                    Email = request.Email,  
                    FirstName = request.FirstName,
                    MiddleName = request.MiddleName,
                    LastName = request.LastName,
                    Gender = request.Gender,
                    CreatedOn = _dateTime.Now,
                    UserCode = myplainCode,             
                };

              
                var addUserResult = await _userManager.CreateAsync(
                     user, request.Password);

                if (!addUserResult.Succeeded)
                    return await Task.FromResult(new Response<AddUserResponse>
                    {
                        Message = RetrieveErrors(addUserResult)
                    });
        
                await _db.SaveChangesAsync();

                if (addUserResult.Succeeded)
                {
                    var addUserToRoleResult = await AddUserRoleAsync(user.Email, request.RoleName);
                    var response = _mapper.Map<AddUserResponse>(await BuildUserModelAsync(user));
                   
                    return await Task.FromResult(new Response<AddUserResponse>
                    {
                        Success = true,
                        Message = "User created sucessfully",
                        Data = response

                    });
                }
                return await Task.FromResult(new Response<AddUserResponse>
                {
                    Message = addUserResult.Errors.IdentityErrorsToList()
                });
            }

            catch (Exception ex)
            {
           
                return await Task.FromResult(new Response<AddUserResponse>
                {
                    Message = ex.Message
                });
            }
        }

        public async Task SignOutAsync(string userName)
        {
            try
            {
                await _signInManager.SignOutAsync();
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<Response<LoginResponse>> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                    return await Task.FromResult(new Response<LoginResponse>
                    {
                        Message = "Declined - User does not exist."
                    });

                    // I ignore email confirmation           
                var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
     


                if (result.Succeeded)
                {
                    var responseModel = _mapper.Map<LoginResponse>(await BuildUserModelAsync(user));
                   
                    user.LastLogOn = _dateTime.Now;

                    await _db.SaveChangesAsync();

                    var token = GenerateJSONWebToken(user);

                    responseModel.UserId = user.Id;
                    responseModel.Token = new TokenData
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        ExpireFrom = _dateTime.Now,
                        ExpireTo = token.ValidTo,
                        ExpireTimeTo = $"{token.ValidTo.ToString("HH:mm:ss")}",
                    
                    };

                    
                    return await Task.FromResult(new Response<LoginResponse>
                    {
                        Success = true,
                        Data = responseModel
                    });
                }

                return await Task.FromResult(new Response<LoginResponse>
                {
                    Message = $"Login failed. Invalid Email/Password.",
                });
            }

            catch (Exception ex)
            {
               // Log.Error(ex, ex.Message);
                return await Task.FromResult(new Response<LoginResponse>
                {
                    Message = ex.Message,
                });
            }


           
        }
    }
}
