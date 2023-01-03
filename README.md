# challenge-open-foods

Este projeto é uma implementação do desafio proposto em:
git@lab.coodesh.com/challenge-20220626.git  (This is a challenge by Coodesh)

Para executar a aplicação é necessário apontar para uma base de dados SqlServer.
Deve ser alterado a string de conexão do banco de dados nos arquivos:

AppSettings.json e AppSettings.Development.json

Exemplo:

	De:
	
"DbConnectionString": "Server=<ENDEREÇO DO SERVIDOR>;Database=<NOME DO BANCO DE DADOS>;User Id=<USUÁRIO>;Password=<SENHA>;Persist Security Info=True;Encrypt=True;TrustServerCertificate=True;"

	Para:
	
"DbConnectionString": "Server=localhost;Database=Challenge;User Id=admin;Password=admin;Persist Security Info=True;Encrypt=True;TrustServerCertificate=True;"

No banco de dados, deve ser executado script:

	.\Coodesh.Challenge.Infrastructure\Database\Scripts\Product.sql
	
Caso deseje alterar a expressão "cron" da importação de produtos, altere a constante "cron" no arquivo \Coodesh.Challenge.API\Jobs\ImporterProductsJob.cs
(Deixei por padrão, a execução diária sempre a 1 hora da manhã)

A aplicação está configurada para rodar em Docker, basta utilizar .\Coodesh.Challenge.API\Dockerfile