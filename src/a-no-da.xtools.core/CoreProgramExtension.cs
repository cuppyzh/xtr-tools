using a_no_da.xtools.core.Services;
using a_no_da.xtools.core.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace a_no_da.xtools.core
{
    public class CoreProgramExtension
    {
        public static void AddDependencyInjection(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IApiCallServices, ApiCallServices>();
        }
    }
}