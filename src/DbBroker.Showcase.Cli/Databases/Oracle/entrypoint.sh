#!/bin/bash

# Start Oracle XE in the background
/etc/init.d/oracle-xe start

# Wait for Oracle XE to start
until echo "SELECT 1 FROM DUAL;" | sqlplus -s SYSTEM/oracle@//localhost:1521/XE
do
  echo "Waiting for Oracle XE to start..."
  sleep 2
done

# Execute the SQL script
echo "Oracle XE is up, executing init.sql..."
sqlplus SYSTEM/oracle@//localhost:1521/XE @/docker-entrypoint-initdb.d/init.sql

# Keep the container running
tail -f /dev/null
