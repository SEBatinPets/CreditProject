using AutoMapper;
using CreditScoring.Data.Repository.Implementation;
using CreditScoring.Data.Repository.Interfaces;
using CreditScoring.Data.SendResult;
using CreditScoring.Data.SendResult.Implementation;
using CreditScoring.Data.SendResult.Interfaces;
using CreditScoring.Domain.Managers.Implementation;
using CreditScoring.Domain.Managers.Interfaces;
using CreditScoring.Infrastructure.Mapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace CreditScoring.Infrastructure.StartupExtensions
{
    internal static class ServiceCollectionExtensions
    {
        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IScoringRepository, ScoringRepository>();
        }
        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);
        }
        public static void ConfigureHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<ISendScoringResult, SendScoringResult>();
        }
        public static void ConfigureManagers(this IServiceCollection services)
        {
            services.AddSingleton<ICreditScoringManager, CreditScoringManager>();
        }
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API (заглушка) сервиса оценки возможности выдачи кредита",
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
    }
}
