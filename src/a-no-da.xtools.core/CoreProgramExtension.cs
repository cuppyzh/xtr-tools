using a_no_da.xtools.core.Attributes;
using a_no_da.xtools.core.Services;
using a_no_da.xtools.core.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace a_no_da.xtools.core
{
    public class CoreProgramExtension
    {
        public static void AddDependencyInjection(WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile(CoreConfig.CONFIG_PATH);

            builder.Services.AddScoped<IApiCallServices, ApiCallServices>();
            builder.Services.AddScoped<XToolsExceptionAttribute>();

            builder.Services.AddLogging(loggingBuilder => {
                loggingBuilder.AddFile("logs/log_{0:yyyy}-{0:MM}-{0:dd}.log", fileLoggerOpts => {
                    fileLoggerOpts.FormatLogFileName = fName => {
                        return String.Format(fName, DateTime.UtcNow);
                    };
                });
            });

            builder.Configuration.GetSection(CoreConfig.SECTION_NAME).Bind(CoreConfig.Config);
        }
    }
}