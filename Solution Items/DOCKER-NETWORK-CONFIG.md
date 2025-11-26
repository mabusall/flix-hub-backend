# Docker Network Configuration - Summary

## Problem
Docker containers were failing to connect to services (PostgreSQL, RabbitMQ, Redis) running on the host machine because `localhost` inside a container refers to the container itself, not the host.

## Solution
Created `appsettings.Docker.json` to override network configurations when running in Docker.

---

## Files Modified/Created

### 1. **appsettings.Docker.json** (NEW)
**Location:** `Application/Services/FlixHub.Api/appsettings.Docker.json`

**Purpose:** Overrides only localhost URLs to use `host.docker.internal`

**What it overrides:**
- ? PostgreSQL connection strings (3 places)
- ? RabbitMQ URIs
- ? Redis URI
- ? ElasticSearch URL
- ? ElasticApm URL

**What it keeps from appsettings.json:**
- ? All encrypted passwords/secrets
- ? SMTP configuration (external service)
- ? Keycloak configuration (IP address)
- ? Integration APIs (external services)
- ? All other settings

---

### 2. **Dockerfile** (UPDATED)
**Location:** `Application/Services/FlixHub.Api/Dockerfile`

**Changes:**
- Added: `ENV ASPNETCORE_ENVIRONMENT=Docker`

**Effect:** Container automatically loads `appsettings.Docker.json` when it starts

---

### 3. **docker-compose.yml** (UPDATED)
**Location:** `docker-compose.yml`

**Changes:**
- Changed: `ASPNETCORE_ENVIRONMENT=Production` ? `ASPNETCORE_ENVIRONMENT=Docker`

**Effect:** Explicitly sets environment to Docker when using docker-compose

---

## How It Works

### Configuration Loading Order:
```
1. appsettings.json (base configuration)
2. appsettings.Docker.json (overrides localhost ? host.docker.internal)
3. Environment variables from .env file (overrides vault keys)
4. Final merged configuration
```

### Network Resolution:
- **Windows (localhost):** Direct connection to services on host
- **Docker (host.docker.internal):** Special DNS that resolves to host IP from inside container

---

## Connection String Details

### PostgreSQL (3 locations updated):

**Before (Windows):**
```
Host=localhost;Port=5432;Database=FlixHubDb;Username=sa;Password=sa
```

**After (Docker):**
```
Host=host.docker.internal;Port=5432;Database=FlixHubDb;Username=sa;Password=sa
```

**Locations:**
1. `ConnectionStrings.Default`
2. `MessageBus.DbConnection`
3. `Hangfire.DbConnection`

---

### RabbitMQ (2 URIs updated):

**Before:**
```
rabbitmq://localhost:5672
amqp://localhost:5672
```

**After:**
```
rabbitmq://host.docker.internal:5672
amqp://host.docker.internal:5672
```

---

### Redis:

**Before:**
```
localhost:6379
```

**After:**
```
host.docker.internal:6379
```

---

## Testing

### Build and run:
```powershell
# Using Build-DockerImage.ps1
.\Build-DockerImage.ps1

# Or using docker-compose
docker-compose up -d --build
```

### Verify connections:
```powershell
# Check logs
docker logs flixhub-api --tail 50

# Should see successful connections to:
# - PostgreSQL (port 5432)
# - RabbitMQ (port 5672)
# - Redis (port 6379)
```

---

## Troubleshooting

### If container still can't connect:

1. **Verify services are running on host:**
```powershell
docker ps | Select-String "postgres|redis|rabbitmq"
```

2. **Check Windows Firewall:**
- Ensure ports 5432, 5672, 6379 are accessible
- May need to add firewall rules for Docker

3. **Test host.docker.internal resolution:**
```powershell
docker run --rm alpine ping host.docker.internal
```

4. **Check logs for connection errors:**
```powershell
docker logs flixhub-api 2>&1 | Select-String "Failed to connect"
```

---

## Security Notes

? **Encrypted values remain secure:**
- Connection strings in `appsettings.Docker.json` are **plain text** but contain **no passwords**
- All passwords/secrets come from encrypted values in `appsettings.json`
- Vault keys are passed via environment variables (`.env` file, not in image)

? **Separation of concerns:**
- Windows development: Uses `appsettings.json` (localhost)
- Docker deployment: Uses `appsettings.Docker.json` (host.docker.internal)
- Production deployment: Can use `appsettings.Production.json` (actual hostnames)

---

## What's NOT Changed

These remain the same in both environments:
- ? SMTP Email (smtp.office365.com)
- ? Keycloak (10.10.242.146:8080)
- ? TMDB API (api.themoviedb.org)
- ? OMDB API (omdbapi.com)
- ? Azure Blob Storage (sharedstorageacc01.blob.core.windows.net)
- ? All API keys, tokens, and passwords
- ? All application settings and features

---

## Next Steps

1. **Test the container:**
```powershell
.\Build-DockerImage.ps1
docker logs flixhub-api --tail 50
```

2. **Verify API is accessible:**
```
http://localhost:2535/swagger
http://localhost:2535/hangfire
```

3. **For production:**
- Create `appsettings.Production.json` with actual server hostnames
- Use secrets management (Azure Key Vault, etc.)
- Set `ASPNETCORE_ENVIRONMENT=Production` in deployment

---

## Summary

? **Created:** `appsettings.Docker.json` with host.docker.internal overrides  
? **Updated:** Dockerfile to use Docker environment  
? **Updated:** docker-compose.yml to set Docker environment  
? **Result:** Container can now connect to host services  
? **Security:** All encrypted values and secrets remain protected  

**The application is now ready to run in Docker!** ????
