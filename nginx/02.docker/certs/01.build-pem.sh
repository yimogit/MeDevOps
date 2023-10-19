# 生成根证书,访问客户端需要安装导入 myCA.pem，根据myCA.key,myCA.pem再生成nginx需要的证书
winpty openssl genrsa -out myCA.key 2048
winpty openssl req -x509 -new -nodes -key myCA.key -days 1825 -out myCA.pem
