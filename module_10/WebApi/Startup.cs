using BusinessLayer;
using BusinessLayer.MessageSenders;
using BusinessLayer.ModelServices.Contracts;
using BusinessLayer.ModelServices.Implementations;
using DataLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi
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
            services.AddControllers();
            services.AddTransient<DbContextOptions>(sp => new DbContextOptionsBuilder().UseSqlServer("Data Source=DESKTOP-3TJHR5I\\SQLEXPRESS;Password=admin;User ID=admin;Initial Catalog=Education").Options);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddLogging(log => log.AddProvider(new DebugLoggerProvider()));
            services.AddSingleton<ILogger>(sp => sp.GetService<ILoggerFactory>().CreateLogger("DLog"));
            services.AddSingleton<IDataAccess, DataAccess>();
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<IHomeworksService, HomeworksService>();
            services.AddScoped<ILectionsService, LectionsService>();
            services.AddScoped<ILectorsService, LectorsService>();
            services.AddScoped<IStudentsService, StudentsService>();
            services.AddScoped<IEmailSender, EmailSender>(sp => new EmailSender(Models.Domain.Email.TryCreate("test@example.com"), "examplepass", "smtp.example.ru"));
            services.AddScoped<ISMSSender, SMSSender>(sp => new SMSSender(Models.Domain.PhoneNumber.TryCreate("+71111111111")));
            services.AddScoped<AttendanceAnalyzer>();
            services.AddScoped<HomeworksAnalyzer>();
            services.AddScoped<LectionReportGenerator>();
            services.AddScoped<StudentReportGenerator>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CourseApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseHandleException();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CourseApi v1"));
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
