using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Buffers.Text;

namespace BlazorAppFörstaTest.Client;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
	private ISessionStorageService _sessionStorage;
	private readonly HttpClient _httpClient;

	public CustomAuthenticationStateProvider(ISessionStorageService sessionStorage, HttpClient httpClient)
	{
		_sessionStorage = sessionStorage;
		_httpClient = httpClient;
	}


	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		var authToken = _sessionStorage.GetItem<string>("authToken"); //Hämtar den fil/del som heter authToken
		var identity = new ClaimsIdentity();
		_httpClient.DefaultRequestHeaders.Authorization = null;

		if (!string.IsNullOrEmpty(authToken))
		{
			try
			{
				identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken), "jwt");
				_httpClient.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Bearer", authToken.Replace("\"", ""));
			}
			catch
			{
				_sessionStorage.RemoveItem("authToken");
				identity = new ClaimsIdentity();
			}

		}

		var user = new ClaimsPrincipal(identity);
		var state = new AuthenticationState(user);

		NotifyAuthenticationStateChanged(Task.FromResult(state));

		return state;

	}

	private IEnumerable<Claim>? ParseClaimsFromJwt(string authToken)
	{
		var payload = authToken.Split('.')[1];
		var jsonBytes = Parse64WithoutPadding(payload);
		var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, Object>>(jsonBytes);
		var claims = keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
		return claims;
	}

	private byte[] Parse64WithoutPadding(string payload)
	{
		switch (payload.Length / 4)
		{
			case 2:
				payload += "==";
				break;
			case 3:
				payload += "=";
				break;
		}

		return Convert.FromBase64String(payload);
	}
}
