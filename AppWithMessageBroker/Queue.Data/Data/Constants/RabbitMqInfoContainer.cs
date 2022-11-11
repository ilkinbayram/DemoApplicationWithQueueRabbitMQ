using Queue.Data.Data.Resources;

namespace Queue.Data.Data.Constants
{
    public static class RabbitMqInfoContainer
    {
        private static IList<ChannelInfo> _channels
        {
            get
            {
                if (!_channels.Any()) return new List<ChannelInfo>();
                return _channels;
            }
            set
            {
                _channels = value;
            }
        }
        public static IList<ChannelInfo> Channels
        {
            get => _channels;
        }
        public static ChannelInfo LastChannel => _channels.Count == 0 ? null : _channels.LastOrDefault();
        public static int Count => _channels.Count;
        public static bool SetChannel(ChannelInfo info)
        {
            try
            {
                bool result = false;
                if (info == null)
                    return result;

                var foundInfo = _channels.FirstOrDefault(x => x.Id == info.Id);
                if (foundInfo == null)
                {
                    if (_channels.Any(x => x.QueueName == info.QueueName ||
                                      x.ExchangeName == info.ExchangeName ||
                                      x.ChannelName == info.ChannelName))
                        return result;
                    _channels.Add(info);
                    result = true;
                    return result;
                }

                foundInfo.ChannelName = info.ChannelName;
                foundInfo.QueueName = info.QueueName;
                foundInfo.ExchangeName = info.ExchangeName;
                foundInfo.ExchangeType = info.ExchangeType;
                foundInfo.RoutingKey = info.RoutingKey;
                result = true;
                return result;
            }
            catch (Exception exc)
            {
                var exMessage = exc.Message;
                var inner = exc.InnerException;
                return false;
            }
            
        }
        public static ChannelInfo GetChannelById(string Id)
        {
            return _channels.FirstOrDefault(x => x.Id == Id);
        }

        public static ChannelInfo GetChannel(string channelName)
        {
            return _channels.FirstOrDefault(x => x.ChannelName == channelName);
        }

        public static ChannelInfo GetChannelByQueue(string queueName)
        {
            return _channels.FirstOrDefault(x => x.QueueName == queueName);
        }

        public static ChannelInfo GetChannelByExchange(string exchangeName)
        {
            return _channels.FirstOrDefault(x => x.ExchangeName == exchangeName);
        }
    }
}
