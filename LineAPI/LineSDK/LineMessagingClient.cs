using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace MSCoreProject.LineSDK
{
    public class LineMessagingClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _channelAccessToken;
        public LineMessagingClient(string channelAccessToken)
        {
            _httpClient = new HttpClient();
            _channelAccessToken = channelAccessToken;
        }
        public async Task<HttpResponseMessage> ReplyMessageAsync(string replyToken, List<Message> messages)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.line.me/v2/bot/message/reply");
            request.Headers.Add("Authorization", $"Bearer {_channelAccessToken}");
            var payload = new
            {
                replyToken = replyToken,
                messages = messages
            };
            request.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            return await _httpClient.SendAsync(request);
        }
        public async Task<HttpResponseMessage> PushMessageAsync(string to, List<Message> messages)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.line.me/v2/bot/message/push");
            request.Headers.Add("Authorization", $"Bearer {_channelAccessToken}");
            var payload = new
            {
                to = to,
                messages = messages
            };
            request.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            return await _httpClient.SendAsync(request);
        }

    }
    public class Message
    {
        [JsonProperty("type")]
        public string Type = "text";

        [JsonProperty("text")]
        public string Text { get; set; }
        
    }
    public class TextMsg : Message
    {
        public string text { get; set; }
        public TextMsg(string text)
        {
            this.text = text;
        }
    }
}
