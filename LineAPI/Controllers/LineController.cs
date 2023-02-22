using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSCoreProject.LineSDK;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace LineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public string Token { get; set; }
        public string Hosting { get; set; }
        public HttpResponseMessage responseMessage { get; set; }
        public LineController(IConfiguration configuration)
        {
            _configuration = configuration;
            Token = _configuration.GetValue<string>("LineSettings:ChannelAccessToken");
            Hosting = _configuration.GetValue<string>("LineSettings:Hosting");
        }
        [HttpPost("ReplyMsg")]
        public async Task<IActionResult> ReplyMessage([FromBody] JsonObject databody)
        {
            string app_path = "";
            List<Message> replymsg = new List<Message>();
            Dictionary<string,string> data = new Dictionary<string,string>();
            var msgClient = new LineMessagingClient(Token);
            var flexClient = new LineFlexMessageClient(Token);

            ReceivedMessage receivedMessage = JsonConvert.DeserializeObject<ReceivedMessage>(databody.ToString());
            if (receivedMessage.Events.Count == 0)
                return Ok();

            var lineEvent = receivedMessage.Events[0];
            string replyToken = lineEvent.ReplyToken;
            string uid = lineEvent.Source.UserId;

            switch (lineEvent.Type.ToLower())
            {
                case "message":
                    switch (lineEvent.Message.Type.ToLower())
                    {
                        case "text":
                            switch (lineEvent.Message.Text.ToLower())
                            {
                                case "{testflex}":
                                    var flexDemo = _configuration.GetValue<String>("JsonFileName:FlexDemo");
                                    app_path = AppDomain.CurrentDomain.BaseDirectory + "wwwroot\\Json\\" + flexDemo;
                                    data = new Dictionary<string, string> {
                                        { "@URI",Hosting}
                                    };
                                    responseMessage = await flexClient.ReplyFlexMessageAsync(replyToken, "FlexDemo", app_path, data);
                                    break;
                                case "{testflexinfo}":
                                    var flexInfo = _configuration.GetSection("JsonFileName:FlexInfo");
                                    app_path = AppDomain.CurrentDomain.BaseDirectory + "wwwroot\\Json\\" + flexInfo;
                                    data = new Dictionary<string, string> {
                                            { "@dept",""},
                                            { "@name","" },
                                            { "@startwork",""},
                                            { "@vamax",""},
                                            { "@va",""}
                                         };
                                    responseMessage = await flexClient.ReplyFlexMessageAsync(replyToken, "FlexInfo", app_path, data);
                                    break;
                                case "{my uid}":
                                    replymsg.Add(new TextMsg("This is your UID : "));
                                    replymsg.Add(new TextMsg(lineEvent.Source.UserId));
                                    responseMessage = await msgClient.ReplyMessageAsync(replyToken, replymsg);
                                    break;
                                default:
                                    //...

                                    break;
                            }
                            break;
                        case "sticker":

                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            var response = responseMessage;
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return Ok();
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return BadRequest(responseContent);
            }
        }
    }
}
