#!/bin/bash

if [ "$#" -ne 1 ]; then
  echo "Usage: Must supply a domain"
  exit 1
fi

DOMAIN=$1

mkdir $DOMAIN
#!/bin/sh
if uname | grep -q "MINGW"; then
  winpty openssl genrsa -out $DOMAIN/server.key 2048
  winpty openssl req -new -key $DOMAIN/server.key -out $DOMAIN/server.csr
else
  openssl genrsa -out $DOMAIN/server.key 2048
  openssl req -new -key $DOMAIN/server.key -out $DOMAIN/server.csr
fi

cat >$DOMAIN/server.ext <<EOF
authorityKeyIdentifier=keyid,issuer
basicConstraints=CA:FALSE
keyUsage = digitalSignature, nonRepudiation, keyEncipherment, dataEncipherment
subjectAltName = @alt_names
[alt_names]
DNS.1 = $DOMAIN
EOF

if uname | grep -q "MINGW"; then
  winpty openssl x509 -req -in $DOMAIN/server.csr -CA ./myCA.pem -CAkey ./myCA.key -CAcreateserial -out $DOMAIN/server.crt -days 36500 -extfile $DOMAIN/server.ext
else
  openssl x509 -req -in $DOMAIN/server.csr -CA ./myCA.pem -CAkey ./myCA.key -CAcreateserial -out $DOMAIN/server.crt -days 36500 -extfile $DOMAIN/server.ext
fi
