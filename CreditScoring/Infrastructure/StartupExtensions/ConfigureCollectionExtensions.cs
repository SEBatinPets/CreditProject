using Microsoft.AspNetCore.Builder;

namespace CreditScoring.Infrastructure.StartupExtensions
{
    internal static class ConfigureCollectionExtensions
    {
        public static void UseConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API сервиса менеджера сбора метрик");
            });
        }
    }
}
