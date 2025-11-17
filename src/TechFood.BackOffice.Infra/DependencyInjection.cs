
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechFood.BackOffice.Application;
using TechFood.BackOffice.Application.Categories.Queries;
using TechFood.BackOffice.Application.Common.Services.Interfaces;
using TechFood.BackOffice.Application.Customers.Queries;
using TechFood.BackOffice.Application.Menu.Queries;
using TechFood.BackOffice.Application.Products.Queries;
using TechFood.BackOffice.Domain.Repositories;
using TechFood.Infra.Persistence.Contexts;
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


        //Data
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        //Queries
        services.AddScoped<IProductQueryProvider, ProductQueryProvider>();
        services.AddScoped<ICategoryQueryProvider, CategoryQueryProvider>();
        services.AddScoped<ICustomerQueryProvider, CustomerQueryProvider>();
        services.AddScoped<IMenuQueryProvider, MenuQueryProvider>();

        services.AddScoped<IImageStorageService, LocalDiskImageStorageService>();

        return services;
    }
}
