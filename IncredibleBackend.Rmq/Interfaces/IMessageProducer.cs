namespace IncredibleBackend.Messaging.Interfaces;

public interface IMessageProducer
{
    public Task ProduceMessage<T>(T model, string message);
}
