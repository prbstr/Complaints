using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Complaints.Core.Complaint;
using Complaints.Core.User;
using Complaints.Data.Contexts;
using Complaints.Data.DataModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Complaints.Api
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authenticationConfiguration = Configuration.GetSection("Authentication");
            var databaseConfiguration = Configuration.GetSection("ConnectionStrings");
            services.Configure<Authentication>(authenticationConfiguration);
            services.Configure<ConnectionStrings>(databaseConfiguration);

            var authSettings = authenticationConfiguration.Get<Authentication>();
            ConfigureAuthentication(services, authSettings);

            services.AddDbContext<ComplaintsContext>(options => options.UseInMemoryDatabase(databaseName: "ComplaintsDb"));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IComplaintService, ComplaintService>();
            services.AddControllers();
            services.AddMvc(options => options.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
