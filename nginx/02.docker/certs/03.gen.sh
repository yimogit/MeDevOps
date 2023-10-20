
#!/bin/bash

# 获取当前脚本所在目录
script_dir=$(dirname "$0")

sh $script_dir/02.build-ssl.sh nginx.devops.test.com

sh $script_dir/02.build-ssl.sh apollo.devops.test.com

sh $script_dir/02.build-ssl.sh rabbitmq.devops.test.com

sh $script_dir/02.build-ssl.sh dns.devops.test.com
