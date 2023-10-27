using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();


app.UseWebSockets();

/*app.MapWhen(context => context.WebSockets.IsWebSocketRequest, webSocketApp =>
{
    webSocketApp.Run(async context =>
    {
        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
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
    });
});*/

//app.UseOwin(pipe => pipe(dictionary => OwinResponse));

app.MapControllers();

app.Run();




    async Task OwinResponse(IDictionary<string, object> dictionary)
{
    string resposne = "Welcome in OWIN!";

    var stream = (Stream)dictionary["owin.ResponseBody"];
    var headers = (IDictionary<string, string[]>)dictionary["owin.ResponseHeaders"];


    byte[] responseBytes = Encoding.UTF8.GetBytes(resposne);
    headers["Content-Type"] = new string[] { "text/plain" };
    headers["Content-Length"] = new string[] { responseBytes.Length.ToString() };


    await stream.WriteAsync(responseBytes, 0, responseBytes.Length - 5);
    await stream.WriteAsync(responseBytes, responseBytes.Length - 5, 5);
}