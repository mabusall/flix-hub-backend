# powershell environemnt variables
```
use below commands to set vault keys prior running any
ef core database commands otherwise you will have exceptions
like add-migration, ...

$env:TasheerKeys:VaultKey1='S6kbU35+td9QKg5x+K4FTg=='
$env:TasheerKeys:VaultKey2='i7}/uiARkZ:D)xip'
```

# databse scaffolding
```
Add-Migration Store_Init -Context StoreDbContext -StartupProject Tasheer.Api -Project Store.Api -Verbose
Remove-Migration -Context StoreDbContext -StartupProject Tasheer.Api -Project Store.Api -Verbose
Update-Database -Context StoreDbContext -StartupProject Tasheer.Api -Project Store.Api -Verbose

Add-Migration OutBox_Init -Context OutBoxDbContext -StartupProject Tasheer.Api -Project Tasheer.Core -Verbose
Update-Database -Context OutBoxDbContext -StartupProject Tasheer.Api -Project Tasheer.Core -Verbose
```