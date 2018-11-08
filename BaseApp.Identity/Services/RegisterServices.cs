using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BaseApp.Identity.Auth;
using BaseApp.Identity.AutoMapper;
using BaseApp.Identity.Helpers;
using BaseApp.Identity.Model;
using BaseApp.Identity.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Examples;
using Swashbuckle.AspNetCore.Swagger;

namespace BaseApp.Identity.Services
{
    public static class RegisterServices
    {
        
        /// <summary>
        /// Register All Services and configurations
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        public static void RegisterAllServices(IServiceCollection services,IConfiguration Configuration)
        {
            
            // Register Database Services
            RegisterDbServices(services, Configuration);
           
            // Register Authentication and Authorization Services
            RegisterAuthenticationServices(services, Configuration);
            
            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);      
            
            // Swagger Configuration 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "My API", Version = "v1" });      
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "Please enter JWT with Bearer into field", Name = "Authorization", Type = "apiKey" });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                    { "Bearer", Enumerable.Empty<string>() }    
                });
                // Enable Swagger examples
                c.OperationFilter<ExamplesOperationFilter>();
                // Enable swagger descriptions
                c.OperationFilter<DescriptionOperationFilter>();
                // Enable swagger response headers
                c.OperationFilter<AddResponseHeadersFilter>();
                // Add (Auth) to action summary
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
            });

            // Register Other Services
            services.AddSingleton<IAuthorizationHandler, ActionAuthorizationHadler>();
            // services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserService, UserService>();
        }

        
        /// <summary>
        /// Register Database Service and configurations
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private static void RegisterDbServices(IServiceCollection services, IConfiguration configuration)
        {
            // DataBase Configuration
            var connectionString = configuration["ConnectionStrings:DefaultConnection"] ?? throw new ArgumentNullException(nameof(services));
            services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(connectionString));
            services.AddScoped<IJwtFactory, JwtFactory>();
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    // Password settings.
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;

                    // Lockout settings.
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;

                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
                
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

        }

        
        /// <summary>
        /// Register Authentication services and configurations
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        private static void RegisterAuthenticationServices(IServiceCollection services,IConfiguration Configuration)
        {
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            // Add Configs
            var config = new AppSettingConfigs();
            Configuration.Bind("AppSettingConfigs", config);      
            services.AddSingleton(config);     
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.SecretKey));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });
                     
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = false,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
               
            };
        
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
                configureOptions.RequireHttpsMetadata = false;

            });        
            
            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess));
            });
           
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Bearer", policy => policy.RequireAuthenticatedUser());
            });
            
            // Register LinkedIn Authentication Service
            services.AddAuthentication()
                .AddOAuth("LinkedIn",
                    c =>
                    {
                        c.ClientId = config.Authentication.LinkedIn.ClientId;
                        c.ClientSecret = config.Authentication.LinkedIn.ClientSecret;
                        c.Scope.Add("r_basicprofile");
                        c.Scope.Add("r_emailaddress");
                        c.CallbackPath = "/signin-linkedin";
                        c.AuthorizationEndpoint = "https://www.linkedin.com/oauth/v2/authorization";
                        c.TokenEndpoint = "https://www.linkedin.com/oauth/v2/accessToken";
                        c.UserInformationEndpoint =
                            "https://api.linkedin.com/v1/people/~:(id,formatted-name,email-address,picture-url)";
                        c.Events = new OAuthEvents
                        {
                            OnCreatingTicket = async context =>
                            {
                                var request = new HttpRequestMessage(HttpMethod.Get,
                                    context.Options.UserInformationEndpoint);
                                request.Headers.Authorization =
                                    new AuthenticationHeaderValue("Bearer", context.AccessToken);
                                request.Headers.Add("x-li-format", "json");

                                var response =
                                    await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
                                response.EnsureSuccessStatusCode();
                                var user = JObject.Parse(await response.Content.ReadAsStringAsync());

                                var userId = user.Value<string>("id");
                                if (!string.IsNullOrEmpty(userId))
                                {
                                    context.Identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId,
                                        ClaimValueTypes.String, context.Options.ClaimsIssuer));
                                }

                                var formattedName = user.Value<string>("formattedName");
                                if (!string.IsNullOrEmpty(formattedName))
                                {
                                    context.Identity.AddClaim(new Claim(ClaimTypes.Name, formattedName,
                                        ClaimValueTypes.String, context.Options.ClaimsIssuer));
                                }

                                var email = user.Value<string>("emailAddress");
                                if (!string.IsNullOrEmpty(email))
                                {
                                    context.Identity.AddClaim(new Claim(ClaimTypes.Email, email, ClaimValueTypes.String,
                                        context.Options.ClaimsIssuer));
                                }

                                var pictureUrl = user.Value<string>("pictureUrl");
                                if (!string.IsNullOrEmpty(pictureUrl))
                                {
                                    context.Identity.AddClaim(new Claim("profile-picture", pictureUrl,
                                        ClaimValueTypes.String,
                                        context.Options.ClaimsIssuer));
                                }
                            }
                        };

                    });
        }
    }
}