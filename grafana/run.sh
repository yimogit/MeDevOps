mkdir -p grafana_data &&  chown -R 472:472 ./grafana_data
docker compose down
docker compose up -d