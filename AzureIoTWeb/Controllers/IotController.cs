using Microsoft.AspNetCore.Mvc;
using AzureIoTWeb.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AzureIoTWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IotController : ControllerBase
    {   
        private readonly IIoTHub _iotRepo;
        public IotController(IIoTHub iotRepo)
        {
            _iotRepo = iotRepo;
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _iotRepo.GetHistoricalData();
            return Ok(data);
        }
    }
}
