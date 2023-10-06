
# 创建 devops 网络
# docker network create devopsnetwork

# 6380运行
cd ./redis-6380 && docker compose down && rm ./data -rf && docker compose up -d

cd ..

# 6381运行
cd ./redis-6381 && docker compose down && rm ./data -rf && docker compose up -d

cd ..

