using AutoMapper;
using LyticalTest.Interfaces;
using LyticalTest.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyticalTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private readonly string _policyName = "CorsPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(opt =>
            {
                opt.AddPolicy(name: _policyName, builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });



            services.AddAuthentication(
          o =>
          {
              o.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
              o.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
              o.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
              o.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
          })
          .AddCookie(IdentityConstants.ApplicationScheme,
          o =>
          {
               //o.Cookie.Expiration = TimeSpan.FromHours(8);
               o.Cookie.SameSite = SameSiteMode.None;
              o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
              o.AccessDeniedPath = new PathString("/");
              o.ExpireTimeSpan = TimeSpan.FromHours(8);
              o.LoginPath = new PathString("/account/login");
              o.LogoutPath = new PathString("/account/logout");
              o.SlidingExpiration = true;
          })
          ;

            //jwt token generation
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = Configuration["Jwt:Issuer"],
               ValidAudience = Configuration["Jwt:Issuer"],
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
           };
       });

            services.AddIdentityCore<ApplicationUser>(OptionsBuilderConfigurationExtensions => { });
            new IdentityBuilder(typeof(ApplicationUser), typeof(ApplicationRole), services)
                .AddRoleManager<RoleManager<ApplicationRole>>()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddEntityFrameworkStores<Database>()
                .AddDefaultTokenProviders();

            services.Configure<SecurityStampValidatorOptions>(
            o =>
            {
                o.ValidationInterval = TimeSpan.FromMinutes(1);
            });


            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            //database conn
            var conn = Configuration["ConnectionStrings:identityConnection"];
            services.AddDbContext<Database>(options => options.UseSqlServer(conn));



            services.AddControllers();
            services.AddScoped<IIdentity, Identity>();
            services.AddScoped(typeof(Services.OAuthService));
            services.AddTransient<IDateTime, CustomDateTime>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LyticalTest", Version = "v1" });
            });

            //Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                
                mc.AddProfile(new ClientMappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LyticalTest v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
