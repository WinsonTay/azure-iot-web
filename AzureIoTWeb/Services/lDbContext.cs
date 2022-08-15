using AzureIoTWeb.Models;
using MongoDB.Driver;

namespace AzureIoTWeb.Services
{
    public interface IDbContext
    {
        IMongoCollection<TelemetryData> HistoricalData();
    }
}
