mkdir -p prometheus_data && chown 65534 ./prometheus_data
docker compose down
docker compose up -d