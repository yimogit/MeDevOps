
#!/bin/bash

if [ "$#" -ne 1 ]
then
  echo "Usage: Must supply a domain"
  exit 1
fi

DOMAIN=$1

mkdir $DOMAIN

winpty openssl genrsa -out $DOMAIN/server.key 2048
winpty openssl req -new -key $DOMAIN/server.key -out $DOMAIN/server.csr

cat > $DOMAIN/server.ext << EOF
authorityKeyIdentifier=keyid,issuer
basicConstraints=CA:FALSE
keyUsage = digitalSignature, nonRepudiation, keyEncipherment, dataEncipherment
subjectAltName = @alt_names
[alt_names]
DNS.1 = $DOMAIN
EOF
winpty openssl x509 -req -in $DOMAIN/server.csr -CA ./myCA.pem -CAkey ./myCA.key -CAcreateserial -out $DOMAIN/server.crt -days 36500 -extfile $DOMAIN/server.ext
