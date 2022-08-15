using AzureIoTWeb.Models;

namespace AzureIoTWeb.Services
{
    public interface IIoTHub
    {
        Task Close();
        Task<TelemetryDataDto> GetHistoricalData();
        Task StartListen(CancellationToken cancellationToken);
    }
}
