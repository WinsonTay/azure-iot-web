using System.Threading.Tasks;
using System;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Security.Authentication;
using Microsoft.Extensions.Options;
using AzureIoTWeb.Models;
namespace AzureIoTWeb.Services
{
    public class IoTHubService : IIoTHub
    {
        private EventHubConsumerClient consumer;
        private readonly IMongoCollection<TelemetryData> _historicalData;

        public string Value { get; set; } = "0";

        public IoTHubService(IOptions<EventHubSettings> settings , IDbContext dbContext)
        {
            consumer = new EventHubConsumerClient(EventHubConsumerClient.DefaultConsumerGroupName, settings.Value.ConnectionString, settings.Value.Name);
            _historicalData = dbContext.HistoricalData();
        }

        public async Task Close()
        {
            await consumer.CloseAsync();
        }
        public async Task StartListen(CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine("Start Listening");

                await foreach (PartitionEvent partitionEvent in consumer.ReadEventsAsync(
                    startReadingAtEarliestEvent: false,
                    cancellationToken: cancellationToken))
                {
                    string readFromPartition = partitionEvent.Partition.PartitionId;
                    byte[] eventBodyBytes = partitionEvent.Data.EventBody.ToArray();

                    TelemetryData eventData = partitionEvent.Data.EventBody.ToObjectFromJson<TelemetryData>();
                    var telemetryData = eventData.Content[0].Data[0];
                    Console.WriteLine($"Publish Time: {eventData.PublishTimestamp}");
                    Console.WriteLine($"Telemetry Data: {telemetryData.Values[0].Value}");

                    Value = telemetryData.Values[0].Value;

                    await _historicalData.InsertOneAsync(eventData);
                    //var data = await telemetryCollection.Find(t => true).ToListAsync();
                   

                    //Console.WriteLine(data.Content[0].Data[0].Values[0].DisplayName);
                    //Console.WriteLine(data.Content[0].Data[0].Values[0].Value);
                    //foreach (var data in telemetryData.Values)
                    //{
                    //    Console.WriteLine($"Address: {data.Address}");
                    //    Console.WriteLine($"Name: {data.DisplayName}");
                    //    Console.WriteLine($"Value:{ data.Value}");
                    //}


                }
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Task Canceled");
                // This is expected if the cancellation token is
                // signaled.
            }
            finally

            {
                await consumer.CloseAsync();
            }
        }
        public async Task<TelemetryDataDto> GetHistoricalData()
        {
            var data = await _historicalData.Find(h => true).ToListAsync();
            data = data.Select(x => new TelemetryData()
            {
                _id = x._id,
                Content = x.Content,
                PublishTimestamp = DateTime.Parse(x.PublishTimestamp).ToLocalTime().ToString()
            }).TakeLast(25).OrderByDescending(x => DateTime.Parse(x.PublishTimestamp)).ToList();
            //return data.TakeLast(25).ToList();
            return new TelemetryDataDto()
            {

                Value = Value,
                HistoricalData = data
            };
        }
    }
}
