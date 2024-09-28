using QYQ.Base.Common.IOCExtensions;
using QYQ.Base.Swagger.Extension;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddLog4Net();
// Add services to the container.
//builder.Services.AddRazorPages();
builder.Services.AddMemoryCache();
builder.Services.AddMultipleService("^2DSupplyStation");
builder.AddQYQSwaggerAndApiVersioning(new NSwag.OpenApiInfo()
{
    Title = "SupplyStation"
}, null, false);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseQYQSwaggerUI("SupplyStation", false);
//app.UseAuthorization();

//app.MapRazorPages();
app.MapControllers();

app.Run();
