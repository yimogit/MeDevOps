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
