# 关闭防火墙
systemctl stop firewalld
# 设置永久不开启防火墙
systemctl disable firewalld

# 关闭swap 分区 
swapoff -a   
#永久关闭
sed -ri 's/.*swap.*/#&/' /etc/fstab

# 将 SELinux 设置为 permissive 模式（相当于将其禁用）
setenforce 0
sed -i 's/^SELINUX=enforcing$/SELINUX=permissive/' /etc/selinux/config

# 允许 iptables 检查桥接流量
cat > /etc/sysctl.d/k8s.conf << EOF
net.bridge.bridge-nf-call-ip6tables = 1
net.bridge.bridge-nf-call-iptables = 1
net.ipv4.ip_forward = 1
EOF

# 添加阿里云 yum 源
cat > /etc/yum.repos.d/kubernetes.repo << EOF
[kubernetes]
name=Kubernetes
baseurl=https://mirrors.aliyun.com/kubernetes/yum/repos/kubernetes-el7-x86_64
enabled=1
gpgcheck=0
repo_gpgcheck=0
gpgkey=https://mirrors.aliyun.com/kubernetes/yum/doc/yum-key.gpg https://mirrors.aliyun.com/kubernetes/yum/doc/rpm-package-key.gpg
EOF

# 安装
yum install -y kubeadm-1.18.8 kubelet-1.18.8 kubectl-1.18.8 ipvsadm
# 自启
sudo systemctl enable --now kubelet