using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
namespace AzureIoTWeb.Models
{
    public class TelemetryData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string PublishTimestamp { get; set; }
        public List<Content> Content { get; set; }
    }

    public class Content
    {
        public string HwId { get; set; }
        public List<Data> Data { get; set; }
    }
    public class Data
    {
        public string CorrelationId { get; set; }
        public string SourceTimestamp { get; set; }
        public List<Values> Values { get; set; }


    }
    public class Values
    {
        public string DisplayName { get; set; }
        public string Address { get; set; }
        public string Value { get; set; }
    }
}
