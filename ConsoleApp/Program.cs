
using ConsoleApp;
using Models;
using MyNamespace;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

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