# powershell environemnt variables
```
use below commands to set vault keys prior running any
ef core database commands otherwise you will have exceptions
like add-migration, ...

$env:FlixHubKeys:VaultKey1='h8LqW9z0+DpXMg2y+Q7kRw=='
$env:FlixHubKeys:VaultKey2='Q9@h]n3zT!mV^2Lf'
```

# databse scaffolding
```
* FlixHub database *
Add-Migration FlixHubDb_Init -Context FlixHubDbContext -StartupProject FlixHub.Api -Project FlixHub.Core.Api -verbos
Update-Database -Context FlixHubDbContext -StartupProject FlixHub.Api -Project FlixHub.Core.Api -verbos
Remove-Migration -Context FlixHubDbContext -StartupProject FlixHub.Api -Project FlixHub.Core.Api -verbos
Drop-Database -Context FlixHubDbContext -StartupProject FlixHub.Api -Project FlixHub.Core.Api -verbos
Script-Migration -Context FlixHubDbContext -StartupProject FlixHub.Api -Project FlixHub.Core.Api -verbos

* message bus *
Add-Migration OutBox_Init -Context OutBoxDbContext -StartupProject FlixHub.Api -Project FlixHub.Core -Verbose
Update-Database -Context OutBoxDbContext -StartupProject FlixHub.Api -Project FlixHub.Core -Verbose
```

# docker images
```

# Create rabbitmq
docker run -d --name RabbitMQ -p 5672:5672 -p 15672:15672 -e RABBITMQ_DEFAULT_USER=guest -e RABBITMQ_DEFAULT_PASS=guest rabbitmq:3-management

# Create redis stack
docker run -d --name redis-stack -p 6379:6379 -p 8001:8001 -e REDIS_ARGS="--requirepass P@ssw0rd" redis/redis-stack:latest

# Create a custom network
docker network create flixhub-net

# Recreate PostgreSQL container
docker run --name postgres-dev --network flixhub-net -e POSTGRES_USER=sa -e POSTGRES_PASSWORD=sa -e POSTGRES_DB=FlixHubDb -p 5432:5432 -d postgres:16

# Recreate pgAdmin container
docker run --name pgadmin-dev --network flixhub-net -e PGADMIN_DEFAULT_EMAIL=mohannad.xox@gmail.com -e PGADMIN_DEFAULT_PASSWORD=admin -p 5050:80 -d dpage/pgadmin4

```

# backup & restore pstgresql
```

// Backup one database (logical, portable)
// Replace FlixHubDb with your database name
docker exec -t postgres-dev pg_dump -U postgres -d FlixHubDb -F c -f /tmp/FlixHubDb.dump

// Copy the dump to Windows
docker cp postgres-dev:/tmp/FlixHubDb.dump "C:\Backups\FlixHubDb.dump"

// Restore one database (logical, portable)
docker cp "C:\Backups\FlixHubDb.dump" postgres-dev:/tmp/FlixHubDb.dump

// Create a fresh, empty DB (from template0)
docker exec -it postgres-dev createdb -U postgres -T template0 FlixHubDb_restored

// Restore with pg_restore
docker exec -it postgres-dev pg_restore -U postgres -d FlixHubDb_restored --clean --if-exists /tmp/FlixHubDb.dump

```

# Where pgAdmin puts backup files (in Docker)
```

// Default path:
/var/lib/pgadmin/storage/mohannad.xox_gmail.com

// Quick way to find the file path
docker exec -it pgadmin-dev bash -lc "ls -la /var/lib/pgadmin/storage/mohannad.xox_gmail.com

```

# Upgrade pgAdmin Docker image
```

docker run -d --name pgadmin-dev -p 5050:80 -e PGADMIN_DEFAULT_EMAIL=mohannad.xox@gmail.com -e PGADMIN_DEFAULT_PASSWORD=admin -v pgadmin-ta:/var/lib/pgadmin dpage/pgadmin4:9.9

```

