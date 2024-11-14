FROM oracleinanutshell/oracle-xe-11g

ADD init.sql /docker-entrypoint-initdb.d/
ADD entrypoint.sh /docker-entrypoint-initdb.d/

# Copy the initialization script
# COPY init.sql /docker-entrypoint-initdb.d/init.sql

# # Grant permissions to the entrypoint script
# RUN chmod +x /docker-entrypoint-initdb.d/init.sql

# Expose the default Oracle port
EXPOSE 1521

# Set environment variables
ENV ORACLE_HOME=/u01/app/oracle/product/11.2.0/xe
ENV PATH=$ORACLE_HOME/bin:$PATH
ENV ORACLE_SID=XE
ENV ORACLE_PDB=XE

# hostname: localhost
# port: 49161
# sid: xe
# username: system
# password: oracle

# Start the Oracle service and run the initialization script
# CMD ["/bin/sh", "-c", "service oracle-xe start && /docker-entrypoint-initdb.d/init.sql"]
