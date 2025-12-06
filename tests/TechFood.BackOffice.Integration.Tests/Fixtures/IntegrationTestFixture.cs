using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using TechFood.BackOffice.Application.Categories.Queries;
using TechFood.BackOffice.Application.Common.Services.Interfaces;
using TechFood.BackOffice.Application.Products.Queries;
using TechFood.BackOffice.Domain.Repositories;
using TechFood.BackOffice.Infra.Persistence.Contexts;
using TechFood.BackOffice.Infra.Persistence.Queries;
using TechFood.BackOffice.Infra.Persistence.Repositories;
using TechFood.Shared.Domain.UoW;
using TechFood.Shared.Infra.Extensions;
using TechFood.Shared.Infra.Persistence.UoW;

namespace TechFood.BackOffice.Integration.Tests.Fixtures;

public class IntegrationTestFixture : IDisposable
{
    public IServiceProvider ServiceProvider { get; }

    public BackOfficeContext DbContext { get; }

    public IntegrationTestFixture()
    {
        var services = new ServiceCollection();

        services.AddSingleton(Options.Create(new InfraOptions
        {
            InfraAssembly = typeof(BackOfficeContext).Assembly
        }));

        // Configure in-memory database
        services.AddDbContext<BackOfficeContext>(options =>
            options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}"));

        // Configure Unit of Work
        services.TryAddScoped<IUnitOfWorkTransaction, UnitOfWorkTransaction>();
        services.TryAddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<BackOfficeContext>());

        // Register application services
        services.AddMediatR(typeof(TechFood.BackOffice.Application.DependencyInjection).Assembly);

        // Register domain repositories
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        // Register query providers
        services.AddScoped<IProductQueryProvider, ProductQueryProvider>();
        services.AddScoped<ICategoryQueryProvider, CategoryQueryProvider>();

        // Mock external services
        var imageUrlResolverMock = new Mock<IImageUrlResolver>();
        imageUrlResolverMock
            .Setup(x => x.BuildFilePath(It.IsAny<string>(), It.IsAny<string>()))
            .Returns<string, string>((folder, filename) => $"/images/{folder}/{filename}");
        imageUrlResolverMock
            .Setup(x => x.CreateImageFileName(It.IsAny<string>(), It.IsAny<string>()))
            .Returns<string, string>((name, contentType) =>
                $"{name.Replace(" ", "-")}-{DateTime.UtcNow:yyyyMMddHHmmss}.{contentType.Replace("image/", "")}");
        services.AddScoped(_ => imageUrlResolverMock.Object);

        var imageStorageServiceMock = new Mock<IImageStorageService>();
        imageStorageServiceMock
            .Setup(x => x.SaveAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);
        services.AddScoped(_ => imageStorageServiceMock.Object);

        ServiceProvider = services.BuildServiceProvider();
        DbContext = ServiceProvider.GetRequiredService<BackOfficeContext>();
    }

    public void Dispose()
    {
        DbContext?.Database.EnsureDeleted();
        DbContext?.Dispose();

        if (ServiceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }

        GC.SuppressFinalize(this);
    }
}
