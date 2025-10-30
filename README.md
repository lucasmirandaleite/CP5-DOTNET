# 🚀 CP5 - Biblioteca API com MongoDB, Health Check e Versionamento

Este projeto é a evolução do CP4, aplicando conceitos avançados de desenvolvimento de APIs com .NET 8, incluindo a migração para **MongoDB**, implementação de **Health Checks** e **versionamento de API**.

**Integrantes do Grupo:**
- Lucas Miranda Leite RM:555161
- Gusthavo Daniel de Souza RM:554681
- Guilherme Damasio Roselli RM:555873

## ✨ Novidades do CP5

- **Migração para MongoDB**: A persistência de dados foi totalmente migrada de SQLite (com Entity Framework Core) para **MongoDB**, um banco de dados NoSQL orientado a documentos. Isso torna a aplicação mais escalável e flexível.
- **Health Check**: Um endpoint `/health` foi adicionado para monitorar a saúde da aplicação e de suas dependências, como a conexão com o MongoDB.
- **Versionamento da API**: A API agora suporta versionamento (v1), permitindo evoluções futuras sem quebrar a compatibilidade com clientes existentes. A documentação do Swagger foi atualizada para refletir isso.
- **Manutenção da Arquitetura**: A estrutura baseada em **Clean Architecture** e **Domain-Driven Design (DDD)** foi mantida e aprimorada, garantindo baixo acoplamento e alta coesão.

## 🏗️ Arquitetura

O projeto mantém a estrutura de **Clean Architecture** com as seguintes camadas:

```
📦 src
 ┣ 📂 Api             -> Controllers, validações, Swagger, Health Check
 ┣ 📂 Application     -> Casos de uso, DTOs, lógica de aplicação
 ┣ 📂 Domain          -> Entidades, Value Objects, Interfaces de repositório
 ┗ 📂 Infrastructure  -> Acesso a dados (MongoDB), implementação de repositórios
```

## 🛠️ Tecnologias Utilizadas

- **.NET 8** - Framework principal
- **MongoDB** - Banco de dados NoSQL para persistência
- **MongoDB.Driver** - Driver oficial para comunicação com o MongoDB
- **Swagger/OpenAPI** - Documentação da API (agora com versionamento)
- **ASP.NET Core Health Checks** - Para monitoramento da aplicação
- **ASP.NET Core** - Framework web

## 🚀 Como Executar

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop/) (recomendado para rodar o MongoDB)
- Git (para clonar o repositório)

### Passo a Passo

1. **Clone o repositório**
   ```bash
   git clone <url-do-repositorio-cp5>
   cd cp5-project
   ```

2. **Inicie o MongoDB com Docker**
   ```bash
   docker run --name mongodb-biblioteca -d -p 27017:27017 mongo
   ```
   A connection string no `appsettings.json` já está configurada para `mongodb://localhost:27017`.

3. **Acesse a pasta da API**
   ```bash
   cd src/Api
   ```

4. **Compile o projeto**
   ```bash
   dotnet build
   ```

5. **Restaure as dependências**
   ```bash
   dotnet restore
   ```

6. **Execute a aplicação**
   ```bash
   dotnet run
   ```

7. **Acesse a documentação Swagger**

   Abra seu navegador e acesse:  
   `http://localhost:5001`

   A interface do Swagger será exibida automaticamente com todos os endpoints da **v1** da API.

## ❤️ Health Check

Para verificar a saúde da aplicação e a conexão com o MongoDB, acesse o endpoint:

`http://localhost:5001/health`

Você receberá um JSON com o status `Healthy` se tudo estiver funcionando corretamente.

## 📦 Persistência com MongoDB

- **Code First**: O modelo de dados continua sendo definido via código C# nas entidades do domínio.
- **BSON Attributes**: As entidades foram decoradas com atributos como `[BsonId]`, `[BsonElement]` e `[BsonIgnore]` para controlar a serialização para o formato BSON do MongoDB.
- **MongoDbContext**: Uma classe de contexto foi criada para gerenciar o acesso às coleções do MongoDB (`Usuarios`, `Livros`, `Emprestimos`).
- **Repositórios**: As implementações dos repositórios na camada de `Infrastructure` foram reescritas para usar o `MongoDB.Driver`, substituindo o Entity Framework Core.

Desenvolvido com as melhores práticas para criar APIs robustas, escaláveis e observáveis com .NET.
