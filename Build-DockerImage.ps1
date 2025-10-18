#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Builds and deploys FlixHub API Docker container with flexible configuration.

.DESCRIPTION
    This script automates the Docker build and deployment process for FlixHub API.
    It supports customizable image names, versions, ports, and environment settings.

.PARAMETER ImageName
    Docker image name (default: "flixhub-api")

.PARAMETER Version
    Image version/tag (default: "latest")

.PARAMETER ContainerName
    Container name (default: "flixhub-api")

.PARAMETER HttpPort
    HTTP port on host (default: 2535)

.PARAMETER HttpsPort
    HTTPS port on host (default: 6979)

.PARAMETER Environment
    ASPNETCORE_ENVIRONMENT (default: "Production")

.PARAMETER VaultKey1
    FlixHubKeys:VaultKey1 encryption key

.PARAMETER VaultKey2
    FlixHubKeys:VaultKey2 encryption key

.PARAMETER BuildOnly
    Only build the image without running container

.PARAMETER SkipRestore
    Skip NuGet package restore step

.PARAMETER CleanBuild
    Remove existing images and containers before building

.EXAMPLE
    .\Build-DockerImage.ps1
    Builds with default settings

.EXAMPLE
    .\Build-DockerImage.ps1 -Version "1.0.0" -Environment "Staging" -HttpPort 8080
    Builds version 1.0.0 for staging environment on port 8080

.EXAMPLE
    .\Build-DockerImage.ps1 -BuildOnly
    Only builds the image without running container

.EXAMPLE
    .\Build-DockerImage.ps1 -CleanBuild
    Performs a clean build removing all existing images and containers
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory=$false)]
    [string]$ImageName = "flixhub-api",
    
    [Parameter(Mandatory=$false)]
    [string]$Version = "latest",
    
    [Parameter(Mandatory=$false)]
    [string]$ContainerName = "flixhub-api",
    
    [Parameter(Mandatory=$false)]
    [int]$HttpPort = 2535,
    
    [Parameter(Mandatory=$false)]
    [int]$HttpsPort = 6979,
    
    [Parameter(Mandatory=$false)]
    [ValidateSet("Development", "Staging", "Production")]
    [string]$Environment = "Docker",
    
    [Parameter(Mandatory=$false)]
    [string]$VaultKey1 = "h8LqW9z0+DpXMg2y+Q7kRw==",
    
    [Parameter(Mandatory=$false)]
    [string]$VaultKey2 = "Q9@h]n3zT!mV^2Lf",
    
    [Parameter(Mandatory=$false)]
    [switch]$BuildOnly,
    
    [Parameter(Mandatory=$false)]
    [switch]$SkipRestore,
    
    [Parameter(Mandatory=$false)]
    [switch]$CleanBuild
)

# ============================================================================
# CONFIGURATION
# ============================================================================

$ErrorActionPreference = "Stop"
$ProjectPath = "Application/Services/FlixHub.Api/FlixHub.Api.csproj"
$DockerfilePath = "Application/Services/FlixHub.Api/Dockerfile"
$FullImageName = "${ImageName}:${Version}"

# ============================================================================
# FUNCTIONS
# ============================================================================

function Write-Step {
    param([string]$Message)
    Write-Host "`n$Message" -ForegroundColor Cyan
}

function Write-Success {
    param([string]$Message)
    Write-Host "? $Message" -ForegroundColor Green
}

function Write-Error {
    param([string]$Message)
    Write-Host "? $Message" -ForegroundColor Red
}

function Write-Warning {
    param([string]$Message)
    Write-Host "??  $Message" -ForegroundColor Yellow
}

function Test-Command {
    param([string]$Command)
    $null = Get-Command $Command -ErrorAction SilentlyContinue
    return $?
}

function Stop-Container {
    param([string]$Name)
    $container = docker ps -a --filter "name=$Name" --format "{{.Names}}" 2>$null
    if ($container) {
        Write-Host "  Stopping container: $Name..." -ForegroundColor Gray
        docker stop $Name 2>$null | Out-Null
        docker rm $Name 2>$null | Out-Null
        Write-Success "Container removed: $Name"
    }
}

function Remove-Image {
    param([string]$Name)
    $image = docker images --filter "reference=$Name" --format "{{.Repository}}:{{.Tag}}" 2>$null
    if ($image) {
        Write-Host "  Removing image: $Name..." -ForegroundColor Gray
        docker rmi $Name 2>$null | Out-Null
        Write-Success "Image removed: $Name"
    }
}

# ============================================================================
# VALIDATION
# ============================================================================

Write-Host @"

??????????????????????????????????????????????????????????????????
?                                                                ?
?           FlixHub API Docker Build & Deployment                ?
?                                                                ?
??????????????????????????????????????????????????????????????????

"@ -ForegroundColor Cyan

# Check if Docker is installed
if (-not (Test-Command "docker")) {
    Write-Error "Docker is not installed or not in PATH"
    exit 1
}

