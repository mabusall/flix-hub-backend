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
Add-Migration Store_Init -Context StoreDbContext -StartupProject FlixHub.Api -Project Store.Api -Verbose
Remove-Migration -Context StoreDbContext -StartupProject FlixHub.Api -Project Store.Api -Verbose
Update-Database -Context StoreDbContext -StartupProject FlixHub.Api -Project Store.Api -Verbose

Add-Migration OutBox_Init -Context OutBoxDbContext -StartupProject FlixHub.Api -Project FlixHub.Core -Verbose
Update-Database -Context OutBoxDbContext -StartupProject FlixHub.Api -Project FlixHub.Core -Verbose
```