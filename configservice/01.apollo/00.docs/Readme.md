## Apollo 配置中心

版本：2.1.0
快速开始：https://www.apolloconfig.com/#/zh/deployment/quick-start-docker
分布式部署：https://www.apolloconfig.com/#/zh/deployment/distributed-deployment-guide
部署架构：https://www.apolloconfig.com/#/zh/deployment/deployment-architecture
性能测试：https://www.apolloconfig.com/#/zh/misc/apollo-benchmark
## 端口说明

JVM8080：对外暴露的网络端口是 8080，里面有 Meta Server，Eureka，Config Service，其中 Config Service 又使用了 ConfigDB

JVM8090：对外暴露的网络端口是 8090，里面有 Admin Service，并且 Admin Service 使用了 ConfigDB

JVM8070：对外暴露的网络端口是 8070，里面有 Portal，并且 Portal 使用了 PortalDB

## 常用命令

```sh
#启动容器所有服务
docker compose up -d
docker compose down

#启动
docker compose up -d apollo-adminservice
docker compose down apollo-adminservice
docker compose restart apollo-adminservice

docker compose up -d apollo-configservice
docker compose down apollo-configservice
docker compose restart apollo-configservice

docker compose up -d apollo-portal
docker compose down apollo-portal
```

## 问题

- 数据库连接：SPRING_DATASOURCE_URL 中使用服务名 apollo-db
- 默认账号密码：apollo admin
- eureka.service.url 配置：默认从数据库读取，可以修改数据库 apolloconfigdb.sql 437 行，也可以在 compose.yml 指定
- 配置项 key 最大长度限制：默认配置是 128。
- 配置项 value 最大长度限制:默认配置是 20000。
