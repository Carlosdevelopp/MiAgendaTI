using DataAccess;
using DataAccess.Contract;
using Infrastructure.Contract;
using Infrastructure.Implementation;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IMiAgendaInfrastructure, MiAgendaInfrastructure>();
builder.Services.AddScoped<IMiAgendaDataAccess, MiAgendaDataAccess>();

var connectionStrings = builder.Configuration.GetConnectionString("AGENDA_DB_CONNECTION");
builder.Services.AddScoped<SqlConnection>(provider => new SqlConnection(connectionStrings));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Usuarios/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Agenda}/{action=Index}/{id?}");

app.Run();
