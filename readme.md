```
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=password' -e 'MSSQL_AGENT_ENABLED=True' -p 1433:1433 -d --name=mssql2019 mcr.microsoft.com/mssql/server:2019-latest
```