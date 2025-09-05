FROM container-registry.oracle.com/database/express:21.3.0-xe

ADD init.sql /opt/oracle/scripts/startup/
# ADD entrypoint.sh /opt/oracle/scripts/startup/

# Copy the initialization script
# COPY init.sql /docker-entrypoint-initdb.d/init.sql

# # Grant permissions to the entrypoint script
# RUN chmod +x /docker-entrypoint-initdb.d/init.sql

# Expose the default Oracle port
EXPOSE 1521
EXPOSE 5500

# Set environment variables
# ENV ORACLE_HOME=/u01/app/oracle/product/18.4.0/xe
# ENV PATH=$ORACLE_HOME/bin:$PATH
ENV ORACLE_SID=XE
ENV ORACLE_PDB=XE
ENV ORACLE_PWD=DBBroker_1

# hostname: localhost
# port: 49161
# sid: xe
# username: system
# password: oracle

# Start the Oracle service and run the initialization script
# CMD ["/bin/sh", "-c", "service oracle-xe start && /docker-entrypoint-initdb.d/init.sql"]
