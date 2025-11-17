using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using TechFood.BackOffice.Application;
using TechFood.Infra;
using TechFood.Infra.Persistence.Contexts;
using TechFood.Shared.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddPresentation(builder.Configuration, new PresentationOptions
    {
        AddSwagger = true,
        AddJwtAuthentication = true,
        SwaggerTitle = "TechFood BackOffice V1",
        SwaggerDescription = "TechFood BackOffice V1"
    });

    builder.Services.AddApplication();
    builder.Services.AddInfra();

    builder.Services.AddAuthorizationBuilder()
       .AddPolicy("backoffice.read", policy => policy.RequireClaim("scope", "backoffice.read"))
       .AddPolicy("backoffice.write", policy => policy.RequireClaim("scope", "backoffice.write"));
}

var app = builder.Build();
{
    //Run migrations
    using (var scope = app.Services.CreateScope())
    {
        var dataContext = scope.ServiceProvider.GetRequiredService<BackOfficeContext>();
        dataContext.Database.Migrate();
    }

    app.UsePathBase("/api");

    app.UseForwardedHeaders();

    if (!app.Environment.IsDevelopment())
    {
        app.UseHsts();
        app.UseHttpsRedirection();
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();

        app.UseSwagger(options =>
        {
            options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0;
        });

        app.UseSwaggerUI();
    }

    app.UseInfra();

    app.UseHealthChecks("/health");

    app.UseStaticFiles(new StaticFileOptions
    {
        RequestPath = app.Configuration["TechFoodStaticImagesUrl"],
        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "images")),
    });

    app.UseRouting();

    app.UseCors();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
