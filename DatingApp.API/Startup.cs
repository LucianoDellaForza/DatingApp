using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API
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
            services.AddDbContext<DataContext>(x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(opt => {
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
            services.AddCors(); //service za CORS policy
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.AddAutoMapper();
            services.AddTransient<Seed>();
            services.AddScoped<IAuthRepository, AuthRepository>();  //AddScoped znaci da se service pravi once per request, npr one instance for each http request
            services.AddScoped<IDatingRepository, DatingRepository>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)  //ukljucujemo JWT autentifikaciju u servise
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Seed seeder)
        {
            if (env.IsDevelopment())    //ako smo u developer mode-u
            {
                app.UseDeveloperExceptionPage();    //bacaj developer exceptione za laksu dijagnozu u npr postmanu
            }
            else
            {
                app.UseExceptionHandler(builder => {    //ubacuje middle-mana u pipeline koji ce hvatati exceptione na globalnom nivou (ali samo ako je aplikacija u production mode-u)
                    builder.Run(async context => {  //context is related to http request/response
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;  //stavicemo da bude InternalServerError status code

                        var error = context.Features.Get<IExceptionHandlerFeature>();   //for storing error
                        if(error != null) 
                        {
                            context.Response.AddApplicationError(error.Error.Message);  //napravili smo static klasu za ovo
                            await context.Response.WriteAsync(error.Error.Message); //upisujemo i error message u http response
                        }
                    });
                });  
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            // seeder.SeedUsers(); - for seeding data in db
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); //za CORS policy (pre MVC-a)
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
