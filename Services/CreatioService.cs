using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FeatureHubPurple.Services
{

    public class CreatioService
    {
        private const string BaseUrl = "https://080925-studio.creatio.com";
        private const string AuthServiceUrl = "/ServiceModel/AuthService.svc/Login";
        private const string UserName = "APIT";
        private const string UserPassword = "Creatio123";
        private HttpClient _client;
        private string _csrfToken;  // New field for CSRF token


        public CreatioService()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("ForceUseSession", "true");
        }

        public async Task<string> TryLoginAsync()
        {
            var authData = new JObject
            {
                {"UserName", UserName},
                {"UserPassword", UserPassword}
            };

            var content = new StringContent(authData.ToString(), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(GetUrl(AuthServiceUrl), content);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = JObject.Parse(await response.Content.ReadAsStringAsync());

                if (responseJson["Code"]?.Value<int>() != 0)
                {
                    throw new UnauthorizedAccessException($"Unauthorized {UserName} for {AuthServiceUrl}");
                }

                // Save CSRF token
                if (TryGetCsrfToken(response.Headers, out var csrfToken))
                {
                    // Add CSRF token as a header for subsequent requests
                    _csrfToken = csrfToken;
                    _client.DefaultRequestHeaders.Add("BPMCSRF", _csrfToken);
                }
                else
                {
                    throw new InvalidOperationException("CSRF token not found in the response headers.");
                }

                // Fetch user information
                var userInfoUrl = "/0/ServiceModel/UserInfoService.svc/getCurrentUserInfo";
                var userInfoRequest = new HttpRequestMessage(HttpMethod.Post, GetUrl(userInfoUrl));
                userInfoRequest.Headers.Add("BPMCSRF", csrfToken); // Add CSRF token as a header

                var userInfoResponse = await _client.SendAsync(userInfoRequest);
                var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync();

                // Parse the user info JSON response
                var userInfoJson = JObject.Parse(userInfoContent);
                var userName = userInfoJson["userInfo"]["contactName"].ToString();

                return userName;
            }

            return null;
        }

        private bool TryGetCsrfToken(HttpResponseHeaders headers, out string csrfToken)
        {
            csrfToken = null;
            if (headers.TryGetValues("Set-Cookie", out var cookieValues))
            {
                var csrfCookie = cookieValues.FirstOrDefault(cookie => cookie.StartsWith("BPMCSRF="));
                if (!string.IsNullOrEmpty(csrfCookie))
                {
                    csrfToken = csrfCookie.Split(';')[0].Trim().Replace("BPMCSRF=", "");
                    return true;
                }
            }
            return false;
        }

        private string GetUrl(string endpoint)
        {
            return $"{BaseUrl}{endpoint}";
        }

        public async Task<TimeSpan> GetTotalTimeForToday()
        {
            if (!_client.DefaultRequestHeaders.Contains("BPMCSRF") && _csrfToken != null)
            {
                _client.DefaultRequestHeaders.Add("BPMCSRF", _csrfToken);
            }

            string response = await _client.GetStringAsync(GetUrl("/0/odata/UsrETMTest"));
            JObject jsonResponse = JObject.Parse(response);
            JArray records = (JArray)jsonResponse["value"];

            TimeSpan totalTime = new TimeSpan(0);
            string format = "MM/dd/yyyy HH:mm:ss";
            foreach (JObject record in records)
            {
                DateTime start = DateTime.ParseExact((string)record["UsrStart"], format, CultureInfo.InvariantCulture);
                DateTime end = DateTime.ParseExact((string)record["UsrEnd"], format, CultureInfo.InvariantCulture);
                DateTime day = DateTime.ParseExact((string)record["UsrDay"], format, CultureInfo.InvariantCulture);

                if (day.Date == DateTime.Today)
                {
                    totalTime += end - start;
                }
            }

            return totalTime;
        }

    }
}
