using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using VacationRental.Service.Interfaces;
using VacationRental.Service.Mapper;
using VacationRental.Service.Services;
using VacationRental.Data.EFContext;
using VacationRental.Common.Models;
using FluentValidation;

namespace VacationRental.Api
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
            services.AddMvc();

            services.AddSwaggerGen(opts => opts.SwaggerDoc("v1", new OpenApiInfo{ Title = "Vacation Rental Information", Version = "v1" }));

            services.AddScoped<IBookingService, BookingsService>();
            services.AddScoped<ICalendarService, CalendarService>();
            services.AddScoped<IRentalService, RentalService>();
            services.AddSingleton<IValidator<BookingBindingModel>, PostBookingValidator>();
            services.AddDbContext<VacationRentalDbContext>(options => options.UseInMemoryDatabase(databaseName: "Lodgify"));
            services.AddAutoMapper(typeof(MappingProfile));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = new GlobalExceptionMiddleware().Invoke
            });
            app.UseSwagger();
            app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/swagger/v1/swagger.json", "VacationRental v1"));
            
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