# flixhub api docker build & deployment (flexible parameters)
```
powershell
# ============================================================================
# FLEXIBLE DOCKER BUILD SCRIPT
# ============================================================================
# IMPORTANT: Run from repository root directory (D:\Source Code\Git\flix-hub-backend\)

# --- STEP 1: Define Variables ---
$ImageName = "flixhub-api"                    # Docker image name
$Version = "latest"                           # Image version/tag (e.g., "1.0.0", "latest")
$ContainerName = "flixhub-api"               # Container name
$HttpPort = 2535                             # HTTP port on host
$HttpsPort = 6979                            # HTTPS port on host
$Protocol = "Production"                      # ASPNETCORE_ENVIRONMENT (Development/Staging/Production)
$VaultKey1 = "h8LqW9z0+DpXMg2y+Q7kRw=="     # FlixHubKeys:VaultKey1
$VaultKey2 = "Q9@h]n3zT!mV^2Lf"              # FlixHubKeys:VaultKey2

# --- STEP 2: Restore NuGet Packages (Required for Docker build) ---
Write-Host "Step 1: Restoring NuGet packages..." -ForegroundColor Cyan
dotnet restore Application/Services/FlixHub.Api/FlixHub.Api.csproj

# --- STEP 3: Build Docker Image ---
Write-Host "Step 2: Building Docker image: ${ImageName}:${Version}..." -ForegroundColor Cyan
docker build -f Application/Services/FlixHub.Api/Dockerfile -t ${ImageName}:${Version} .

# --- STEP 4: Stop and Remove Existing Container (if exists) ---
Write-Host "Step 3: Removing existing container (if any)..." -ForegroundColor Cyan
docker stop $ContainerName 2>$null
docker rm $ContainerName 2>$null

# --- STEP 5: Run Docker Container ---
Write-Host "Step 4: Starting container: $ContainerName..." -ForegroundColor Cyan
docker run -d `
  --name $ContainerName `
  -p "${HttpPort}:80" `
  -p "${HttpsPort}:443" `
  -e ASPNETCORE_ENVIRONMENT=$Protocol `
  -e "FlixHubKeys__VaultKey1=$VaultKey1" `
  -e "FlixHubKeys__VaultKey2=$VaultKey2" `
  --restart unless-stopped `
  ${ImageName}:${Version}

# --- STEP 6: Verify Container Status ---
Write-Host "Step 5: Verifying container status..." -ForegroundColor Cyan
docker ps -a | Select-String $ContainerName

# --- STEP 7: Display Access URLs ---
Write-Host "`n==================================================" -ForegroundColor Green
Write-Host "✅ Docker container deployed successfully!" -ForegroundColor Green
Write-Host "==================================================" -ForegroundColor Green
Write-Host "Container Name: $ContainerName" -ForegroundColor Yellow
Write-Host "Image: ${ImageName}:${Version}" -ForegroundColor Yellow
Write-Host "HTTP URL:  http://localhost:$HttpPort" -ForegroundColor Yellow
Write-Host "HTTPS URL: https://localhost:$HttpsPort" -ForegroundColor Yellow
Write-Host "Swagger:   http://localhost:$HttpPort/swagger" -ForegroundColor Yellow
Write-Host "Hangfire:  http://localhost:$HttpPort/hangfire" -ForegroundColor Yellow
Write-Host "==================================================" -ForegroundColor Green

# --- STEP 8: View Logs (Optional) ---
# docker logs -f $ContainerName

```

# flixhub api docker - quick commands
```powershell
# Quick one-liner (uses default ports 2535/6979)
dotnet restore Application/Services/FlixHub.Api/FlixHub.Api.csproj; docker build -f Application/Services/FlixHub.Api/Dockerfile -t flixhub-api:latest .; docker stop flixhub-api 2>$null; docker rm flixhub-api 2>$null; docker run -d --name flixhub-api -p 2535:80 -p 6979:443 -e ASPNETCORE_ENVIRONMENT=Production -e "FlixHubKeys__VaultKey1=h8LqW9z0+DpXMg2y+Q7kRw==" -e "FlixHubKeys__VaultKey2=Q9@h]n3zT!mV^2Lf" --restart unless-stopped flixhub-api:latest

# View container logs (real-time)
docker logs -f flixhub-api

# View last 50 log lines
docker logs flixhub-api --tail 50