# Check if dotnet is installed
if (-not (Test-Command "dotnet")) {
    Write-Error ".NET SDK is not installed or not in PATH"
    exit 1
}

# Check if we're in the correct directory
if (-not (Test-Path $ProjectPath)) {
    Write-Error "Project file not found: $ProjectPath"
    Write-Host "Please run this script from the repository root directory" -ForegroundColor Yellow
    exit 1
}

# ============================================================================
# CLEAN BUILD (Optional)
# ============================================================================

if ($CleanBuild) {
    Write-Step "?? Performing clean build..."
    
    Stop-Container -Name $ContainerName
    Remove-Image -Name $FullImageName
    
    Write-Host "  Pruning Docker build cache..." -ForegroundColor Gray
    docker builder prune -a -f | Out-Null
    Write-Success "Clean build preparation completed"
}

# ============================================================================
# STEP 1: RESTORE NUGET PACKAGES
# ============================================================================

if (-not $SkipRestore) {
    Write-Step "?? Step 1: Restoring NuGet packages..."
    
    try {
        dotnet restore $ProjectPath
        if ($LASTEXITCODE -eq 0) {
            Write-Success "NuGet packages restored successfully"
        } else {
            throw "dotnet restore failed with exit code $LASTEXITCODE"
        }
    }
    catch {
        Write-Error "Failed to restore NuGet packages: $_"
        exit 1
    }
} else {
    Write-Warning "Skipping NuGet restore (--SkipRestore flag)"
}

# ============================================================================
# STEP 2: BUILD DOCKER IMAGE
# ============================================================================

Write-Step "?? Step 2: Building Docker image: $FullImageName..."

try {
    $buildStart = Get-Date
    docker build -f $DockerfilePath -t $FullImageName .
    
    if ($LASTEXITCODE -eq 0) {
        $buildDuration = (Get-Date) - $buildStart
        Write-Success "Docker image built successfully in $($buildDuration.ToString('mm\:ss'))"
    } else {
        throw "docker build failed with exit code $LASTEXITCODE"
    }
}
catch {
    Write-Error "Failed to build Docker image: $_"
    exit 1
}

# ============================================================================
# STEP 3: RUN CONTAINER (Optional)
# ============================================================================

if ($BuildOnly) {
    Write-Warning "Build-only mode: Skipping container deployment"
    Write-Host "`n????????????????????????????????????????????????????????????" -ForegroundColor Green
    Write-Success "Docker image built successfully: $FullImageName"
    Write-Host "????????????????????????????????????????????????????????????`n" -ForegroundColor Green
    exit 0
}

Write-Step "?? Step 3: Deploying container: $ContainerName..."

# Stop and remove existing container
Stop-Container -Name $ContainerName

# Run new container
try {
    docker run -d `
        --name $ContainerName `
        -p "${HttpPort}:8080" `
        -p "${HttpsPort}:8081" `
        -e ASPNETCORE_ENVIRONMENT=$Environment `
        -e "FlixHubKeys__VaultKey1=$VaultKey1" `
        -e "FlixHubKeys__VaultKey2=$VaultKey2" `
        --restart unless-stopped `
        $FullImageName | Out-Null
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "Container started successfully"
    } else {
        throw "docker run failed with exit code $LASTEXITCODE"
    }
}
catch {
    Write-Error "Failed to start container: $_"
    exit 1
}

# ============================================================================
# STEP 4: VERIFY CONTAINER STATUS
# ============================================================================

Write-Step "?? Step 4: Verifying container status..."

Start-Sleep -Seconds 2

$containerStatus = docker ps --filter "name=$ContainerName" --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"

if ($containerStatus) {
    Write-Host $containerStatus -ForegroundColor White
    Write-Success "Container is running"
} else {
    Write-Warning "Container may have failed to start. Checking logs..."
    docker logs $ContainerName --tail 20
    exit 1
}

# ============================================================================
# FINAL SUMMARY
# ============================================================================

Write-Host @"

??????????????????????????????????????????????????????????????????
?                                                                ?
?              ? DEPLOYMENT SUCCESSFUL                          ?
?                                                                ?
??????????????????????????????????????????????????????????????????

?? Configuration:
   • Container Name:  $ContainerName
   • Image:           $FullImageName
   • Environment:     $Environment
   • HTTP Port:       $HttpPort (? 8080)
   • HTTPS Port:      $HttpsPort (? 8081)

?? Access URLs:
   • HTTP:      http://localhost:$HttpPort
   • HTTPS:     https://localhost:$HttpsPort
   • Swagger:   http://localhost:$HttpPort/swagger
   • Hangfire:  http://localhost:$HttpPort/hangfire

?? Quick Commands:
   • View logs:       docker logs -f $ContainerName
   • Stop container:  docker stop $ContainerName
   • Start container: docker start $ContainerName
   • Restart:         docker restart $ContainerName

??????????????????????????????????????????????????????????????????

"@ -ForegroundColor Green

exit 0
