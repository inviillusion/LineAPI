using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;

namespace MSCoreProject.LineSDK
{
    public class LineFlexMessageClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _channelAccessToken;
        public LineFlexMessageClient(string channelAccessToken)
        {
            _httpClient = new HttpClient();
            _channelAccessToken = channelAccessToken;
        }
        /// <summary>
        /// If uri is null = Default from Json file
        /// </summary>
        /// <param name="uri">uri replace to @URI</param>
        public async Task<HttpResponseMessage> PushFlexMessageAsync(string uid, string altText, string jsonPathFile, string uri = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.line.me/v2/bot/message/push");
            request.Headers.Add("Authorization", $"Bearer {_channelAccessToken}");
            string jsonString = System.IO.File.ReadAllText(jsonPathFile);
            jsonString = jsonString.Replace("@URI", uri);
            jsonString = jsonString.Replace("@ALTTEXT", altText);
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonString);
            JObject jObject = new JObject
            {
                ["type"] = "flex",
                ["altText"] = altText,
                ["contents"] = jsonObject
            };
            var flexMessage = jObject;
            var payload = new
            {
                to = uid,
                messages = new object[] { jObject }
            };
            string debug = JsonConvert.SerializeObject(payload);
            request.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            return await _httpClient.SendAsync(request);
        }
        /// <summary>
        /// If uri is null = Default from Json file
        /// </summary>
        /// <param name="uri">uri replace to @URI</param>
        public async Task<HttpResponseMessage> ReplyFlexMessageAsync(string replyToken,string altText, string jsonPathFile,Dictionary<string,string> data)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.line.me/v2/bot/message/reply");
            request.Headers.Add("Authorization", $"Bearer {_channelAccessToken}");
            string jsonString = System.IO.File.ReadAllText(jsonPathFile);
            foreach(var item in data )
            {
                jsonString = jsonString.Replace(item.Key, item.Value);
            }
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonString);
            JObject jObject = new JObject
            {
                ["type"] = "flex",
                ["altText"] = altText,
                ["contents"] = jsonObject
            };
            var flexMessage = jObject;
            var payload = new
            {
                replyToken = replyToken,
                messages = new object[] { jObject }
            };
            string debug = JsonConvert.SerializeObject(payload);
            request.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            return await _httpClient.SendAsync(request);
        }
    }
}
