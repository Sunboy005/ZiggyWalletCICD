using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ZiggyZiggyWallet.Data.EFCore;
using ZiggyZiggyWallet.Data.Repository.Implementations;
using ZiggyZiggyWallet.Data.Repository.Interfaces;
using ZiggyZiggyWallet.Models;
using ZiggyZiggyWallet.Services.Implementations;
using ZiggyZiggyWallet.Services.Interfaces;

namespace ZiggyZiggyWallet
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //JWT Services
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                var param = new TokenValidationParameters();
                param.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SecurityKey"]));
                param.ValidateIssuer = false;
                param.ValidateAudience = false;
                option.TokenValidationParameters = param;
            });
            //Swagger Gen
            services.AddSwaggerGen(config =>
            config.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "ZiggyZiggyWallet",
                Version = "v1",
                Description = "First of All Intro"

            }));
            //DBContext
            services.AddDbContextPool<ZiggyDBContext>(
               options => options.UseSqlite(Configuration.GetConnectionString("Default"))
            );

           
            //AppRole/Sign In Defination
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                //options.Password.RequireDigit = true;
                //options.Password.RequiredLength = 8;
                //options.Password.RequireLowercase = false;
                //options.Password.RequireNonAlphanumeric = false;
                //options.Password.RequireUppercase = false;
                //options.Password.RequiredUniqueChars = 0;

                options.SignIn.RequireConfirmedEmail = true;

            }).AddEntityFrameworkStores<ZiggyDBContext>()
            .AddDefaultTokenProviders();

            //AppRepositories
             services.AddScoped<ITransactionsRepository, TransactionRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IWalletCurrencyRepository, WalletCurrencyRepository>();
            services.AddScoped<ITransactionsRepository, TransactionRepository>();

            //AppServices
            services.AddScoped<IJWTServices, JWTServices>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IWalletServices, WalletServices>();
            services.AddScoped<ICurrencyServices, CurrencyServices>();
            services.AddScoped<IWalletCurrencyServices, WalletCurrencyServices>();
            services.AddScoped<ITransactionServices, TransactionServices>();


            //Other Services
            services.AddCors();
            services.AddAutoMapper();

            //Seeder Class
            services.AddTransient<SeederClass>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SeederClass seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               

            }

            app.UseRouting();  

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

             seeder.SeedMe().Wait();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ziggy Wallet V2"));
            app.UseHttpsRedirection();
        }
    }
}
