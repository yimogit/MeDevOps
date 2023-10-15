using Furion.EventBus;

namespace MqDemo.Services
{
    [DynamicApiController]
    public class MqService
    {
        // 依赖注入事件发布者 IEventPublisher
        private readonly IEventPublisher _eventPublisher;
        public MqService(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }
        // 发布 ToDo:Create 消息
        public async Task CreateDoTo(string name)
        {
            await _eventPublisher.PublishAsync(new ChannelEventSource("ToDo:Create", name));
            // 也可以延迟发布，比如延迟 3s
            await _eventPublisher.PublishDelayAsync(new ChannelEventSource("ToDo:Create", name), 3000);
        }


    }
}
