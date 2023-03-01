using BlazorAppFörstaTest.Shared.DTOs;
using DataChatAccess;
using Microsoft.AspNetCore.SignalR;

namespace BlazorAppFörstaTest.Server.Hubs;

public class ChatHub : Hub //Ärver Hub från SignalR
{
	ChatRepository _chatRepository;
	public ChatHub(ChatRepository chatRepository)
	{
		_chatRepository = chatRepository;
	}
	
	public async Task BroadCastMessage(ChatMessageDto message) //Får med CHatMessagtDto från Shardprojektet
	{
		//Här spara meddelandet till en databas.
		await _chatRepository.AddMessage(message);
		await Clients.All.SendAsync("BroadCastMessage", message); //Client metod från Hub som skickar message objekt till alla användare
	}
}