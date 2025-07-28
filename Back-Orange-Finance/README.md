# Back-End-Orange-Finance


## Rodar comandos para aplicar migration

- Precisa estar dentro do projeto infraestrutura
 
- Adicionar migration
 `dotnet ef migrations add WebhookInitialMigration`
- Criar banco de dados com as migrations
 `dotnet ef database update`