# Check container status
docker ps -a | Select-String flixhub-api

# Stop container
docker stop flixhub-api

# Start existing container
docker start flixhub-api

# Restart container
docker restart flixhub-api

# Remove container
docker stop flixhub-api; docker rm flixhub-api

# Remove image
docker rmi flixhub-api:latest

# Clean rebuild (removes everything)
docker stop flixhub-api 2>$null; docker rm flixhub-api 2>$null; docker rmi flixhub-api:latest 2>$null; docker builder prune -a -f; dotnet restore Application/Services/FlixHub.Api/FlixHub.Api.csproj; docker build -f Application/Services/FlixHub.Api/Dockerfile -t flixhub-api:latest .; docker run -d --name flixhub-api -p 2535:80 -p 6979:443 -e ASPNETCORE_ENVIRONMENT=Production -e "FlixHubKeys__VaultKey1=h8LqW9z0+DpXMg2y+Q7kRw==" -e "FlixHubKeys__VaultKey2=Q9@h]n3zT!mV^2Lf" --restart unless-stopped flixhub-api:latest

# Execute commands inside container
docker exec -it flixhub-api /bin/bash

# Inspect container configuration
docker inspect flixhub-api

# View container resource usage
docker stats flixhub-api --no-stream
```

# flixhub api docker - custom port examples
```powershell
# Example 1: Development with ports 5000/5001
docker run -d --name flixhub-api-dev -p 5000:80 -p 5001:443 -e ASPNETCORE_ENVIRONMENT=Development -e "FlixHubKeys__VaultKey1=h8LqW9z0+DpXMg2y+Q7kRw==" -e "FlixHubKeys__VaultKey2=Q9@h]n3zT!mV^2Lf" flixhub-api:latest

# Example 2: Staging with ports 8080/8443
docker run -d --name flixhub-api-staging -p 8080:80 -p 8443:443 -e ASPNETCORE_ENVIRONMENT=Staging -e "FlixHubKeys__VaultKey1=h8LqW9z0+DpXMg2y+Q7kRw==" -e "FlixHubKeys__VaultKey2=Q9@h]n3zT!mV^2Lf" flixhub-api:staging

# Example 3: Production with standard ports 80/443 (requires admin/sudo)
docker run -d --name flixhub-api-prod -p 80:80 -p 443:443 -e ASPNETCORE_ENVIRONMENT=Production -e "FlixHubKeys__VaultKey1=h8LqW9z0+DpXMg2y+Q7kRw==" -e "FlixHubKeys__VaultKey2=Q9@h]n3zT!mV^2Lf" flixhub-api:1.0.0

# Example 4: Multiple environments on same machine
docker run -d --name flixhub-dev -p 5000:80 -e ASPNETCORE_ENVIRONMENT=Development -e "FlixHubKeys__VaultKey1=h8LqW9z0+DpXMg2y+Q7kRw==" -e "FlixHubKeys__VaultKey2=Q9@h]n3zT!mV^2Lf" flixhub-api:latest
docker run -d --name flixhub-staging -p 6000:80 -e ASPNETCORE_ENVIRONMENT=Staging -e "FlixHubKeys__VaultKey1=h8LqW9z0+DpXMg2y+Q7kRw==" -e "FlixHubKeys__VaultKey2=Q9@h]n3zT!mV^2Lf" flixhub-api:latest
docker run -d --name flixhub-prod -p 2535:80 -e ASPNETCORE_ENVIRONMENT=Production -e "FlixHubKeys__VaultKey1=h8LqW9z0+DpXMg2y+Q7kRw==" -e "FlixHubKeys__VaultKey2=Q9@h]n3zT!mV^2Lf" flixhub-api:latest
```

# flixhub api docker - using docker-compose (alternative method)
```bash
# Using docker-compose (easier for complex setups)
# Make sure .env file exists in root with vault keys

# Build and start
docker-compose up -d --build

# Stop and remove
docker-compose down

# View logs
docker-compose logs -f flixhub-api

# Restart service
docker-compose restart flixhub-api

# Rebuild single service
docker-compose up -d --build flixhub-api
```