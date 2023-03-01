using System.Net.Http.Json;
using BlazorAppFörstaTest.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorAppFörstaTest.Client.Pages;

partial class Chat : ComponentBase
{
	public ChatMessageDto CurrentMessage { get; set; } = new();
	public List<ChatMessageDto> Messages { get; set; }

	private HubConnection _chatHub;

	protected override async Task OnInitializedAsync() //Kan göra samma saker här i som i en konstruktor
	{
		Messages = new List<ChatMessageDto>();

		_chatHub = new HubConnectionBuilder().WithUrl(NavigationManager.BaseUri + "hubs/chat").Build(); //Gör inställningar innan instansen skapas för var den ska connecta mot och lyssna på servern

		_chatHub.On<ChatMessageDto>("BroadCastMessage", (newMessage) =>
		{
			Messages.Add(newMessage);
			StateHasChanged(); //För att sidan ska veta att den ska hämta från Servern.
		});

		var respons = await HttpClient.GetFromJsonAsync<ChatMessageDto[]>(HttpClient.BaseAddress + "allMessages");

		if (respons != null)
		{
			Messages.AddRange(respons);
		}

		await _chatHub.StartAsync();
		await base.OnInitializedAsync();
	}

	private async Task SendMessage()
	{
		CurrentMessage.TimeStamp = DateTime.UtcNow; //Bättre att spara som UTC för att spara tiden.
		//Messages.Add(CurrentMessage); //Ta bort då meddelandet kommer från hubben istället
		await _chatHub.SendAsync("BroadCastMessage", CurrentMessage);
		CurrentMessage.Message = string.Empty;
	}


}