Before start:
You need to run allTasks in Tasks Runner Explorer (View > Other Windows > Task Runner Explorer) after first checkout from source save. Before running tasks rebuild solution.
If sass does not restore, run (npm rebuild node-sass) in src\BaseApp.Web folder.
You need to create and apply all migrations (see EF docs belows for details)

EF docs:
For migrations use EF Core Package Manager Console:
Open console (View > Other Windows > Package Manager Console) and select default project "src\BaseApp.Data.ProjectMigration". Then run ef commands (Update-Database for example)
For more details visit: https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/powershell

If you prefer cmd way you need to open console in src\BaseApp.Web folder:
To create new migration: dotnet ef --project ../BaseApp.Data.ProjectMigration --startup-project . migrations add migrationName
To apply migrations (also create DB if it does not exists):  dotnet ef --project ../BaseApp.Data.ProjectMigration --startup-project . database update

TODO:
WebApi binding (in progress, to view help pages use /swagger/ui url)
Logging (apply nlog 4.3 release renderes which specified here https://github.com/NLog/NLog.Web/issues/53)
todo marked in code 
Documentation