## 在其他服务器使用镜像

### 设置 dns

首先需要设置 dns,才能够访问到 nexus.devops.test.com

```
vi /etc/resolv.conf
```

修改为

```
nameserver 192.168.123.214
nameserver 114.114.114.114
```

重启网络：`systemctl restart NetworkManager`

ping nexus.devops.test.com
ip 是 192.168.123.214 就对了

### 登录

docker login nexus.devops.test.com -u puller -p devops666
docker login push.nexus.devops.test.com -u pusher -p devops666

提示：Error response from daemon: Get "https://nexus.devops.test.com/v2/": tls: failed to verify certificate: x509: certificate signed by unknown authority

是因为 https 是自签证书，也需要安装 pem 证书到 linux

1. 复制 myCA.pem 到主机
2. 复制证书到证书安装目录 `cp ./myCA.pem /etc/pki/ca-trust/source/anchors/`
3. 生效证书：`update-ca-trust -f`
4. 重启 openssl 服务(查找openssl `ps aux | grep openssl`的pid: xxx xxx pid号 pts/0 R+，kill掉pid,会自动重新启动 `kill pid号`)
5. 或直接重启服务器生效(`reboot`)


### 配置成功
