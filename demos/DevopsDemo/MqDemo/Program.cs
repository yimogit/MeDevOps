
using Furion;
using Furion.Core;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

Serve.Run(services =>
{

    services.AddEventBus(builder =>
    {
        // 注册 ToDo 事件订阅者
        builder.AddSubscriber<ToDoEventSubscriber>();

        // 通过类型注册，Furion 4.2.1+ 版本
        builder.AddSubscriber(typeof(ToDoEventSubscriber));

        // 批量注册事件订阅者
        //builder.AddSubscribers(ass1, ass2, ....);

        // 创建连接工厂
        var factory = new ConnectionFactory
        {
            HostName = "192.168.123.214",
            Port = 5672,
            VirtualHost = "admin_vhost",
            UserName = "root",
            Password = "devops666",
        };

        // 创建默认内存通道事件源对象，可自定义队列路由key，比如这里是 eventbus
        var rbmqEventSourceStorer = new RabbitMQEventSourceStorer(factory, "eventbus", 3000);

        // 替换默认事件总线存储器
        builder.ReplaceStorer(serviceProvider =>
        {
            return rbmqEventSourceStorer;
        });
    });
});
