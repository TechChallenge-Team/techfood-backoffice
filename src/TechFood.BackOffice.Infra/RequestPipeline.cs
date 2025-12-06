using Microsoft.AspNetCore.Builder;

namespace TechFood.BackOffice.Infra
{
    public static class RequestPipeline
    {
        public static IApplicationBuilder UseInfra(this IApplicationBuilder app)
        {
            app.UseSharedInfra();

            return app;
        }
    }
}
