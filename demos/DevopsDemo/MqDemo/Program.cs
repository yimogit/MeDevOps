
using Furion;
using Furion.Core;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

Serve.Run(services =>
{

    services.AddEventBus(builder =>
    {
        // ע�� ToDo �¼�������
        builder.AddSubscriber<ToDoEventSubscriber>();

        // ͨ������ע�ᣬFurion 4.2.1+ �汾
        builder.AddSubscriber(typeof(ToDoEventSubscriber));

        // ����ע���¼�������
        //builder.AddSubscribers(ass1, ass2, ....);

        // �������ӹ���
        var factory = new ConnectionFactory
        {
            HostName = "192.168.123.214",
            Port = 5672,
            VirtualHost = "admin_vhost",
            UserName = "root",
            Password = "devops666",
        };

        // ����Ĭ���ڴ�ͨ���¼�Դ���󣬿��Զ������·��key������������ eventbus
        var rbmqEventSourceStorer = new RabbitMQEventSourceStorer(factory, "eventbus", 3000);

        // �滻Ĭ���¼����ߴ洢��
        builder.ReplaceStorer(serviceProvider =>
        {
            return rbmqEventSourceStorer;
        });
    });
});
