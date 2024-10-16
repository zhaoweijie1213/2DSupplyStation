using _2DSupplyStation.AuthenticationExtend;
using _2DSupplyStation.Models;
using _2DSupplyStation.Services;
using QYQ.Base.Common.IOCExtensions;
using QYQ.Base.Swagger.Extension;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//builder.Logging.AddLog4Net();
// Add services to the container.
//builder.Services.AddRazorPages();
builder.Services.AddMemoryCache();
builder.Services.AddHostedService<HiddenFolderRenamer>();
builder.Services.Configure<List<MenuConfig>>(builder.Configuration.GetSection("Menus"));
builder.Services.AddMultipleService("^2DSupplyStation");
builder.AddQYQSwaggerAndApiVersioning(new NSwag.OpenApiInfo()
{
    Title = "SupplyStation"
}, null, false);

builder.Services.AddSerilog(configureLogger =>
{
    configureLogger.Enrich.WithMachineName()
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(builder.Configuration);
});

//��������
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder =>
        {
            builder.AllowAnyMethod()
            .AllowAnyOrigin()
            .AllowAnyHeader();
        });
});

builder.Services.AddAuthorization();
#region UrlToken
builder.Services.AddAuthentication(options =>
{
    options.AddScheme<UrlTokenAuthenticationHandler>(UrlTokenAuthenticationDefaults.AuthenticationScheme, "CustomUrlTokenScheme");
    options.DefaultAuthenticateScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignOutScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSerilogRequestLogging();
//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("CorsPolicy");
app.UseRouting();
app.UseQYQSwaggerUI("SupplyStation", false);
app.UseAuthentication();
app.UseAuthorization();

//app.MapRazorPages();
app.MapControllers();

app.Run();
