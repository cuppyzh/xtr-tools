using a_no_da.xtools.core.Services.Interfaces;
using a_no_da.xtools.core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using a_no_da.xtools.modules.poadocument.Services.Interfaces;
using a_no_da.xtools.modules.poadocument.Services;
using a_no_da.xtools.core;
using Microsoft.Extensions.Configuration;

namespace a_no_da.xtools.modules.poadocument
{
    public class PoaDocumentProgramExtension
    {
        public static void AddDependencyInjection(WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile(ModuleConfig.CONFIG_PATH);

            builder.Services.AddScoped<IDocumentServices, DocumentServices>();
            builder.Services.AddScoped<IPRChangesServices, PRChangesServices>();

            builder.Configuration.GetSection(ModuleConfig.SECTION_NAME).Bind(ModuleConfig.Config);
        }
    }
}