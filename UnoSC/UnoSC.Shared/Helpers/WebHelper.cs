using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UnoSC.Shared.Helpers
{
    class WebHelper
    {

        public static string UA = "UnoSC indev";
        public static string baseUrl = "https://www.soundcloud.com";
        public static string baseApiUrl = "https://api.soundcloud.com";

        public static string MakeRequest(string url)
        {
            string httpResponseBody = "";
            Task t = Task.Run(async () => {
                using (HttpClient httpClient = new HttpClient())
                {
                    var headers = httpClient.DefaultRequestHeaders;
                    headers.UserAgent.TryParseAdd(UA);
                    if (url.StartsWith("/")) url = baseApiUrl + url;
                    Uri requestUri = new Uri(url);
                    HttpResponseMessage httpResponse = new HttpResponseMessage();
                    try
                    {
                        httpResponse = await httpClient.GetAsync(requestUri);
                        httpResponse.EnsureSuccessStatusCode();
                        httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                    }
                    catch (Exception ex)
                    {
                        httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                    }
                }
            });
            t.Wait();
            return httpResponseBody;
        }
    }
}
