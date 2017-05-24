## Startup manual:
Before running tasks **rebuild** solution.<br/>
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

## WebApi docs:
Convention: All non-get methods with one parameter binds FromBody (or FromForm if IFormFile presents). See ApiControllerConvention for details <br/>
To view help pages use /swagger url)