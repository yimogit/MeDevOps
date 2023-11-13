## 创建目录并赋权限

当前最新版本：v10.2.0

```sh
mkdir -p grafana_data &&  chown -R 472:472 ./grafana_data
```

### grafana

k8s 监控需要安装插件 DevOpsProdigy KubeGraf，版本用 6.7.2，配置 config basic64 解码配置，进入容器中 安装依赖 grafana-cli plugins install grafana-piechart-panel

## ubuntu 安装 prometheus-node-exporter

1. 我们执行这个命令来更新可用软件包的列表和它们的所有版本。
   sudo apt-get update
2. 通过这个命令，我们将继续安装软件包
   sudo apt-get install prometheus-node-exporter
3. 要检查你是否已经成功安装了软件包，你可以用下面的命令列出所有已安装的软件包。
   dpkg -l prometheus-node-exporter

## windows 安装 prometheus-node-exporter

## k8s 插件安装

插件名 DevOpsProdigy KubeGraf

## clickhouse 插件安装

插件名 vertamedia-clickhouse-datasource

## mysql 安装 mysqld_exporter

docker run -d -p 9104:9104 -e DATA_SOURCE_NAME="root:devops666@(192.168.123.214:3306)/" --name mysqld_exporter prom/mysqld-exporter:v0.14.0

## mongo-exporter 安装 mongo-exporter

docker run -d -p 9216:9216 percona/mongodb_exporter:0.39.0 --mongodb.uri=mongodb://root:devops666@192.168.123.214:27017/admin?ssl=false --mongodb.indexstats-colls=crm_basic --mongodb.collstats-colls=\*

docker run -d -p 9216:9216 -p 17001:17001 percona/mongodb_exporter:0.20.0 --mongodb.uri=mongodb://prometheus:prometheus@192.168.123.214:27017 --compatible-modemongodb_ss_wt_log_log_bytes_written

docker run -d \
 --name mongodb-exporter \
 -p 9216:9216 \
 percona/mongodb*exporter:0.39.0 \
 /opt/bitnami/mongodb-exporter/bin/mongodb_exporter --discovering-mode --mongodb.indexstats-colls=* --mongodb.collstats-colls=\_ --web.listen-address=":9216" --web.telemetry-path="/metrics" --mongodb.direct-connect=false --mongodb.uri="mongodb://root:devops666@192.168.123.214:27017/admin?ssl=false"

### mongodb 插件 grafana-mongodb-datasource 安装

- `grafana-cli plugins install grafana-mongodb-datasource` 被墙了，
- 直接官网下载，复制到/app/prometheus-grafana/grafana_data/plugins，然后重启 grafana `cd /app/prometheus-grafana && docker-compose restart grafana`

## grafana 面板

version:v6.7.2
envoy:11021
windows:12566
Linux:10180
k8s:插件 DevOpsProdigy KubeGraf，配置 config basic64 解码配置，进入容器中 安装依赖 grafana-cli plugins install grafana-piechart-panel
clickhouse:2515，需要安装插件并配置数据源 grafana-cli plugins install vertamedia-clickhouse-datasource 1.9.5
mysql:7362
mongo:
