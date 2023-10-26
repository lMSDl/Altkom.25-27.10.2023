
using Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
httpClient.BaseAddress = new Uri("http://localhost:5145/api/");

var response = await httpClient.GetAsync("People");

/*if(response.StatusCode != System.Net.HttpStatusCode.OK)
{
    return;
}*/

/*if(!response.IsSuccessStatusCode)
{
    return;
}*/

response.EnsureSuccessStatusCode();

var people = await response.Content.ReadFromJsonAsync<IEnumerable<Person>>();


var firstName = Console.ReadLine();
var lastName = Console.ReadLine();

var person = new Person { FirstName = firstName, LastName = lastName };


/*using (var stringContent = new StringContent(JsonSerializer.Serialize(person), MediaTypeHeaderValue.Parse("application/json"))) {
    response = await httpClient.PostAsync("People", stringContent);
}*/
response = await httpClient.PostAsJsonAsync("People", person);
response.EnsureSuccessStatusCode();


response = await httpClient.GetAsync("People");
people = await response.Content.ReadFromJsonAsync<IEnumerable<Person>>();

Console.ReadLine();