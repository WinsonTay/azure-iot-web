namespace AzureIoTWeb.Models
{
    public class TelemetryDataDto
    {
        public string Value { get; set; }
        public List<TelemetryData> HistoricalData { get; set; }
    }
}
