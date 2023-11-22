## 消息队列

### RabbitMQ

- RabbitMQ 仓库：https://github.com/rabbitmq/rabbitmq-server
- 教程代码仓库：https://github.com/rabbitmq/rabbitmq-tutorials
- Docker 镜像：https://hub.docker.com/_/rabbitmq/
- 文档：https://rabbitmq.com/documentation.html
- 延迟消息插件下载：https://github.com/rabbitmq/rabbitmq-delayed-message-exchange/releases

## 启动

`docker compose up -d`

## 停止

`docker compose down`

## 插件安装

```sh
docker exec -it rabbitmq_3_12 /bin/bash -c "rabbitmq-plugins enable rabbitmq_delayed_message_exchange"
docker exec -it rabbitmq_3_12 /bin/bash -c "rabbitmq-plugins enable rabbitmq_prometheus"
```

## SDK

.net: https://github.com/rabbitmq/rabbitmq-dotnet-client
