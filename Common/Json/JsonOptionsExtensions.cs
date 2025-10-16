using System.Text.Json.Serialization;

namespace Common.Json
{
    public static class JsonOptionsExtensions
    {
        public static IServiceCollection AddJsonEnumStringConverter(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // Convert Enum -> string (ví dụ: "Manager" thay vì 2)
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            return services;
        }
    }
}
