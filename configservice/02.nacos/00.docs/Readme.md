## Nacos 配置中心

版本：2.3.1
快速开始：https://nacos.io/docs/latest/quickstart/quick-start-docker/
K8S 部署 nacos：https://nacos.io/docs/latest/quickstart/quick-start-kubernetes/
架构：https://nacos.io/docs/latest/architecture/

## 端口说明

8848：服务端口
9848：GRPC 通信端口

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
