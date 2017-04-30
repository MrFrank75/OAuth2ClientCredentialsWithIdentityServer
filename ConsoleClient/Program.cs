using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string tokenResult = GetHttpResponseAsync(new Uri("http://localhost:50151/connect/token"), "ClientIdThatCanOnlyRead", "scope.readaccess", "secret1")
                .GetAwaiter()
                .GetResult();

            Console.WriteLine("Token acquired from Authorization Server:");
            Console.WriteLine(tokenResult);
            Console.ReadKey();
        }

        private static async Task<string> GetHttpResponseAsync(Uri uriAuthorizationServer, string clientId, string scope, string clientSecret)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = uriAuthorizationServer;
            client.DefaultRequestHeaders
                .Accept
                .Clear();
            client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpContent httpContent = new FormUrlEncodedContent(
                new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),   
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("scope", scope),
                    new KeyValuePair<string, string>("client_secret", clientSecret)
                });

            HttpResponseMessage postResponse = await client.PostAsync(uriAuthorizationServer, httpContent);

            return await postResponse.Content.ReadAsStringAsync();
        }

    }
}
