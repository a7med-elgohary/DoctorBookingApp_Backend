using Domain.DTOs;
using DoctorBooking_Backend.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DoctorBooking_Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1️⃣ Controllers
            builder.Services.AddControllers();

            // 2️⃣ HttpContext
            builder.Services.AddHttpContextAccessor();

            // 3️⃣ Database Config (من ملف مستقل)
            builder.Services.AddMasterDatabase(builder.Configuration);

            // 4️⃣ AutoMapper Config (من ملف مستقل)
            builder.Services.AddMapperConfig(); // << الأفضل من AddAutoMapper(typeof(Program))

            // 5️⃣ App Services (DI) - ملف مستقل لتنظيم كل الـ AddScoped
            builder.Services.AddServicesConfig();

            // 6️⃣ Auth & JWT Config (لو عندك ملف خاص بيه)
            builder.Services.AddJwtAuthentication(builder.Configuration);

            // 7️⃣ Swagger
            builder.Services.AddSwaggerConfiguration();
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
            builder.Services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<JwtSettings>>().Value);


            // 8️⃣ Build
            var app = builder.Build();

            // 9️⃣ Middlewares
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Doctor Booking API V1");
                    c.RoutePrefix = "swaggerxcxcx";
                });

                // Redirect default route to swagger
                app.MapGet("/", context =>
                {
                    context.Response.Redirect("/swagger");
                    return Task.CompletedTask;
                });
            }



            app.UseHttpsRedirection();

            app.UseAuthentication(); // مهم قبل Authorization
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
