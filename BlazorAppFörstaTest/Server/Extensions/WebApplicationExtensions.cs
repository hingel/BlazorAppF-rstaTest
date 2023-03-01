using BlazorAppFörstaTest.Server.Services.Interfaces;
using BlazorAppFörstaTest.Shared.DTOs;
using DataChatAccess;
using DataChatAccess.Models;

namespace BlazorAppFörstaTest.Server.Extensions;

public static class WebApplicationExtensions
{
	public static WebApplication MapAuthEndpoints(this WebApplication app)
	{
		app.MapPost("/user/register", RegisterHandler);
		app.MapPost("/user/login", LoginHandler);
		return app;
	}

	private static async Task<IResult> RegisterHandler(IAuthService authService, UserRegisterDto dto)
	{
		var user = new UserModel() { Email = dto.Email, Nickname = dto.Nickname };
		var response =  await authService.RegisterUserAsync(user, dto.Password);

		return response.Success ? Results.Ok(response) : Results.BadRequest(response);
	}

	private static async Task<IResult> LoginHandler(IAuthService authService, UserLoginDto dto)
	{
		var response = await authService.LogInUserAsync(dto.Email, dto.Password);

		return response.Success ? Results.Ok(response) : Results.BadRequest(response);
	}
}