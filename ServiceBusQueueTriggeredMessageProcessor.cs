using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Function.ServiceBusMessageProcessor
{
    public class ServiceBusQueueTriggeredMessageProcessor(ILogger<ServiceBusQueueTriggeredMessageProcessor> logger)
    {
        [Function(nameof(ServiceBusQueueTriggeredMessageProcessor))]
        public async Task Run(
            [ServiceBusTrigger("%QueueName%", Connection = "ServiceBusConnection")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            if (message.Body.ToString().Contains("exception"))
                throw new Exception("input message contains exception!");

            logger.LogInformation("Message ID: {id}", message.MessageId);
            logger.LogInformation("Message Body: {body}", message.Body);
            logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

            // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }
    }
}
