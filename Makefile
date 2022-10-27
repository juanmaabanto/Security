create-testdb:
	@echo "Creando base de datos MSSQL"
	@docker build -t mssql:dev ./database
	# @docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=P@ssw0rd' -e 'MSSQL_PID=Express' --name sqlserver -p 14339:1433 -d mssql:dev

create-api:
	@echo "Creando api"
	@dotnet publish
	@docker build -t securityapi:dev ./src/Security.API
	# @docker run -e 'ASPNETCORE_ENVIRONMENT=development' --name securityapi -p 5203:80 -d securityapi:dev

up: create-testdb create-api
	@docker-compose up -d