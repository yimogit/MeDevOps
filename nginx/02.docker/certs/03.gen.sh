
#!/bin/bash

# 获取当前脚本所在目录
script_dir=$(dirname "$0")
cd $script_dir
sh ./02.build-ssl.sh nginx.devops.test.com

sh ./02.build-ssl.sh apollo.devops.test.com

sh ./02.build-ssl.sh rabbitmq.devops.test.com

sh ./02.build-ssl.sh dns.devops.test.com

sh ./02.build-ssl.sh nexus.devops.test.com

sh ./02.build-ssl.sh push.nexus.devops.test.com

sh ./02.build-ssl.sh jumpserver.devops.test.com

sh ./02.build-ssl.sh prometheus.devops.test.com

sh ./02.build-ssl.sh grafana.devops.test.com

sh ./02.build-ssl.sh k8s.devops.test.com

sh ./02.build-ssl.sh kibana.devops.test.com
