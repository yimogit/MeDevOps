## 说明

- [Docker Compose V2 安装 ClickHouse v20.6.8.5 ](https://juejin.cn/post/7285922296009850937)

## 获取默认配置文件方法

```sh
mkdir ./work
docker run -it --rm --entrypoint=/bin/bash -v ./work:/work --privileged=true --user=root yandex/clickhouse-server:20.6.8.5
进入容器后复制配置到work文件夹
cp -r /etc/clickhouse-server/* /work
exit
```

## 进入容器创建具有管理员权限的用户 root

```sh
# 进入容器
docker exec -it db_clickhouse_20_6 /bin/bash
# 连接clickhouse
clickhouse-client --user default --password devops666
# 创建 root 用户
CREATE USER root IDENTIFIED BY 'devops666';

#启用/禁用内省功能以进行查询概要分析，否则会报错 Code: 446
set allow_introspection_functions=1

# 授权管理员权限
GRANT ALL ON *.* TO root WITH GRANT OPTION;

# 创建测试数据库
CREATE DATABASE testdb

# 创建测试表
CREATE TABLE testdb.Ck_TestInfo (Id UInt64, Name String) ENGINE = MergeTree() ORDER BY Id;

```

## 其他

[访问权限文档](https://clickhouse.com/docs/zh/operations/access-rights#enabling-access-control)
CURRENT_USER:当前用户，或指定用户
查看用户：`SHOW CREATE USER CURRENT_USER`
查看用户权限：`SHOW GRANTS For CURRENT_USER`
