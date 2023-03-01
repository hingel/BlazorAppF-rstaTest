using BlazorAppFörstaTest.Shared;
using DataChatAccess.Models;

namespace BlazorAppFörstaTest.Server.Services.Interfaces;

public interface IAuthService
{
	Task<ServiceResponse<int>> RegisterUserAsync(UserModel user, string password);
	Task<ServiceResponse<string>> LogInUserAsync(string email, string password);
	Task<bool> CheckUserExistsAsync(string email);
}