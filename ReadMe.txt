ASP.NET Core 5
Database: MSSQL (localdb)

1. Before running tasks rebuild solution.
2. You need to run allTasks in Tasks Runner Explorer (View > Other Windows > Task Runner Explorer) after first checkout from source save.
	If sass does not restore, run npm rebuild node-sass in src\BaseApp.Web folder.
3. You need to create and apply all migrations (see EF docs belows for details).
	Open console (View > Other Windows > Package Manager Console) and select default project src\BaseApp.Data.ProjectMigration.
	Then run ef commands (Update-Database for example)
4. Run BaseApp.Web project
5. Open page http://localhost:38717/
6. In the right top corner click on: guest > Log On
7.1. Use for Administrator login: admin and password: 1111111
7.2. Use for DataEntryOperator login: operator and password: 1111111
8. After successful logon click on bottom of page on 'Admin panel' link
9. Here we have 'Apps' menu opened where shown apps
10. Admin user have addiction possibility to create/edit apps