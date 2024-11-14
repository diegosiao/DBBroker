FROM mcr.microsoft.com/mssql/server:2019-latest

# Set environment variables
ENV SA_PASSWORD=YourStrong!Passw0rd
ENV ACCEPT_EULA=Y

# Copy the initialization scripts
COPY init.sql /init.sql
COPY entrypoint.sh /entrypoint.sh
COPY entrypoint.ps1 /entrypoint.ps1

# Grant permissions to the entrypoint script
# RUN chmod +x /entrypoint.sh

# Run the entrypoint script
ENTRYPOINT ["/bin/bash", "/entrypoint.sh"]
