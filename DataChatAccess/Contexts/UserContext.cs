using DataChatAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataChatAccess.Contexts;

public class UserContext : DbContext
{
	public DbSet<UserModel> UserModels { get; set; }

	public UserContext(DbContextOptions options) : base(options)
	{
		
	}

}