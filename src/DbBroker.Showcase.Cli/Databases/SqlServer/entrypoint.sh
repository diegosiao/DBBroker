#!/bin/bash

# Start SQL Server in the background
/opt/mssql/bin/sqlservr &

# Wait for SQL Server to start
sleep 20s

# Execute the SQL script using bash
echo "Creating database and seeding data..."
/opt/mssql-tools18/bin/sqlcmd -S localhost -N o -U sa -P dbbroker1! -i /init.sql

# Wait indefinitely to keep the container running
wait
