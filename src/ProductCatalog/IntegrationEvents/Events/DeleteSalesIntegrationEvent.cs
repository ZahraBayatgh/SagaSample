﻿using EventBus.Events;

namespace ProductCatalogService.IntegrationEvents.Events
{
    public class DeleteInventoryIntegrationEvent: IntegrationEvent
    {
        public DeleteInventoryIntegrationEvent(string productName, string correlationId) : base(correlationId)
        {
            ProductName = productName;
            CorrelationId = correlationId;
        }

        public string ProductName { get; private set; }
    }
}
