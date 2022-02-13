using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;

namespace ServiceCreditRequest.Infrastructure.Extensions
{
    public static class ConfigureCollectionExtensions
    {
        public static void UseConfigureMigration(this IMigrationRunner migrationRunner)
        {
            migrationRunner.MigrateUp();
        }
        public static void UseConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API сервиса заявки на кредит");
                c.RoutePrefix = string.Empty;
            });

        }
    }
}
