namespace MSCoreProject.LineSDK
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    public class ReceivedMessage
    {
        [JsonProperty("destination")]
        public string Destination { get; set; }

        [JsonProperty("events")]
        public List<Event> Events { get; set; }
    }

    public class Event
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("message")]
        public Messages Message { get; set; }

        [JsonProperty("webhookEventId")]
        public string WebhookEventId { get; set; }

        [JsonProperty("deliveryContext")]
        public DeliveryContext DeliveryContext { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("source")]
        public Source Source { get; set; }

        [JsonProperty("replyToken")]
        public string ReplyToken { get; set; }

        [JsonProperty("mode")]
        public string Mode { get; set; }
    }

    public class Messages
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class DeliveryContext
    {
        [JsonProperty("isRedelivery")]
        public bool IsRedelivery { get; set; }
    }

    public class Source
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }
    }
}
