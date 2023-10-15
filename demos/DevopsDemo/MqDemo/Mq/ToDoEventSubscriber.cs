// 实现 IEventSubscriber 接口
using Furion.EventBus;

public class ToDoEventSubscriber : IEventSubscriber
{
    private readonly ILogger<ToDoEventSubscriber> _logger;
    public ToDoEventSubscriber(ILogger<ToDoEventSubscriber> logger)
    {
        _logger = logger;
    }

    [EventSubscribe("ToDo:Create")]
    public async Task CreateToDo(EventHandlerExecutingContext context)
    {
        var todo = context.Source;
        _logger.LogInformation("创建一个 ToDo：{Name}", todo.Payload);
        await Task.CompletedTask;
    }

    // 支持多个
    [EventSubscribe("ToDo:Create")]
    [EventSubscribe("ToDo:Update")]
    public async Task CreateOrUpdateToDo(EventHandlerExecutingContext context)
    {
        var todo = context.Source;
        _logger.LogInformation("创建或更新一个 ToDo：{Name}", todo.Payload);
        await Task.CompletedTask;
    }


    // 支持正则表达式匹配，4.2.10+ 版本支持
    [EventSubscribe("(^1[3456789][0-9]{9}$)|((^[0-9]{3,4}\\-[0-9]{3,8}$)|(^[0-9]{3,8}$)|(^\\([0-9]{3,4}\\)[0-9]{3,8}$)|(^0{0,1}13[0-9]{9}$))", FuzzyMatch = true)]
    public async Task RegexHandler(EventHandlerExecutingContext context)
    {
        var eventId = context.Source.EventId;
        await Task.CompletedTask;
    }

    // 支持多种异常重试配置，Furion 4.2.10+ 版本支持
    [EventSubscribe("test:error", NumRetries = 3)]
    [EventSubscribe("test:error", NumRetries = 3, RetryTimeout = 1000)] // 重试间隔时间
    [EventSubscribe("test:error", NumRetries = 3, ExceptionTypes = new[] { typeof(ArgumentException) })]    // 特定类型异常才重试
    public async Task ExceptionHandler(EventHandlerExecutingContext context)
    {
        var eventId = context.Source.EventId;
        await Task.CompletedTask;
    }
}