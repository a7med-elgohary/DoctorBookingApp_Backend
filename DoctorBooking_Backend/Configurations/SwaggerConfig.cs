using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace DoctorBooking_Backend.Configurations
{
    public static class SwaggerConfig
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ERP API",
                    Version = "v1",
                    Description = "ERP System API Documentation"
                });
            });
            services.AddSwaggerGen(c =>
            {
                // ✅ تعريف ال Authorization button فقط
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter your JWT token in the text box below. Example: Bearer {your token}"
                });

                // ✅ مش هنضيف SecurityRequirement هنا علشان ميتربطش بكل endpoint
            });
        }
    }
}
