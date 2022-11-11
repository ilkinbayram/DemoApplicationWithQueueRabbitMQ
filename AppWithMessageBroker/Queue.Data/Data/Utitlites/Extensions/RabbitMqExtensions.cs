using Queue.Data.Data.Resources.Enum;
using RabbitMQ.Client;

namespace Queue.Data.Data.Utitlites.Extensions
{
    public static class RabbitMqExtensions
    {
        public static IModel DeclareQueue(this IModel channel, string name)
        {
            if (string.IsNullOrEmpty(name))
                name = $"def_que.{Guid.NewGuid().ToString().ToLower().Replace("-",string.Empty).Substring(0,6)}";
            channel.QueueDeclare(name, true, false, false);
            return channel;
        }

        public static IModel DeclareExchange(this IModel channel, string exchangeName, ExchType exchangeType)
        {
            if (string.IsNullOrEmpty(exchangeName))
                exchangeName = $"def_exch.{Guid.NewGuid().ToString().ToLower().Replace("-", string.Empty).Substring(0, 6)}";
            channel.ExchangeDeclare(exchangeName, exchangeType.ToString(), true, false);
            return channel;
        }

        public static IModel Bind(this IModel channel, string queueName, string exchangeName, string routingKey)
        {
            if (string.IsNullOrEmpty(routingKey))
                routingKey = $"def_route.k.{Guid.NewGuid().ToString().ToLower().Replace("-", string.Empty).Substring(0, 6)}";

            channel.QueueBind(queueName, exchangeName, routingKey, null);
            return channel;
        }
    }
}
