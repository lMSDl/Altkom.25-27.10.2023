using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace OWIN.Controllers
{
    public class WebSocketController : ControllerBase
    {
        //[HttpGet("/ws")] //HTTP 1.1
        [Route("/ws")] //HTTP 2
        public async Task Get()
        {
            if(!HttpContext.WebSockets.IsWebSocketRequest)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var response = Encoding.UTF8.GetBytes("Hello in WebSocket!");

            await webSocket.SendAsync(new ArraySegment<byte>(response, 0, response.Length), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);

            _ = Task.Run(async () =>
            {
                do
                {
                    var message = Encoding.UTF8.GetBytes("Ping");
                    await webSocket.SendAsync(new ArraySegment<byte>(message, 0, message.Length), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
                    await Task.Delay(5000);
                } while (!webSocket.CloseStatus.HasValue);
            });


            do
            {
                byte[] buffer = new byte[1];
                try
                {
                    var received = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), received.MessageType, received.EndOfMessage, CancellationToken.None);
                }
                catch
                {

                }
            } while (!webSocket.CloseStatus.HasValue);

            await webSocket.CloseAsync(webSocket.CloseStatus.Value, webSocket.CloseStatusDescription, CancellationToken.None);
        }
    }
}
