using Microsoft.Extensions.Options;
using MongoDB.Driver;
using AzureIoTWeb.Models;
namespace AzureIoTWeb.Services
{
    public class MongoDbClient : IDbContext
    {
        private readonly IMongoDatabase _database = null;

        public MongoClient client { get; set; } = new MongoClient();
        public MongoDbClient(IOptions<MongoDbSettings> settings)
        {
            client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.DatabaseName);

        }

        public IMongoCollection<TelemetryData> HistoricalData() => _database.GetCollection<TelemetryData>("telemetry-data");
    }
}
