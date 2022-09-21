using IncredibleBackend.Messaging.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace IncredibleBackend.Messaging;

public class MessageProducer : IMessageProducer
{
    private readonly ILogger<MessageProducer> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public MessageProducer(ILogger<MessageProducer> logger, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task ProduceMessage<T>(T model, string message)
    { 
        _logger.LogInformation(message);
        await _publishEndpoint.Publish(model);
    }
}
