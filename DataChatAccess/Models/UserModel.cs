namespace DataChatAccess.Models;

public class UserModel
{
	public int Id { get; set; }
	public string Nickname { get; set; }
	public string Email { get; set; }
	public DateTime CreationDate { get; set; } = DateTime.UtcNow;
	public byte[] PasswordHash { get; set; }
	public byte[] PasswordSalt { get; set; }
}