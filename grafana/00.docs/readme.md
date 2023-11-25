## 创建目录并赋权限

当前最新版本：v10.2.0

```sh
mkdir -p grafana_data &&  chown -R 472:472 ./grafana_data
mkdir -p grafana_data_6 &&  chown -R 472:472 ./grafana_data_6
```

可以直接在后台安装数据源及插件