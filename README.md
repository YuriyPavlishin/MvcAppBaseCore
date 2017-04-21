## Startup manual:
Before running tasks rebuild solution.<br/>
You need to run allTasks in Tasks Runner Explorer (*View > Other Windows > Task Runner Explorer*) after first checkout from source save.<br/> 
If sass does not restore, run **npm rebuild node-sass** in *src\BaseApp.Web folder*.<br/>
You need to create and apply all migrations (see EF docs belows for details)

## EF docs:
For migrations use EF Core Package Manager Console:<br/>
Open console (*View > Other Windows > Package Manager Console*) and select default project *src\BaseApp.Data.ProjectMigration*. Then run ef commands (**Update-Database** for example)<br/>
For more details visit: [powershell docs](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/powershell) 
<br/><br/>
If you prefer cmd way you need to open console in *src\BaseApp.Web folder*:<br/>
To create new migration: *dotnet ef --project ../BaseApp.Data.ProjectMigration --startup-project . migrations add migrationName*<br/>
To apply migrations (also create DB if it does not exists):  *dotnet ef --project ../BaseApp.Data.ProjectMigration --startup-project . database update*

## TODO:
WebApi binding (in progress, to view help pages use /swagger/ui url)<br/>
Logging (apply [nlog 4.3 release](https://github.com/NLog/NLog.Web/issues/53) renderes<br/>
todo marked in code<br/>
Documentation
