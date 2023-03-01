using BlazorAppF�rstaTest.Server.Hubs;
using BlazorAppF�rstaTest.Server.Services;
using BlazorAppF�rstaTest.Server.Services.Interfaces;
using BlazorAppF�rstaTest.Shared.DTOs;
using DataChatAccess;
using DataChatAccess.Contexts;
using DataChatAccess.Models;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using BlazorAppF�rstaTest.Server.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Denna kan vi ta bort till en b�rjan
//builder.Services.AddControllersWithViews(); //
builder.Services.AddRazorPages();


builder.Services.AddDbContext<UserContext>(options =>
{
	var connectionString = builder.Configuration.GetConnectionString("UserDb");
	options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ChatRepository>();


builder.Services.AddSignalR(); //L�gga till detta f�r kontakt f�r SignalR f�r Blazor Server. Websocket, socket. Variant.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging(); //Kan g�ra brytpunkter i frontend delen.
}
else
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection(); //Vid anropning av HTTP g�r det till HTTPS p� server sidan

app.UseBlazorFrameworkFiles(); //N�r n�gon anropar server utan path, skickas Blazor apopen tillbaks till clienten
app.UseStaticFiles(); //F�r att kunna hosta sidor/filer i client. Mappar upp filerna.

app.UseRouting(); //Navigationmanager som �r inbyggd i Blazo

app.MapHub<ChatHub>("/hubs/chat"); //


app.MapGet("/allMessages", async (ChatRepository repo) => await repo.GetAllMessages());

//Detta �r flyttat till WebApplicationsExtensions

//app.MapPost("/user/register", async (IAuthService authService, UserRegisterDto dto) =>
//{
//	var user = new UserModel() { Email = dto.Email, Nickname = dto.Nickname};
//	return await authService.RegisterUserAsync(user, dto.Password);
//});

//app.MapPost("/user/login", async (IAuthService authService, UserLoginDto dto) =>
//{
//	return await authService.LogInUserAsync(dto.Email, dto.Password);
//});

app.MapAuthEndpoints();

app.MapRazorPages(); //Kan visa error sidan
//app.MapControllers(); //Mappa endpoints
app.MapFallbackToFile("index.html"); //Indexsidan

app.Run();
