namespace BlazorAppFörstaTest.Shared.DTOs;

public class ChatMessageDto
{
	public string Name { get; set; } = string.Empty;
	public string Message { get; set; } = string.Empty;

	public DateTime TimeStamp { get; set; }
}