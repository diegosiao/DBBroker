# Start SQL Server in the background
Start-Process -FilePath "sqlservr.exe" -ArgumentList "--accept-eula" -NoNewWindow

# Wait for SQL Server to start
$ErrorActionPreference = "Stop"
$connectionString = "Server=localhost;Integrated Security=SSPI;Connection Timeout=30"

do {
    try {
        Write-Output "Waiting for SQL Server to start..."
        $sqlcmd = Invoke-Sqlcmd -ConnectionString $connectionString -Query "SELECT 1" -ErrorAction Stop
        $isReady = $true
    } catch {
        Start-Sleep -Seconds 2
    }
} while (-not $isReady)

# Execute the SQL script
Write-Output "SQL Server is up, executing init.sql..."
Invoke-Sqlcmd -ConnectionString $connectionString -InputFile "/init.sql"

# Wait indefinitely to keep the container running
Wait-Process -Name "sqlservr"
