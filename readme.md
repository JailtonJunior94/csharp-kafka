## Sobre
O Projeto Csharp Kafka é uma PoC ([Prova de Conceito](https://pt.wikipedia.org/wiki/Prova_de_conceito#:~:text=Uma%20prova%20de%20conceito%2C%20ou,uma%20pesquisa%20ou%20artigo%20t%C3%A9cnico.)) de como podemos utilizar [Kafka](https://kafka.apache.org), [Debezium](https://debezium.io), [Zookeeper](https://zookeeper.apache.org) e [CDC](https://docs.microsoft.com/pt-br/sql/relational-databases/track-changes/about-change-data-capture-sql-server?view=sql-server-ver15) para capturar eventos no [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-2019) e enviar notificações no Slack.

## Tecnologias Utilizadas 🚀
* **[C#](https://docs.microsoft.com/pt-br/dotnet/csharp/)**
* **[Azure Functions](https://docs.microsoft.com/pt-br/azure/azure-functions/)**
* **[Worker Service](https://docs.microsoft.com/pt-br/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-5.0&tabs=visual-studio)**
* **[Apache Kafka](https://kafka.apache.org)**
* **[Zookeeper](https://zookeeper.apache.org)**
* **[Debezium](https://debezium.io)**
* **[CDC](https://docs.microsoft.com/pt-br/sql/relational-databases/track-changes/about-change-data-capture-sql-server?view=sql-server-ver15)**
*  **[SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-2019)**

## Desenho da Solução 🎨
<p align="center">
  <img src="https://ik.imagekit.io/usw9dpm4u3i/csharp-kafka_w4kjp5e5D.png" width="400" title="Main">
</p>

## Como iniciar? 
Primeiro de tudo precisamos criar o nosso container com o SQL Server através do Docker e o seguinte comando.
```
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=password' -e 'MSSQL_AGENT_ENABLED=True' -p 1433:1433 -d --name=mssql2019 mcr.microsoft.com/mssql/server:2019-latest
```
Após criação do container do SQL Server precisamos criar o banco de dados da PoC.
```
CREATE DATABASE KafkaPoC
```
Agora vamos criar a tabela Customer
```
CREATE TABLE dbo.Customers
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] VARCHAR (50) NOT NULL,
    [Email] VARCHAR (100) NOT NULL,
    [CreatedAt] DATETIME NOT NULL,
    [UpdatedAt] DATETIME NULL,
    [Active] BIT NOT NULL
)
```
Para que o Kafka, Zookeeper e Debezium funcione do jeito que precisamos é preciso habilitar o CDC (Change Data Capture) com os seguintes comandos.
```
EXEC sys.sp_cdc_enable_db
```
Após habilitar o CDC no nosso banco de dados SQL Server, vamos habilitar para que a tabela Customer escute todos os eventos do banco.
```
EXEC sys.sp_cdc_enable_table
@source_schema = N'dbo',
@source_name   = N'Customers',
@role_name     = N'Admin',
@supports_net_changes = 1
```
Feito isso vamos iniciar a nossa infra com o arquivo docker-compose.yml que está no projeto
```
docker-compose up
```
Precisamos configurar os nossos conectores, para isso vamos utilizar a API do Debezium.<br>
Para obter os conectores devemos fazer uma requisição GET na seguinte URL.
```
http://{seuip}:8083/connector-plugins
```
Resultado: 
```
[
    {
        "class": "io.debezium.connector.sqlserver.SqlServerConnector",
        "type": "source",
        "version": "1.2.5.Final"
    },
]
```
Para configurar o nosso conector entre o SQL Server e o Kafka precisamos fazer uma requisição do tipo POST na seguinte URL.
```
http://seuip:8083/connectors

{
	"name": "sqlserver-customers-connector",
	"config": {
    "name":"sqlserver-customers-connector",
		"connector.class": "io.debezium.connector.sqlserver.SqlServerConnector",
    "table.whitelist": "dbo.Customers",
		"database.hostname": "{seuip}",
		"database.port": "1433",
		"database.user": "sa",
		"database.password": "{password}",
		"database.dbname": "KafkaPoC",
		"database.server.name": "dbserver",
		"database.history.kafka.bootstrap.servers": "{seuip}:9092",
		"database.history.kafka.topic": "dbhistory.customers"
	}
}
```
Collection (Postman) - Debezium
```
https://www.getpostman.com/collections/4e9eb39b472f14431938
```

Depois de configurado toda infra do projeto, podemos testar as notificações atráves das operações CRUD que foram criadas utilizando Azure Function:

Collection (Postman) - Azure Functions
```
https://www.getpostman.com/collections/a8bda617f739c1321475
```
## Swagger
<p align="center">
  <img src="https://ik.imagekit.io/usw9dpm4u3i/Swagger_imGNEGfPr0.PNG" width="400" title="Main">
</p>

## Slack
<p align="center">
  <img src="https://ik.imagekit.io/usw9dpm4u3i/Slack_hktg7ZB97w.PNG" width="400" title="Main">
</p>