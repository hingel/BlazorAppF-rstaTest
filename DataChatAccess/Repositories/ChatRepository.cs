using BlazorAppFörstaTest.Shared.DTOs;
using DataChatAccess.Models;
using MongoDB.Driver;

namespace DataChatAccess
{
    public class ChatRepository
	{
		private readonly IMongoCollection<ChatMessageModel> _chatMessagesMongoCollection;

		public ChatRepository()
		{
			var host = "localhost";
			var databaseName = "ChatDb";
			var port = "27017";
			var connectionString = $"mongodb://{host}:{port}";
			var client = new MongoClient(connectionString);
			var database = client.GetDatabase(databaseName);
			_chatMessagesMongoCollection = database.GetCollection<ChatMessageModel>("ChatMessages",
				new MongoCollectionSettings() { AssignIdOnInsert = true });
		}

		public async Task AddMessage(ChatMessageDto chatMessagedto)
		{
			
			await _chatMessagesMongoCollection.InsertOneAsync(ConvertChatMessage(chatMessagedto));
		}

		public async Task<ChatMessageDto[]> GetAllMessages()
		{
			var filter = Builders<ChatMessageModel>.Filter.Empty;
			var all = await _chatMessagesMongoCollection.FindAsync(filter);

			return all.ToList().Select(ConvertToDto).ToArray();
		}

		private ChatMessageModel ConvertChatMessage (ChatMessageDto chatMessageDto)
		{
			return new ChatMessageModel()
			{
				SenderName = chatMessageDto.Name,
				Message = chatMessageDto.Message,
				TimeSent = chatMessageDto.TimeStamp
			};
		}

		private ChatMessageDto ConvertToDto(ChatMessageModel cmm)
		{
			return new ChatMessageDto()
			{
				Name = cmm.SenderName,
				Message = cmm.Message,
				TimeStamp = cmm.TimeSent
			};
		}

	}
}