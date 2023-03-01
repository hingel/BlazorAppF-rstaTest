using BlazorAppFörstaTest.Server.Hubs;
using BlazorAppFörstaTest.Server.Services;
using BlazorAppFörstaTest.Server.Services.Interfaces;
using BlazorAppFörstaTest.Shared.DTOs;
using DataChatAccess;
using DataChatAccess.Contexts;
using DataChatAccess.Models;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using BlazorAppFörstaTest.Server.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Denna kan vi ta bort till en början
//builder.Services.AddControllersWithViews(); //
builder.Services.AddRazorPages();


builder.Services.AddDbContext<UserContext>(options =>
{
	var connectionString = builder.Configuration.GetConnectionString("UserDb");
	options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ChatRepository>();


builder.Services.AddSignalR(); //Lägga till detta för kontakt för SignalR för Blazor Server. Websocket, socket. Variant.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging(); //Kan göra brytpunkter i frontend delen.
}
else
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection(); //Vid anropning av HTTP går det till HTTPS på server sidan

app.UseBlazorFrameworkFiles(); //När någon anropar server utan path, skickas Blazor apopen tillbaks till clienten
app.UseStaticFiles(); //För att kunna hosta sidor/filer i client. Mappar upp filerna.

app.UseRouting(); //Navigationmanager som är inbyggd i Blazo

app.MapHub<ChatHub>("/hubs/chat"); //


app.MapGet("/allMessages", async (ChatRepository repo) => await repo.GetAllMessages());

//Detta är flyttat till WebApplicationsExtensions

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
