# 📦 ApiSistemaEstoque

API REST para gerenciamento de estoques, desenvolvida em **ASP.NET Core**, utilizando **Clean Architecture**, **MediatR**, **Entity Framework Core**, **ASP.NET Identity** e **JWT** para autenticação e autorização.

---

## 🚀 Tecnologias Utilizadas

* .NET 8
* ASP.NET Core Web API
* Entity Framework Core
* ASP.NET Core Identity
* JWT (JSON Web Token)
* MediatR (CQRS)
* SQLite / SQL Server (dependendo da configuração)
* Swagger (OpenAPI)

---

## 📂 Estrutura do Projeto

A solução segue os princípios da **Clean Architecture**, separando responsabilidades:

```
ApiSistemaEstoque
│
├── ApiSistemaEstoque.API            # Controllers (camada de apresentação)
│   └── Controllers
│
├── ApiSistemaEstoque.Application    # Casos de uso / Regras de aplicação
│   ├── Handlers                     # Commands & Queries (MediatR)
│   ├── Interfaces                   # Contratos (ex: Auth, JWT)
│   └── DTOs / Requests / Responses
│
├── ApiSistemaEstoque.Domain         # Domínio puro
│   ├── Entities
│   ├── Enums
│   └── ValueObjects
│
├── ApiSistemaEstoque.Infrastructure # Infraestrutura
│   ├── Context                      # DbContexts (EF Core)
│   ├── Auth                         # Identity, JWT Services
│   ├── Repositories
│   └── Migrations
│
├── ApiSistemaEstoque.Host           # Projeto de inicialização
│   ├── Program.cs
│   └── appsettings.json
│
└── ApiSistemaEstoque.sln
```

### 🔑 Responsabilidades

| Camada         | Responsabilidade                              |
| -------------- | --------------------------------------------- |
| API            | Receber requisições HTTP e retornar respostas |
| Application    | Orquestrar regras de negócio (CQRS)           |
| Domain         | Regras de negócio puras                       |
| Infrastructure | Banco de dados, Identity, JWT                 |
| Host           | Configuração da aplicação                     |

---

## ⚙️ Pré-requisitos

* .NET SDK 8 instalado
* Git
* Visual Studio / VS Code (opcional)

Verifique:

```bash
dotnet --version
```

---

## ▶️ Como Rodar o Projeto

### 1️⃣ Clone o repositório

```bash
git clone https://github.com/seu-usuario/ApiSistemaEstoque.git
cd ApiSistemaEstoque
```

### 2️⃣ Restaurar dependências

```bash
dotnet restore
```

### 3️⃣ Executar a API

```bash
dotnet run --project ApiSistemaEstoque.Host
```

A API será iniciada em algo como:

```
http://localhost:5295
```

Swagger:

```
http://localhost:5295/swagger
```

---

## 🗄️ Banco de Dados e Migrations

O projeto utiliza **Entity Framework Core**.

### 📌 Criar uma migration

Execute **a partir da raiz da solução**:

```bash
dotnet ef migrations add UpdateTipoMovimentacao --context EstoqueContext --project ApiSistemaEstoque.Infrastructure --startup-project ApiSistemaEstoque.Host

```

### 📌 Aplicar migrations

```bash
dotnet ef database update --context EstoqueContext --project ApiSistemaEstoque.Infrastructure --startup-project ApiSistemaEstoque.Host

```

📍 As migrations ficam em:

```
ApiSistemaEstoque.Infrastructure/Migrations
```

---

## 🔐 Autenticação e Autorização

A API utiliza **JWT + ASP.NET Identity**.

### 🔑 Fluxo

1. Registrar usuário
2. Realizar login
3. Receber token JWT
4. Enviar token no header Authorization

### Exemplo de Header

```
Authorization: Bearer {seu_token_aqui}
```

### Exemplo de Login

```http
POST /api/auth/login
```

Resposta:

```json
{
  "userId": "guid",
  "email": "usuario@email.com",
  "token": "jwt_token"
}
```

---

## 📦 Endpoints Principais

### Estoques

| Método | Endpoint                        | Descrição         |
| ------ | ------------------------------- | ----------------- |
| POST   | /api/estoques                   | Cadastrar estoque |
| GET    | /api/estoques                   | Listar estoques   |
| GET    | /api/estoques/{codigo}          | Buscar por código |
| PUT    | /api/estoques/{codigo}          | Editar estoque    |
| PATCH  | /api/estoques/{codigo}/inativar | Inativar estoque  |

⚠️ Todos exigem autenticação.

---

## 🧪 Testes

Você pode testar via:

* Swagger
* Postman
* curl

---

## 🧠 Observações Importantes

* O projeto usa **MediatR** (CQRS)
* Controllers são apenas mediadores
* Toda regra fica nos Handlers
* JWT contém claims de Role

---

## 👨‍💻 Autor

Projeto desenvolvido para fins de estudo e evolução em arquitetura backend com .NET.

---

## ✅ Status

✔️ Autenticação JWT
✔️ Identity
✔️ Controle de acesso
✔️ Clean Architecture
✔️ CQRS


