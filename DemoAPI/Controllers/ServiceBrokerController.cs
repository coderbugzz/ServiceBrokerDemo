using DemoAPI.Hub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceBrokerController : ControllerBase
    {
        private readonly IHubContext<ApiHub> _hubContext;

        public ServiceBrokerController(IHubContext<ApiHub> hubContext)
        {
            _hubContext = hubContext;
        }
        // GET: api/<ServiceBrokerController>
        [HttpGet]
        public async Task<IActionResult> InvokeSendDataToClient(string message)
        {
            try
            {
                // Invoke the SendDataToClients method on the connected clients
                await _hubContext.Clients.All.SendAsync("ReceiveData", message);

                return Ok("Data sent to clients successfully.");
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

      
    }
}
