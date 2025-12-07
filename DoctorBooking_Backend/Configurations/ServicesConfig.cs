

using Presentation.Serveces;

namespace DoctorBooking_Backend.Configurations
{
    public static class ServicesConfig
    {
        public static IServiceCollection AddServicesConfig(this IServiceCollection services)
        {
            // ✅ Services
            services.AddScoped<AuthService>();
            //services.AddScoped<IEmployeeService, EmployeeService>();
            // services.AddScoped<INotificationService, NotificationService>();




            // ✅ Repositories
            //services.AddScoped<IApplicationUserRepo, ApplicationUserRepo>();
            //services.AddScoped<IEmployeeRepo, EmployeeRepo>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
