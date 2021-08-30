using CognizantChallenge.Compilers;
using CognizantChallenge.Compilers.Joodle;
using CognizantChallenge.DAL.Context;
using CognizantChallenge.DAL.Repositories;
using CognizantChallenge.DAL.Repositories.Challenges;
using CognizantChallenge.DAL.Repositories.Users;
using CognizantChallenge.DAL.UnitOfWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CognizantChallenge
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
            services.AddControllersWithViews();
            services.AddScoped<ICompiler, JdoodleCompiler>();
            services.AddScoped<IChallengesRepository, ChallengesRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            var settings = Configuration.GetSection(nameof(Settings)).Get<Settings>();
            services.AddDbContext<Context>(options =>
            {
                options.UseSqlite(settings.ConnnectionStrings.Sqlite);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}",
                    defaults: new {controller = "Home", action = "Index"});
            });
        }
    }
}