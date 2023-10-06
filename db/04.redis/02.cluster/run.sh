
# 创建 devops 网络
# docker network create devopsnetwork

# 6380运行
cd ./redis-6380 && chmod +x ./run.sh && ./run.sh

cd ..

# 6381运行
cd ./redis-6381 && chmod +x ./run.sh && ./run.sh

cd ..

