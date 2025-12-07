using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DoctorBooking_Backend.Configurations
{
    public static class DatabaseConfig
    {
        public static IServiceCollection AddMasterDatabase(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    config.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null
                    )
                )
            );

            return services;
        }
    }
}
