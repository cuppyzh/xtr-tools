using cuppyzh.xtrtools.poadocumentgenerator.Services;
using cuppyzh.xtrtools.poadocumentgenerator.Services.Interfaces;
using cuppyzh.xtrtools.poadocumentgenerator.Utilities;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
configuration.GetSection(ApplicationSettings.GIT_SECTION_NAME).Bind(ApplicationSettings.Git);
configuration.GetSection(ApplicationSettings.DOCUMENT_SECTION_NAME).Bind(ApplicationSettings.Document);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation().ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddLogging(loggingBuilder => {
    loggingBuilder.AddFile("logs/log_{0:yyyy}-{0:MM}-{0:dd}.log", fileLoggerOpts => {
        fileLoggerOpts.FormatLogFileName = fName => {
            return String.Format(fName, DateTime.UtcNow);
        };
    });
});

builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IDocumentServices, DocumentServices>();
builder.Services.AddScoped<IPrChangesServices, PrChangesServices>();
builder.Services.AddScoped<IApiCallServices, ApiCallServices>();

using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
    .SetMinimumLevel(LogLevel.Trace)
    .AddConsole());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
