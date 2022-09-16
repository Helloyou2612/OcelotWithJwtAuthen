// See https://aka.ms/new-console-template for more information

using ClientApp;

HttpClient client = new HttpClient();

client.DefaultRequestHeaders.Clear();
client.BaseAddress = new Uri("http://localhost:9050"); //gateway

Console.WriteLine("\n>>>>>>>>>>>>>>>>>GATEWAY<<<<<<<<<<<<<<<<<");
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


Console.WriteLine("\n>>>>>>>>>>>>>>>>>SERVICE<<<<<<<<<<<<<<<<<");
HttpClient client2 = new HttpClient();

client2.DefaultRequestHeaders.Clear();
client2.BaseAddress = new Uri("http://localhost:9051"); //service
// 1. without access_token will not access the service
//    and return 401 .
var resServiceWithoutToken = client2.GetAsync("/api/customers").Result;

Console.WriteLine($"Sending Request to /api/customers , without token.");
Console.WriteLine($"Result : {resServiceWithoutToken.StatusCode}");

//2. with access_token will access the service
//   and return result.
client2.DefaultRequestHeaders.Clear();
client2.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");
var resServiceWithToken = client2.GetAsync("/api/customers").Result;

Console.WriteLine($"\nSend Request to /api/customers , with token.");
Console.WriteLine($"Result : {resServiceWithToken.StatusCode}");
Console.WriteLine(resServiceWithToken.Content.ReadAsStringAsync().Result);

//3. visit no auth service 
Console.WriteLine("\nNo Auth Service Here ");
client2.DefaultRequestHeaders.Clear();
var resService = client2.GetAsync("/api/customers/1").Result;

Console.WriteLine($"Send Request to /api/customers/1");
Console.WriteLine($"Result : {resService.StatusCode}");
Console.WriteLine(resService.Content.ReadAsStringAsync().Result);

Console.WriteLine("\n>>>>>>>>>>>>>>>>>AUTH CENTER<<<<<<<<<<<<<<<<<");
//4. without access_token will not access the Auth server
//    and return 401 
HttpClient client1 = new HttpClient();

client1.DefaultRequestHeaders.Clear();
client1.BaseAddress = new Uri("http://localhost:9059"); //AuthServer

var authServerResWithToken = client1.GetAsync("/api/demo").Result;

Console.WriteLine($"\nSend Request to /demo , without token.");
Console.WriteLine($"Result : {authServerResWithToken.StatusCode}");
Console.WriteLine(authServerResWithToken.Content.ReadAsStringAsync().Result);

//5. with access_token will access the Auth server
//   and return result.
client.DefaultRequestHeaders.Clear();
client1.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");
var resAuthWithToken = client1.GetAsync("/api/demo").Result;

Console.WriteLine($"\nSend Request to /demo , with token.");
Console.WriteLine($"Result : {resAuthWithToken.StatusCode}");
Console.WriteLine(resAuthWithToken.Content.ReadAsStringAsync().Result);

//6. visit no auth service in Auth server
Console.WriteLine("\nNo Auth Service Here ");
client1.DefaultRequestHeaders.Clear();
var resAuth = client1.GetAsync("/api/demo/1").Result;

Console.WriteLine($"Send Request to /demo/1");
Console.WriteLine($"Result : {resAuth.StatusCode}");
Console.WriteLine(resAuth.Content.ReadAsStringAsync().Result);

Console.Read();


