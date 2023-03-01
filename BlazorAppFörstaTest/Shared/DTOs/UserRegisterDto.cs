using System.ComponentModel.DataAnnotations;

namespace BlazorAppFörstaTest.Shared.DTOs;

public class UserRegisterDto
{
	[Required, StringLength(10, MinimumLength = 3)]
	public string Nickname { get; set; }

	[Required, EmailAddress]
	public string Email { get; set; }

	[Required, StringLength(30, MinimumLength = 8)]
	public string Password { get; set; }

	[Compare("Password")] //Sla jämföra med Password propertyn
	public string ConfirmPassword { get; set; }

}