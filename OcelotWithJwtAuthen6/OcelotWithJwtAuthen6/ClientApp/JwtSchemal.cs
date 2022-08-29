using Newtonsoft.Json;

namespace ClientApp;

public class JwtSchemal
{
    public static string GetJwt()
    {
        var client = new HttpClient();

        client.BaseAddress = new Uri("http://localhost:9059"); //AuthServer
        client.DefaultRequestHeaders.Clear();

        var res2 = client.GetAsync("/api/auth?name=catcher&pwd=123").Result;

        dynamic jwt = JsonConvert.DeserializeObject(res2.Content.ReadAsStringAsync().Result);

        return jwt.access_token;
    }
}