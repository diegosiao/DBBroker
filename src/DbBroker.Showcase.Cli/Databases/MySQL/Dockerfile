FROM mysql:latest

# Set environment variables
ENV MYSQL_ROOT_PASSWORD=DBBroker_1
# ENV MYSQL_DATABASE=dbbroker
# ENV MYSQL_USER=dbbroker
# ENV MYSQL_PASSWORD=DBBroker_1

# Copy the initialization script
COPY init.sql /docker-entrypoint-initdb.d/

# Expose the default MySQL port
EXPOSE 3306
