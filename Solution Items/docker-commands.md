# Docker Commands for FlixHub API

## Build and Run Commands

### Build Docker Image
```bash
# From repository root directory
docker build -f Application/Services/FlixHub.Api/Dockerfile -t flixhub-api:latest .
```

### Run Container
```bash
# Basic run
docker run -d -p 8080:80 --name flixhub-api flixhub-api:latest

# Run with environment variables
docker run -d -p 8080:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e ConnectionStrings__DefaultConnection="your-connection-string" \
  -e TMDB__ApiKey="your-tmdb-api-key" \
  -e OMDB__ApiKey="your-omdb-api-key" \
  --name flixhub-api \
  flixhub-api:latest
```

### Docker Compose Commands
```bash
# Start all services
docker-compose up -d

# Start and rebuild
docker-compose up -d --build

# View logs
docker-compose logs -f flixhub-api

# Stop all services
docker-compose down

# Stop and remove volumes
docker-compose down -v
```

## Container Management

### View Running Containers
```bash
docker ps
```

### View Container Logs
```bash
docker logs flixhub-api
docker logs -f flixhub-api  # Follow logs
```

### Stop Container
```bash
docker stop flixhub-api
```

### Remove Container
```bash
docker rm flixhub-api
```

### Remove Image
```bash
docker rmi flixhub-api:latest
```

### Execute Commands in Container
```bash
# Open shell in running container
docker exec -it flixhub-api /bin/bash

# Run dotnet command
docker exec -it flixhub-api dotnet --version
```

## Environment Variables

Configure these environment variables for production:

- `ASPNETCORE_ENVIRONMENT` - Set to Production, Staging, or Development
- `ConnectionStrings__DefaultConnection` - Database connection string
- `TMDB__ApiKey` - The Movie Database API key
- `OMDB__ApiKey` - Open Movie Database API key
- `Keycloak__*` - Keycloak configuration settings
- `Hangfire__*` - Hangfire configuration settings

## Publishing to Docker Hub

```bash
# Login to Docker Hub
docker login

# Tag image
docker tag flixhub-api:latest yourusername/flixhub-api:latest
docker tag flixhub-api:latest yourusername/flixhub-api:1.0.0

# Push to Docker Hub
docker push yourusername/flixhub-api:latest
docker push yourusername/flixhub-api:1.0.0
```

## Production Considerations

1. **Use HTTPS**: Configure SSL certificates for production
2. **Environment Variables**: Use Docker secrets or environment files for sensitive data
3. **Health Checks**: Add health check endpoints to your API
4. **Logging**: Configure proper logging to external systems
5. **Resource Limits**: Set memory and CPU limits in docker-compose.yml
6. **Database Migrations**: Run migrations before starting the application
7. **Monitoring**: Integrate with monitoring tools like Prometheus/Grafana

## Example Production docker-compose.yml snippet

```yaml
services:
  flixhub-api:
    deploy:
      resources:
        limits:
          cpus: '2'
          memory: 2G
        reservations:
          cpus: '1'
          memory: 1G
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:80/health"]
      interval: 30s
      timeout: 10s
      retries: 3
      start_period: 40s
```
