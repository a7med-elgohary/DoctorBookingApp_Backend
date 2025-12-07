using Microsoft.Extensions.DependencyInjection;
//using AutoMapper;

namespace DoctorBooking_Backend.Configurations
{
    public static class MapperConfig
    {
        public static IServiceCollection AddMapperConfig(this IServiceCollection services)
        {
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}
