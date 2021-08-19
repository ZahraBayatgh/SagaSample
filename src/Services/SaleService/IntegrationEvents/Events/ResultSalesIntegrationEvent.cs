﻿using EventBus.Events;

namespace SaleService.IntegrationEvents.Events
{
    public class ResultSalesIntegrationEvent : IntegrationEvent
    {
        public ResultSalesIntegrationEvent(int productId, bool isSuccess, string correlationId) : base(correlationId)
        {
            ProductId = productId;
            IsSuccess = isSuccess;
            CorrelationId = correlationId;
        }

        public int ProductId { get; private set; }
        public bool IsSuccess { get; private set; }
    }
}
