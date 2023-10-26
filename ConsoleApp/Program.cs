
using ConsoleApp;
using Models;
using MyNamespace;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

var webApiClient = new WebApiClient("http://localhost:5145/api/");

var people = await webApiClient.GetAsync<IEnumerable<Models.Person>>("People");

var firstName = Console.ReadLine();
var lastName = Console.ReadLine();

var person = new Models.Person { FirstName = firstName, LastName = lastName };

person = await webApiClient.PostAsync<Models.Person>("People", person);
people = await webApiClient.GetAsync<IEnumerable<Models.Person>>("People");

Console.ReadLine();

var httpClient = new HttpClient();
var products = await new MyClass("http://localhost:5145", httpClient).ProductsAllAsync();


Console.ReadLine();