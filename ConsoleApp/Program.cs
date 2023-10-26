
using ConsoleApp;
using Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

var webApiClient = new WebApiClient("http://localhost:5145/api/");

var people = await webApiClient.GetAsync<IEnumerable<Person>>("People");

var firstName = Console.ReadLine();
var lastName = Console.ReadLine();

var person = new Person { FirstName = firstName, LastName = lastName };

person = await webApiClient.PostAsync<Person>("People", person);
people = await webApiClient.GetAsync<IEnumerable<Person>>("People");

Console.ReadLine();