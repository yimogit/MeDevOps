# 复制当前目录到linux文件夹 执行即可

# 创建 devops 网络
docker network create devopsnetwork

# mysql运行
cd ./mysql && chmod +x ./run.sh && ./run.sh

cd ..

# mongo运行
cd ./mongo && chmod +x ./run.sh && ./run.sh

cd ..

# clickhouse 运行
cd ./clickhouse && chmod +x ./run.sh && ./run.sh
