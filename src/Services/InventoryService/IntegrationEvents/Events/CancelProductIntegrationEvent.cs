using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaleService.IntegrationEvents.Events
{
    public class CancelProductIntegrationEvent :IntegrationEvent
    {
        public CancelProductIntegrationEvent(string name, int count)
        {
            Name = name;
            Count = count;
        }

        public string Name { get; set; }
        public int Count { get; set; }
    }
}
