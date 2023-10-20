## nginx 安装使用

## Windows

- nginx 快速重启脚本：01.windows/nginx-start.bat
- nssm 创建 nginx 服务脚本：01.windows/nginx-nssm-service.bat
- nginx 下载：http://nginx.org/download/nginx-1.24.0.zip
- nssm 下载：http://nginx.org/download/nginx-1.24.0.zip

### 证书申请

openssl 生成自签证书

```sh
#依次执行，输入信息，我这里都输入了 ym
openssl genrsa -out server.key 1024
openssl req -new -key server.key -out server.csr
openssl genrsa -out ca.key 1024
openssl req -new -key ca.key -out ca.csr
openssl x509 -req -in ca.csr -signkey ca.key -out ca.crt
openssl x509 -req -CA ca.crt -CAkey ca.key -CAcreateserial -in server.csr -out server.crt
```

遇到卡死签么加上 winpty ， 即：`winpty openssl genrsa -out server.key 1024`

浏览器提示不安全的解决方案：设置域名客户端导入自己的 CA 证书
原文：https://deliciousbrains.com/ssl-certificate-authority-for-local-https-development/

### 添加自签证书申请脚本

01.build-pem.sh 生成 pem 证书，导入到客户端口受信证书中
02.build-ssl.sh 根据 ca 证书生成对应域名的 ssl 证书
03.gen.sh 生成测试证书，重新生成先删除目录

### 配置 dns

每个域名都配置 hosts 有点麻烦，使用 sameersbn/bind：部署一个 dns 服务

- Github：https://github.com/sameersbn/docker-bind
- 版本： sameersbn/bind bind:9.16.1-20200524 ， webmin: v1.941
- 设置时区
- 设置 webmin 账号密码：root devops666
- 默认端口：
  - '53:53/udp'
  - '53:53/tcp'
- 后台服务端口

  - '10000:10000/tcp'

- 访问：https://ip:10000
- 设置语言：中文
- 配置域名：添加域名到可信赖的访问来源
- 配置参考：https://blog.csdn.net/qq_36961626/article/details/123314087
