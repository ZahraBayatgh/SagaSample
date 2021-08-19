using Microsoft.Azure.ServiceBus;
using System;

namespace EventBus.ServiceBus
{
    public interface IServiceBusPersisterConnection : IDisposable
    {
        ITopicClient TopicClient { get; }
        ISubscriptionClient SubscriptionClient { get; }
        public string SubscriptionClientName { get; set; }
    }
}