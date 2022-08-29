// See https://aka.ms/new-console-template for more information

using ClientApp;

HttpClient client = new HttpClient();

client.DefaultRequestHeaders.Clear();
client.BaseAddress = new Uri("http://localhost:9050"); //gateway

// 1. without access_token will not access the service
//    and return 401 .
var resWithoutToken = client.GetAsync("/customers").Result;

Console.WriteLine($"Sending Request to /customers , without token.");
Console.WriteLine($"Result : {resWithoutToken.StatusCode}");

//2. with access_token will access the service
//   and return result.
client.DefaultRequestHeaders.Clear();
Console.WriteLine("\nBegin Auth....");
var jwt = JwtSchemal.GetJwt();
Console.WriteLine("End Auth....");
Console.WriteLine($"\nToken={jwt}");

client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");
var resWithToken = client.GetAsync("/customers").Result;

Console.WriteLine($"\nSend Request to /customers , with token.");
Console.WriteLine($"Result : {resWithToken.StatusCode}");
Console.WriteLine(resWithToken.Content.ReadAsStringAsync().Result);

//3. visit no auth service 
Console.WriteLine("\nNo Auth Service Here ");
client.DefaultRequestHeaders.Clear();
var res = client.GetAsync("/customers/1").Result;

Console.WriteLine($"Send Request to /customers/1");
Console.WriteLine($"Result : {res.StatusCode}");
Console.WriteLine(res.Content.ReadAsStringAsync().Result);


//4. with access_token will access the Auth server
//   and return result.
HttpClient client1 = new HttpClient();

client1.DefaultRequestHeaders.Clear();
client1.BaseAddress = new Uri("http://localhost:9059"); //AuthServer

client1.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");
var authServerResWithToken = client1.GetAsync("/api/demo").Result;

Console.WriteLine($"\nSend Request to /demo , with token.");
Console.WriteLine($"Result : {authServerResWithToken.StatusCode}");
Console.WriteLine(authServerResWithToken.Content.ReadAsStringAsync().Result);

Console.Read();


