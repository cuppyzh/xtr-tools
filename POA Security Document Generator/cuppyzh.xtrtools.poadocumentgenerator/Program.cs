using cuppyzh.xtrtools.poadocumentgenerator.Utilities;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
configuration.GetSection(ApplicationSettings.GIT_SECTION_NAME).Bind(ApplicationSettings.Git);
configuration.GetSection(ApplicationSettings.DOCUMENT_SECTION_NAME).Bind(ApplicationSettings.Document);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation().ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

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
