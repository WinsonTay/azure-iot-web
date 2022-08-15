namespace AzureIoTWeb.Services
{
    public class MyHostedService : BackgroundService
    {
        private readonly IIoTHub _iothubService;
        private readonly IServiceProvider _serviceProvider;
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
        public MyHostedService(IIoTHub iothub, IServiceProvider serviceProvider)
        {
            _iothubService = iothub;
            _serviceProvider = serviceProvider;
        }
       
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await _iothubService.StartListen(cancellationToken);
            //using (IServiceScope scope = _serviceProvider.CreateScope())
            //{
            //    IIoTHub scopedProcessingService =
            //        scope.ServiceProvider.GetRequiredService<IIoTHub>();
            //    await _iothubService.StartListen(cancellationToken);

            //}
            
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Stopping Service..");
            await _iothubService.Close();
            _stoppingCts.Cancel();
            
        }
    }
}
