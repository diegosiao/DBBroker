#!/bin/bash
# Start SQL Server in the background
/opt/mssql/bin/sqlservr & sleep 20s # Wait for SQL Server to start
echo "Creating database and seeding data..."
/opt/mssql-tools18/bin/sqlcmd -S localhost -N o -U sa -P DBBroker_1 -i /init.sql
wait # Wait indefinitely to keep the container running