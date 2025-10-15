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