using AutoMapper;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using ServiceCreditRequest.Client;
using ServiceCreditRequest.Data.Repositories.Implementation;
using ServiceCreditRequest.Data.Repositories.Interfaces;
using ServiceCreditRequest.Domain.Managers.Implementation;
using ServiceCreditRequest.Domain.Managers.Interfaces;
using ServiceCreditRequest.Infrastructure.Mapper;
using ServiceCreditRequest.Jobs;
using ServiceCreditRequest.Jobs.Factory;
using ServiceCreditRequest.Jobs.HostedService;
using ServiceCreditRequest.Jobs.Schedule;
using System;

namespace ServiceCreditRequest.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureDbConnection(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSqlServer()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(Startup).Assembly).For.Migrations()
                ).AddLogging(lb => lb
                    .AddFluentMigratorConsole());
        }
        public static void ConfigureQuartz(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            services.AddSingleton<CreditRequestJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(CreditRequestJob),
                cronExpression: $"{configuration["Quartz:cronExpression"]}")); // Запускать каждые 5 секунд
                //cronExpression: "0/5 * * * * ?"));

            services.AddHostedService<QuartzHostedService>();
        }
        public static void ConfigureMapper(this IServiceCollection services)
        {
            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);
        }
        public static void ConfigureRepository(this IServiceCollection services)
        {
            services.AddSingleton<ICreditRequestRepository, CreditRequestRepository>();
            services.AddSingleton<ICreditContractRepository, CreditContractRepository>();
            services.AddSingleton<ICreditApplicantRepository, CreditApplicantRepository>();
        }
        public static void ConfigureManagers(this IServiceCollection services)
        {
            services.AddSingleton<ICreditRequestManager, CreditRequestManager>();
        }
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API сервиса заявки на кредит",
                    TermsOfService = new Uri("https://github.com/sebatinpets"),
                    Contact = new OpenApiContact
                    {
                        Name = "SEBatin",
                        Email = "sebatin1995@gmail.com",
                        Url = new Uri("https://github.com/sebatinpets"),
                    }
                });
            });
        }
        public static void ConfigureHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<ICreditRequestClient, CreditRequestClient>();
            //.AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(1000)));
        }
    }
}
