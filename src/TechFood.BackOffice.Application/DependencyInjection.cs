using Microsoft.Extensions.DependencyInjection;
using TechFood.BackOffice.Application.Common.Services;
using TechFood.BackOffice.Application.Common.Services.Interfaces;

namespace TechFood.BackOffice.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //Services
        services.AddTransient<IEmailSender, EmailSender>();

        services.AddSingleton<IOrderNumberService, OrderNumberService>();
        services.AddTransient<IImageUrlResolver, ImageUrlResolver>();

        return services;
    }
}
