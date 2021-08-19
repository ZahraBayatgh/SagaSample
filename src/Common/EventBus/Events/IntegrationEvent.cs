using Newtonsoft.Json;
using System;

namespace EventBus.Events
{
    public class IntegrationEvent
    {
        public IntegrationEvent(string correlationId)
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
            CorrelationId = correlationId;
        }

        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createDate, string correlationId)
        {
            Id = id;
            CreationDate = createDate;
            CorrelationId = correlationId;
        }

        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public DateTime CreationDate { get; private set; }

        [JsonProperty]
        public string CorrelationId { get; set; }
    }
}
