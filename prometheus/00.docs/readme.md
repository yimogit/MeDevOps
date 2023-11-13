## 创建目录并赋权限

```sh
mkdir prometheus_data && chown 65534 ./prometheus_data
```

## 重载配置

```sh
curl -X POST http://localhost:9090/-/reload
或
curl -X POST -u root:devops666 http://localhost:9090/-/reload
```

## linux docker 安装 node_exporter

```sh
docker run -d \
  -p 9100:9100 \
  -v "/:/host:ro,rslave" \
  --name node_exporter  \
  quay.io/prometheus/node-exporter:v1.7.0 \
  --path.rootfs=/host
```

- Grafana 面板推荐：
  - Linux Hosts Metrics | Base ID：10180
  - Node Exporter Dashboard 220417 通用 Job 分组版 ID: 16098
