using Microsoft.EntityFrameworkCore;
using Fortinite_Project.Web.Services;
using Fortinite_Project.Web.Workers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Configuraçao do BD
var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
builder.Services.AddDbContext<Fortinite_Project.Web.Data.AppDbContext>(
    options => options.UseSqlite(connectionString).EnableSensitiveDataLogging()

);

//Configurações da API do Fortinite
builder.Services.AddScoped<IFortniteSyncService, FortniteSyncService>();

builder.Services.AddHttpClient<IFortniteSyncService, FortniteSyncService>();

builder.Services.AddHostedService<FortniteSyncWorker>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
