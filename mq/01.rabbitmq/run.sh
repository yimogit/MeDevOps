docker compose down
docker compose up -d
docker exec -it rabbitmq_3_12 /bin/bash -c "rabbitmq-plugins enable rabbitmq_delayed_message_exchange"
docker exec -it rabbitmq_3_12 /bin/bash -c "rabbitmq-plugins enable rabbitmq_prometheus"
