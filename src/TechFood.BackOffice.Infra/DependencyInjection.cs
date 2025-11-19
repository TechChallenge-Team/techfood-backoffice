
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Amazon.DynamoDBv2;
using Amazon.Extensions.NETCore.Setup;
using TechFood.BackOffice.Application;
using TechFood.BackOffice.Application.Categories.Queries;
using TechFood.BackOffice.Application.Common.Services.Interfaces;
using TechFood.BackOffice.Application.Customers.Queries;
using TechFood.BackOffice.Application.Menu.Queries;
using TechFood.BackOffice.Application.Products.Queries;
using TechFood.BackOffice.Domain.Repositories;
using TechFood.Infra.Persistence.Contexts;
using TechFood.Infra.Persistence.DynamoDB;
using TechFood.Infra.Persistence.ImageStorage;
using TechFood.Infra.Persistence.Queries;
using TechFood.Infra.Persistence.Repositories;
using TechFood.Shared.Infra.Extensions;

namespace TechFood.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services)
    {
        services.AddSharedInfra<BackOfficeContext>(new InfraOptions
        {
            DbContext = (serviceProvider, dbOptions) =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                dbOptions.UseSqlServer(config.GetConnectionString("DataBaseConection"));
            },
            ApplicationAssembly = typeof(BackOffice.Application.DependencyInjection).Assembly
        });

        // Configure DynamoDB with specific settings
        services.AddSingleton<IAmazonDynamoDB>(serviceProvider =>
        {
            var config = serviceProvider.GetRequiredService<IConfiguration>();
            var awsSection = config.GetSection("AWS");
            var dynamoSection = config.GetSection("DynamoDB");
            
            var awsConfig = new AmazonDynamoDBConfig
            {
                RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(awsSection.GetValue<string>("Region") ?? "us-east-1")
            };

            // Check if we should use local DynamoDB configuration
            var useLocalCredentials = dynamoSection.GetValue<bool>("UseLocalCredentials");
            var serviceUrl = dynamoSection.GetValue<string>("ServiceURL");
            
            if (useLocalCredentials && !string.IsNullOrEmpty(serviceUrl))
            {
                awsConfig.ServiceURL = serviceUrl;
                awsConfig.UseHttp = true;
                
                // Use DynamoDB-specific credentials for local development
                var accessKey = dynamoSection.GetValue<string>("AccessKey") ?? "dummy";
                var secretKey = dynamoSection.GetValue<string>("SecretKey") ?? "dummy";
                
                return new AmazonDynamoDBClient(accessKey, secretKey, awsConfig);
            }

            // For production, use default AWS credentials (IAM roles, environment variables, etc.)
            return new AmazonDynamoDBClient(awsConfig);
        });


        //Data
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        //Queries
        services.AddScoped<IProductQueryProvider, ProductQueryProvider>();
        services.AddScoped<ICategoryQueryProvider, CategoryQueryProvider>();
        services.AddScoped<IMenuQueryProvider, MenuQueryProvider>();

        services.AddScoped<IImageStorageService, LocalDiskImageStorageService>();

        return services;
    }
    
    public static async Task InitializeDynamoDbAsync(IServiceProvider serviceProvider)
    {
        var dynamoDb = serviceProvider.GetRequiredService<IAmazonDynamoDB>();
        await DynamoDbInitializer.InitializeTablesAsync(dynamoDb);
    }
}
