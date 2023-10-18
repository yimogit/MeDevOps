# ⛳️ MeDevOps

关于 DevOps 的一些实践

## 🎯 目标

好记性不如烂笔头，渡人渡己

## 💡 概览

- db: 数据库
  - ./init.sh:创建网络，启动所有服务
  - mongo: 组件服务名
    - 00.docs: 相关文档
    - 01.single:单节点配置
    - 02.cluster:集群配置
    - config: 配置文件夹
    - compose.yml: docker compose 配置
    - run.sh: 启动 compose 的脚本，也可以做一些前置操作
- configservice: 配置中心
  - apollo:阿波罗
- mq
  - rabbitmq
