
using ConsoleApp;
using Grpc.Net.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Models;
using MyNamespace;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

using var channel = GrpcChannel.ForAddress("https://localhost:7180");


var streamClient = new GrpcService.Protos.GrpcStream.GrpcStreamClient(channel);
var downStream = streamClient.FromServer(new GrpcService.Protos.Request { Text = "Hello!" });
var source = new CancellationTokenSource();
var counter = 0;
while (await downStream.ResponseStream.MoveNext(source.Token))
{
    Console.WriteLine($"{counter} {downStream.ResponseStream.Current.Text}");
    counter++;
    if (counter == 1000)
    {
        //source.Cancel();
        break;
    }
}

var upStream = streamClient.ToServer();

foreach (var letter in "Im using STREAM!!")
    await upStream.RequestStream.WriteAsync(new GrpcService.Protos.Request { Text = letter.ToString() });

await upStream.RequestStream.CompleteAsync();
var response = await upStream.ResponseAsync;

Console.WriteLine(response.Text);


var streams = streamClient.FromAndToServer();

_ = Task.Run(async () =>
{
    for (int i = 0; i < int.MaxValue; i++)
    {
        await streams.RequestStream.WriteAsync(new GrpcService.Protos.Request { Text = i.ToString() });
    }
    await streams.RequestStream.CompleteAsync();
});
_ = Task.Run(async () =>
{
    while (await streams.ResponseStream.MoveNext(CancellationToken.None))
    {
        Console.WriteLine(streams.ResponseStream.Current.Text);
    }
});








var client = new GrpcService.Protos.PeopleService.PeopleServiceClient(channel);

var people = await client.ReadAsync(new GrpcService.Protos.Void());

await client.CreateAsync(new GrpcService.Protos.Person { FirstName = "Ewa", LastName = "Ewowska" });

people = await client.ReadAsync(new GrpcService.Protos.Void());

Console.ReadLine();









static async Task WebAPI()
{
    var webApiClient = new WebApiClient("http://localhost:5145/");

    var token = await webApiClient.GetStringAsync("login?login=admin&password=admin");
    webApiClient.SetToken(token);

    var people = await webApiClient.GetAsync<IEnumerable<Models.Person>>("api/People");

    var firstName = Console.ReadLine();
    var lastName = Console.ReadLine();

    var person = new Models.Person { FirstName = firstName, LastName = lastName };

    person = await webApiClient.PostAsync<Models.Person>("api/People", person);
    people = await webApiClient.GetAsync<IEnumerable<Models.Person>>("api/People");

    Console.ReadLine();

    var httpClient = new HttpClient();
    var products = await new MyClass("http://localhost:5145", httpClient).ProductsAllAsync();


    Console.ReadLine();
}

static async Task SignalR()
{
    var webApiClient = new WebApiClient("http://localhost:5145/");

    var signalR = new HubConnectionBuilder()
        .WithUrl("http://localhost:5145/SignalR/Demo", x => x.AccessTokenProvider = () => webApiClient.GetStringAsync("login?login=admin&password=admin"))
        .WithAutomaticReconnect()
        .Build();

    signalR.On<string>("TextMessage", x => TextMessage(x));

    void TextMessage(string x)
    {
        Console.WriteLine(x);
    }


    signalR.Reconnecting += SignalR_Reconnecting;
    signalR.Reconnected += SignalR_Reconnected;

    Task SignalR_Reconnected(string? arg)
    {
        Console.WriteLine("Connected");
        return Task.CompletedTask;
    }

    Task SignalR_Reconnecting(Exception? arg)
    {
        if (arg != null)
            Console.WriteLine(arg.Message);
        Console.WriteLine("Reconnecting...");
        return Task.CompletedTask;
    }

    await signalR.StartAsync();

    await signalR.SendAsync("SayHelloToOthers", $"Hello my name is {signalR.ConnectionId}");


    var group = Console.ReadLine();

    await signalR.SendAsync("JoinGroup", group);


    var signalRpeople = new HubConnectionBuilder()
        .WithUrl("http://localhost:5145/SignalR/People")
        .Build();

    signalRpeople.On<int>("PersonDeleted", x => Console.WriteLine($"Usunięto osobę o id {x}"));

    await signalRpeople.StartAsync();

    await signalRpeople.SendAsync("AddPerson", new Models.Person { FirstName = Console.ReadLine(), LastName = Console.ReadLine() });
}