using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FeatureHubPurple.Services
{
    public class CreatioService
    {
        private const string AuthServiceUrl = "https://080925-studio.creatio.com/ServiceModel/AuthService.svc/Login";
        private const string ODataUrl = "https://080925-studio.creatio.com/0/odata";
        private const string UserName = "APIT";
        private const string UserPassword = "Creatio123";

        private CookieContainer _authCookie;

        public void TryLogin()
        {
            var authData = @"{
                ""UserName"":""" + UserName + @""",
                ""UserPassword"":""" + UserPassword + @"""
            }";
            var request = CreateRequest(AuthServiceUrl, authData);
            _authCookie = new CookieContainer();
            request.CookieContainer = _authCookie;

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        var responseMessage = reader.ReadToEnd();
                        Console.WriteLine(responseMessage);
                        if (responseMessage.Contains("\"Code\":1"))
                        {
                            throw new UnauthorizedAccessException($"Unauthorized {UserName} for {AuthServiceUrl}");
                        }
                    }
                    string authName = ".ASPXAUTH";
                    string authCookeValue = response.Cookies[authName].Value;
                    _authCookie.Add(new Uri(AuthServiceUrl), new Cookie(authName, authCookeValue));
                }
            }
        }

        private HttpWebRequest CreateRequest(string url, string data)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("ForceUseSession", "true");
            var bytes = Encoding.UTF8.GetBytes(data);
            request.ContentLength = bytes.Length;

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }
            return request;
        }
    }
}
