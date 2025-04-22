using apigw_monitor.Data;
using apigw_monitor.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
// Konfigurasi Database
builder.Services.AddDbContext<RequestDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddDbContext<DelapanElementPingDBContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DelapanElementPingDBContextConnection")));
//builder.Services.AddDbContext<DelapanElementApiGWDBContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DelapanElementApiGWDBContextConnection")));
builder.Services.AddControllers();

var app = builder.Build();
//Middleware untuk menyimpan request ke database
app.Use(async (context, next) =>
{
    using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<RequestDbContext>();

var requestBody = "";
if (context.Request.ContentLength > 0)
{
    context.Request.EnableBuffering();
    using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true);
    requestBody = await reader.ReadToEndAsync();
    context.Request.Body.Position = 0;
}

var logEntry = new RequestLog
{
    Timestamp = DateTime.UtcNow,
    Method = context.Request.Method,
    Url = context.Request.Path,
    Headers = JsonSerializer.Serialize(context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString())),
    Body = requestBody
};

db.RequestLogs.Add(logEntry);
await db.SaveChangesAsync();

await next();
});

// Endpoint untuk melihat request yang sudah tersimpan
app.MapGet("/logs", async ([FromServices] RequestDbContext db) =>
{
    return Results.Json(await db.RequestLogs.OrderByDescending(r => r.Timestamp).ToListAsync());
});

//// Menjalankan migrasi database saat aplikasi pertama kali berjalan
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<RequestDbContext>();
//    db.Database.Migrate();
//}

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}
app.UseExceptionHandler("/Error");
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllers();
app.Run();
