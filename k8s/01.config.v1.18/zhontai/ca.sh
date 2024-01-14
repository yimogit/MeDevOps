
docker login nexus.devops.test.com -u puller -p devops666
kubectl create secret generic testcom_CA \
    --from-file=.dockerconfigjson=~/.docker/config.json> \
    --type=kubernetes.io/dockerconfigjson


# 创建授权
kubectl create secret \
docker-registry \
nexus-login-registry \
--docker-server=nexus.devops.test.com \
--docker-username=puller \
--docker-password=devops666 \
-n default

kubectl create secret tls testcom_CA --cert=./myCA.pem --key=./myCA.key
