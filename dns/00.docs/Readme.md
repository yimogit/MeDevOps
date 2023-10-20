## 配置 dns

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
- 访问 webmin：https://ip:10000
- 设置语言：中文
- 配置 dns 将 *.test.com 解析到 192.168.123.214，参考：https://blog.csdn.net/qq_36961626/article/details/123314087
- 本地设置 dns:192.168.123.214
- 使用 https://ip:10000访问 webmin
- 配置 nginx 域名：dns.devops.test.com
- 添加域名到可信赖的访问来源
- 现在使用 https 访问就可以了
- 添加域名也无需再配置hosts了