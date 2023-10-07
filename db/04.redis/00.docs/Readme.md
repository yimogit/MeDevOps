## 说明

- 安装源：[DockerHub](https://hub.docker.com/_/redis)
- 默认配置文件：[配置文件示例](https://redis.io/docs/management/config/) [6.2](https://raw.githubusercontent.com/redis/redis/6.2/redis.conf)
- 运行时指定配置文件
  ```sh
  docker run -v /myredis/conf:/usr/local/etc/redis --name myredis redis redis-server /usr/local/etc/redis/redis.conf
  ```
- 局域网访问配置
  ```
  #不限制IP访问，局域网能够访问
  bind 0.0.0.0
  #禁用保护模式
  protected-mode no
  ```
- 设置密码:`requirepass devops666`
  - v6.0 后的版本增加了通过 [ACL 的方式设置用户名密码](https://redis.io/docs/management/security/acl/)
- 持久化：`appendonly yes`
  - 启用后默认使用的 AOF（Append-Only File）持久化方式
  - [AOF/RDB 等持久化方式文档说明](https://redis.io/docs/management/persistence/)
- 进程文件：`pidfile /data/redis.pid`
  - 包含当前 Redis 进程的 PID（进程 ID）

## Redis 集群搭建

- 参考：http://www.mydlq.club/article/93/

### 是什么？

- Redis 集群是一种高可用、可水平扩展的 Redis 部署方式。它将 Redis 数据库分布在多个节点上，是为了提供高性能、高可用性和可伸缩性而设计的分布式 Redis 解决方案

### 为什么？

- 高可用性： Redis 集群采用主从复制和故障转移等机制来保证数据的高可用性。当某个节点出现故障时，集群会自动将其角色转移到其他健康节点上，确保数据的持久性和可访问性。
- 可扩展性： Redis 集群支持水平扩展，可以通过增加节点的数量来为系统提供更多的存储和处理能力。同时，Redis 集群也支持在线扩容和缩容，使得扩展和收缩集群变得更加容易。
- 更高的读写性能： 由于 Redis 集群采用了分片机制和客户端分区等技术，可以实现数据的分布式存储和负载均衡，提高了整个系统的吞吐量和性能。此外，Redis 也支持内存映射文件和异步 IO 等特性，进一步提高了读写性能。
- 更安全的数据存储： Redis 集群支持多节点复制和数据备份等机制，可以保证数据的安全性和可靠性。此外，Redis 还提供了密码认证、网络访问控制等安全特性，可以更好地保护数据免受未经授权的访问。
- 更灵活的应用场景： Redis 集群支持的数据结构非常丰富，如字符串、哈希表、列表、集合、有序集合等，可以满足各种不同的应用场景需求。例如，缓存、消息队列、实时计算、会话管理等等。

### 常见问题？

- 配置、管理和维护成本相对高
- 不支持多数据库,只能使用 0 数据库
- 不支持跨节点的事务操作
- 批量操作时支持有限，如数据不在一个节点，则会报错
- 在部署 Redis 集群模式时，至少需要六个节点组成集群才能保证集群的可用性。

### 集群规划

- 节点分配
  - devops02:192.168.123.216
  - devops03:192.168.123.219
  - devops04:192.168.123.222
- 端口分配
  - 6389: redis 访问端口
  - 16389: 集群端口, 普通端口号加 10000，集群节点之间的通讯
- 不要设置密码，未找到节点间通信带密码的解决方案，-a password 只是主节点访问使用
- 集群的 redis 配置模板

  ```yaml
  #端口
  port 6380

  #是否开启 Redis 集群模式
  cluster-enabled yes

  #设置 Redis 集群配置信息及状态的存储位置
  cluster-config-file nodes.conf

  #设置 Redis 群集节点的通信的超时时间
  cluster-node-timeout 5000
  appendonly yes
  daemonize no
  protected-mode no
  pidfile  /data/redis.pid

  #主节点需要的最小从节点数，只有达到这个数，主节点失败时，它从节点才会进行迁移。
  # cluster-migration-barrier 1

  #设置集群可用性
  # cluster-require-full-coverage yes

  # 集群节点 IP，如果要外部访问需要修改为宿主机IP，如：192.168.123.216
  # cluster-announce-ip 默认172.x.x.x
  #客户端连接端口
  #cluster-announce-port 6380
  #节点间通信端口
  #cluster-announce-bus-port 16380
  ```

- docker compose.yml 配置模板

  ```yaml
  version: '3.1'
  services:
    redis:
      container_name: cluster_redis_6380
      image: redis:6.2.13
      restart: always
      command: redis-server /usr/local/etc/redis/redis.conf
      volumes:
        - ./data:/data
        - ./config/redis.conf:/usr/local/etc/redis/redis.conf
      ports:
        - '6380:6380'
        - '16380:16380'
      networks:
        - devopsnetwork

  networks:
    devopsnetwork:
      external: true
  ```

### 集群部署

将上面的配置模板文件按下面的目录结构创建

```md
- node-cluster
  - redis-6380
    - config
      - redis.conf :需要配置模板中修改 port:6380
    - compose.yml:需要配置模板中修改 container_name: cluster_redis_6380
  - redis-6381
    - config
      - redis.conf :需要配置模板中修改 port:6381
    - compose.yml:需要配置模板中修改 container_name: cluster_redis_6381
```

1. 修改 redis.conf 中的 port
2. 修改 compose.yml 中的 container_name: cluster_redis_6381
3. 将 node-cluster 目录上传到准备的集群节点服务器：192.168.123.216，192.168.123.219，192.168.123.222
4. 在服务器的对应目录中执行 `docker compose up -d`
5. 依托于创建的 redis 容器，使用下面的命令创建集群并添加节点

```sh
devops02=192.168.123.216
devops03=192.168.123.219
devops04=192.168.123.222

docker exec -it cluster_redis_6380 redis-cli -p 6380 --cluster create \
${devops02}:6380 \
${devops02}:6381 \
${devops03}:6380 \
${devops03}:6381 \
${devops04}:6380 \
${devops04}:6381 \
--cluster-replicas 1 \
--cluster-yes
```

6. 连接集群测试

```sh
docker exec -it cluster_redis_6380 redis-cli -p 6380 -c
```

- 查看集群信息：`cluster info`
- 查看集群节点信息:`cluster nodes`

本地连接测试

```sh
redis-cli -h 192.168.123.216 -p 6380 -c
```

至此，终于是一步步的创建好了 redis 集群

## 集群外部访问

设置 cluster-announce-ip 为宿主机 IP 即可

## 后语

耗时半天，先是折腾了一会创建集群密码的问题，然后又是尝试各种连接工具，时间太快了
