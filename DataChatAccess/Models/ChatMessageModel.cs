using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataChatAccess.Models;

public class ChatMessageModel
{
    [BsonId]
    public ObjectId Id { get; set; }
    [BsonElement]
    public string SenderName { get; set; } = string.Empty;
    [BsonElement]
    public string Message { get; set; } = string.Empty;
    [BsonElement]
    public DateTime TimeSent { get; set; }

}