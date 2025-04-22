using System.Net.Security;
using ApiGatewayWithTrotthleManagement.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient("Butch", client =>
{
    client.BaseAddress = new Uri("http://apigw-monitor.8elements.mobi/");
}).ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
{
    MaxConnectionsPerServer = 27
    //,
    //SslOptions = new SslClientAuthenticationOptions
    //{
    //    RemoteCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
    //}
});
builder.Services.AddSingleton<BackendThrottler>();

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
