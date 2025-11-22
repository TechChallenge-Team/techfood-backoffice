
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechFood.BackOffice.Application.Categories.Queries;
using TechFood.BackOffice.Application.Common.Services.Interfaces;
using TechFood.BackOffice.Application.Menu.Queries;
using TechFood.BackOffice.Application.Products.Queries;
using TechFood.BackOffice.Domain.Repositories;
using TechFood.BackOffice.Infra.Persistence.Contexts;
using TechFood.BackOffice.Infra.Persistence.ImageStorage;
using TechFood.BackOffice.Infra.Persistence.Queries;
using TechFood.BackOffice.Infra.Persistence.Repositories;
using TechFood.Shared.Infra.Extensions;

namespace TechFood.BackOffice.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services)
    {

        services.AddSharedInfra<BackOfficeContext>(new InfraOptions
        {
            DbContext = (serviceProvider, dbOptions) =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                var mongoSection = config.GetSection("MongoDB");

                var connectionString = mongoSection.GetValue<string>("ConnectionString") ?? "mongodb://localhost:27017";
                var databaseName = mongoSection.GetValue<string>("DatabaseName") ?? "techfood";
                dbOptions.UseMongoDB(connectionString, databaseName);
            },
            ApplicationAssembly = typeof(BackOffice.Application.DependencyInjection).Assembly
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
}
