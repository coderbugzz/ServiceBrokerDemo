namespace DemoAPI.Hub
{
    using Microsoft.AspNetCore.SignalR;
    public class ApiHub : Hub
    {
        public async Task SendDataToClients(string message)
        {
            if (Clients != null)
            {
                await Clients.All.SendAsync("ReceiveData", message);
            }
        }
    }
}
