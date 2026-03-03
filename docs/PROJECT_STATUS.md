# 📖 Amar & Servir - Documentação do Projeto

> **Última atualização:** Gerado automaticamente  
> **Repositório:** [AmarEServir no GitHub](https://github.com/lucianop-bs/AmarEServir)  
> **Branch principal:** `main`

---

## 📋 Índice

1. [Visão Geral](#-visão-geral)
2. [Arquitetura do Sistema](#-arquitetura-do-sistema)
3. [Stack Tecnológica](#-stack-tecnológica)
4. [Estrutura do Backend](#-estrutura-do-backend)
5. [Estrutura do Frontend](#-estrutura-do-frontend)
6. [Endpoints da API](#-endpoints-da-api)
7. [Modelos de Domínio](#-modelos-de-domínio)
8. [Testes](#-testes)
9. [Status do Desenvolvimento](#-status-do-desenvolvimento)
10. [Bugs Conhecidos](#-bugs-conhecidos)
11. [Próximos Passos](#-próximos-passos)
12. [Como Executar](#-como-executar)

---

## 🎯 Visão Geral

O **Amar & Servir** é um sistema de gerenciamento de células (grupos de estudo/comunidade) para organizações religiosas ou comunitárias. O sistema permite:

- **Autenticação segura** com JWT e Refresh Tokens
- **Gerenciamento de usuários** com diferentes roles (Admin, Líder, Membro)
- **Gerenciamento de células** com líderes e membros associados
- **Interface web responsiva** para acesso via navegador

### Contexto de Negócio

O sistema foi projetado para facilitar a organização de grupos comunitários, onde:
- **Células** são pequenos grupos de pessoas que se reúnem regularmente
- Cada célula possui um **Líder** responsável
- **Membros** podem participar de uma ou mais células
- **Administradores** têm controle total sobre o sistema

---

## 🏗 Arquitetura do Sistema

O projeto segue uma arquitetura de **microsserviços** com separação clara entre frontend e backend:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                            AMAR & SERVIR                                     │
│                                                                              │
│    ┌──────────────────────┐              ┌──────────────────────┐           │
│    │                      │              │                      │           │
│    │   🌐 FRONTEND        │    HTTP/S    │   ⚙️ BACKEND         │           │
│    │   Angular 21         │◄────────────►│   .NET 9             │           │
│    │   (SPA)              │    JSON      │   (REST API)         │           │
│    │                      │    + JWT     │                      │           │
│    └──────────────────────┘              └───────────┬──────────┘           │
│                                                      │                       │
│                                                      │                       │
│                                          ┌───────────▼──────────┐           │
│                                          │                      │           │
│                                          │   🗄️ MongoDB         │           │
│                                          │   (NoSQL Database)   │           │
│                                          │                      │           │
│                                          └──────────────────────┘           │
│                                                                              │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Padrões Arquiteturais Utilizados

| Padrão | Onde é Aplicado | Benefício |
|--------|-----------------|-----------|
| **Clean Architecture** | Backend | Separação de responsabilidades, testabilidade |
| **CQRS** | Application Layer | Separação de leitura/escrita, escalabilidade |
| **Repository Pattern** | Infrastructure | Abstração do acesso a dados |
| **Result Pattern** | Core | Tratamento de erros sem exceções |
| **Mediator (MediatR)** | Application | Desacoplamento entre controllers e handlers |

---

## 🛠 Stack Tecnológica

### Backend (amareservir-api)

| Tecnologia | Versão | Propósito |
|------------|--------|-----------|
| **.NET** | 9.0 | Framework principal |
| **ASP.NET Core** | 9.0 | Web API |
| **MongoDB.Driver** | Latest | Conexão com banco de dados |
| **MediatR** | Latest | Implementação do padrão Mediator/CQRS |
| **FluentValidation** | Latest | Validação de commands/queries |
| **BCrypt.Net** | Latest | Hash de senhas |
| **JWT Bearer** | Latest | Autenticação via tokens |

### Frontend (amarservir-app)

| Tecnologia | Versão | Propósito |
|------------|--------|-----------|
| **Angular** | 21.1.0 | Framework SPA |
| **TypeScript** | 5.9.2 | Linguagem principal |
| **RxJS** | 7.8.0 | Programação reativa |
| **ngx-toastr** | 20.0.4 | Notificações toast |
| **jwt-decode** | 4.0.0 | Decodificação de tokens JWT |

### Banco de Dados

| Tecnologia | Propósito |
|------------|-----------|
| **MongoDB** | Banco NoSQL para persistência |
| **Database:** | `AmarEServirDB` |

### Ferramentas de Teste (Backend)

| Tecnologia | Propósito |
|------------|-----------|
| **NUnit** | Framework de testes |
| **Moq** | Mocking de dependências |
| **FluentAssertions** | Assertions mais legíveis |
| **Bogus** | Geração de dados fake |

---

## 📁 Estrutura do Backend

O backend segue a **Clean Architecture** com as seguintes camadas:

```
backend/
├── Auth.API/                          # Projeto principal da API
│   ├── Api/                           # 🌐 Camada de Apresentação
│   │   ├── Controllers/               # Controllers REST
│   │   │   ├── AuthController.cs      # Autenticação (login, refresh, me)
│   │   │   ├── UserController.cs      # CRUD de usuários
│   │   │   └── CellsController.cs     # CRUD de células
│   │   └── Configurations/            # Configurações da API
│   │       ├── ApiConfig.cs           # Configuração geral
│   │       ├── AuthConfig.cs          # Configuração de autenticação
│   │       └── JwtSettings.cs         # Settings do JWT
│   │
│   ├── Application/                   # 📋 Camada de Aplicação (Use Cases)
│   │   ├── Auth/                      # Casos de uso de autenticação
│   │   │   ├── Login/                 # Command + Handler + Validator
│   │   │   └── Refresh/               # Refresh token
│   │   ├── Users/                     # Casos de uso de usuários
│   │   │   ├── CreateUser/            # Criar usuário
│   │   │   ├── UpdateUser/            # Atualizar usuário
│   │   │   ├── DeleteUser/            # Deletar usuário
│   │   │   ├── GetUserByGuid/         # Buscar usuário por ID
│   │   │   ├── Models/                # ViewModels/DTOs
│   │   │   └── Common/Validators/     # Validadores compartilhados
│   │   ├── Cells/                     # Casos de uso de células
│   │   │   ├── CreateCell/            # Criar célula
│   │   │   ├── UpdateCell/            # Atualizar célula
│   │   │   ├── DeleteCell/            # Deletar célula
│   │   │   ├── GetCellByGuid/         # Buscar célula por ID
│   │   │   └── Contracts/             # DTOs de células
│   │   ├── Services/                  # Serviços de aplicação
│   │   │   ├── IJwtTokenService.cs    # Interface do serviço JWT
│   │   │   ├── JwtTokenService.cs     # Implementação JWT
│   │   │   ├── ICurrentUserService.cs # Interface usuário atual
│   │   │   └── CurrentUserService.cs  # Implementação
│   │   └── Common/                    # Comportamentos compartilhados
│   │       └── ValidateBehavior.cs    # Pipeline de validação MediatR
│   │
│   ├── Domain/                        # 🏛️ Camada de Domínio (Entidades)
│   │   ├── User.cs                    # Entidade Usuário
│   │   ├── Cell.cs                    # Entidade Célula
│   │   ├── Address.cs                 # Value Object Endereço
│   │   ├── RefreshToken.cs            # Entidade RefreshToken
│   │   ├── Enums/                     
│   │   │   └── UserRole.cs            # Enum de roles
│   │   ├── Errors/                    # Definição de erros do domínio
│   │   │   ├── UserErrors.cs          
│   │   │   ├── CellError.cs           
│   │   │   └── AuthErrors.cs          
│   │   └── Contracts/                 # Interfaces de repositório
│   │       ├── IUserRepository.cs     
│   │       └── ICellRepository.cs     
│   │
│   ├── Infrastructure/                # 🔧 Camada de Infraestrutura
│   │   ├── Persistence/               
│   │   │   ├── Context/               
│   │   │   │   └── MongoContext.cs    # Contexto do MongoDB
│   │   │   ├── Repositories/          
│   │   │   │   ├── UserRepository.cs  
│   │   │   │   └── CellRepository.cs  
│   │   │   └── Mapping/               
│   │   │       └── MongoDbMapping.cs  # Mapeamentos do MongoDB
│   │   └── DependencyInjection.cs     # Registro de serviços
│   │
│   ├── Program.cs                     # Entry point da aplicação
│   └── .env                           # Variáveis de ambiente
│
├── Core/                              # 🧱 Shared Kernel
│   ├── Entities/                      
│   │   └── BaseEntity.cs              # Entidade base com Id
│   ├── Results/                       # Result Pattern
│   │   ├── Base/                      
│   │   │   ├── Result.cs              # Result genérico
│   │   │   └── ResultBase.cs          # Base abstrata
│   │   ├── Api/                       
│   │   │   ├── ApiResult.cs           # Result para API
│   │   │   ├── ApiError.cs            
│   │   │   └── ApiInfo.cs             
│   │   ├── Errors/                    
│   │   │   ├── Error.cs               # Classe de erro
│   │   │   ├── ErrorType.cs           # Enum de tipos de erro
│   │   │   └── IError.cs              # Interface
│   │   └── Extensions/                
│   │       ├── ResultExtensions.cs    
│   │       ├── ResultValidationExtension.cs
│   │       └── ApiResultExtension.cs  
│   ├── Filters/                       
│   │   └── ApiResultFilter.cs         # Filter para respostas
│   ├── Middlewares/                   
│   │   └── GlobalExceptionHandler.cs  # Handler global de exceções
│   └── Json/Converter/                
│       └── NullableGuidConverter.cs   # Conversor JSON
│
└── tests/                             # 🧪 Testes
    └── AmarEServir.UnitTests/         
        └── Application/Users/         
            └── GetUserByIdHandlerTests.cs
```

### Explicação das Camadas

#### 🌐 Api (Presentation Layer)
Responsável por receber as requisições HTTP e retornar respostas. Os controllers são "magros" - apenas recebem a requisição, enviam para o MediatR e retornam o resultado.

#### 📋 Application (Use Cases)
Contém toda a lógica de aplicação organizada em Commands (escrita) e Queries (leitura). Cada caso de uso possui:
- **Command/Query**: DTO de entrada
- **Handler**: Lógica de execução
- **Validator**: Validação com FluentValidation

#### 🏛️ Domain (Business Logic)
Coração do sistema com entidades ricas que encapsulam regras de negócio. As entidades possuem:
- Propriedades privadas com setters protegidos
- Métodos de validação internos
- Factory methods para criação (`Create()`)

#### 🔧 Infrastructure (External Concerns)
Implementações concretas de repositórios e integrações externas. Dependências:
- MongoDB Driver
- Mapeamentos de persistência

#### 🧱 Core (Shared Kernel)
Código compartilhado entre projetos:
- **Result Pattern**: Evita exceções para fluxo de controle
- **BaseEntity**: Classe base para entidades
- **Error handling**: Sistema padronizado de erros

---

## 🌐 Estrutura do Frontend

```
amarservir-app/
├── src/
│   ├── app/
│   │   ├── core/                      # 🧱 Núcleo da aplicação
│   │   │   ├── components/            # Componentes compartilhados
│   │   │   │   └── navbar/            # Barra de navegação
│   │   │   │       ├── navbar.ts      
│   │   │   │       ├── navbar.html    
│   │   │   │       └── navbar.css     
│   │   │   │
│   │   │   ├── guards/                # Route Guards
│   │   │   │   ├── auth-guard.ts      # Protege rotas autenticadas
│   │   │   │   └── guest-guard.ts     # Protege rotas de visitantes
│   │   │   │
│   │   │   ├── interceptors/          # HTTP Interceptors
│   │   │   │   ├── auth-interceptor.ts    # Adiciona token nas requisições
│   │   │   │   └── error-interceptor.ts   # Trata erros HTTP globalmente
│   │   │   │
│   │   │   ├── layouts/               # Layouts da aplicação
│   │   │   │   └── main-layout/       # Layout principal (com navbar)
│   │   │   │
│   │   │   ├── models/                # Interfaces/Types
│   │   │   │   ├── auth/              
│   │   │   │   │   ├── auth.model.ts      # LoginRequest, LoginResponse
│   │   │   │   │   └── register.model.ts  # RegisterRequest
│   │   │   │   └── user/              
│   │   │   │       └── user.model.ts      # User interface
│   │   │   │
│   │   │   └── services/              # Serviços Angular
│   │   │       ├── auth/              
│   │   │       │   ├── auth.service.ts        # Login, Logout, Refresh
│   │   │       │   └── register.service.ts    # Cadastro
│   │   │       ├── user/              
│   │   │       │   └── user.service.ts        # Operações de usuário
│   │   │       └── notification/      
│   │   │           └── notification.service.ts # Toasts
│   │   │
│   │   ├── pages/                     # 📄 Páginas da aplicação
│   │   │   ├── auth/                  # Páginas de autenticação
│   │   │   │   ├── login/             
│   │   │   │   │   ├── login.ts       
│   │   │   │   │   ├── login.html     
│   │   │   │   │   └── login.css      
│   │   │   │   └── cadastro/          
│   │   │   │       ├── register.ts    
│   │   │   │       ├── register.html  
│   │   │   │       └── register.css   
│   │   │   │
│   │   │   └── home/                  # Página inicial
│   │   │       ├── home.ts            
│   │   │       ├── home.html          
│   │   │       └── home.css           
│   │   │
│   │   ├── app.ts                     # Componente raiz
│   │   ├── app.html                   
│   │   ├── app.css                    
│   │   ├── app.config.ts              # Configuração da aplicação
│   │   └── app.routes.ts              # Definição de rotas
│   │
│   ├── assets/                        # Arquivos estáticos
│   │   └── img/                       
│   │
│   ├── environments/                  # Configurações por ambiente
│   │   └── environment.ts             # Variáveis de ambiente
│   │
│   ├── index.html                     # HTML principal
│   ├── main.ts                        # Bootstrap da aplicação
│   └── styles.css                     # Estilos globais
│
├── public/                            # Arquivos públicos
│   └── favicon.ico                    
│
├── package.json                       # Dependências NPM
└── angular.json                       # Configuração Angular CLI
```

### Fluxo de Autenticação no Frontend

```
┌─────────────┐     ┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│   Login     │────►│ AuthService │────►│    API      │────►│  MongoDB    │
│   Page      │     │   login()   │     │  /login     │     │             │
└─────────────┘     └──────┬──────┘     └─────────────┘     └─────────────┘
                          │
                          ▼
              ┌───────────────────────┐
              │  localStorage         │
              │  - token              │
              │  - refreshToken       │
              └───────────────────────┘
                          │
                          ▼
              ┌───────────────────────┐
              │  AuthInterceptor      │
              │  Adiciona Bearer      │
              │  em todas requisições │
              └───────────────────────┘
```

---

## 🔌 Endpoints da API

### Autenticação (`/api/auth`)

| Método | Endpoint | Descrição | Auth Required |
|--------|----------|-----------|---------------|
| `POST` | `/login` | Realiza login e retorna tokens | ❌ |
| `POST` | `/refresh-token` | Renova access token | ❌ |
| `GET` | `/me` | Retorna dados do usuário logado | ✅ |

#### Exemplo: Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "usuario@email.com",
  "password": "senha123"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "abc123def456..."
}
```

---

### Usuários (`/api/user`)

| Método | Endpoint | Descrição | Auth Required |
|--------|----------|-----------|---------------|
| `POST` | `/` | Cria novo usuário | ❌ |
| `GET` | `/{id}` | Busca usuário por ID | ✅ |
| `PATCH` | `/{id}` | Atualiza usuário | ✅ |
| `DELETE` | `/{id}` | Remove usuário | ✅ |

#### Exemplo: Criar Usuário
```http
POST /api/user
Content-Type: application/json

{
  "name": "João Silva",
  "email": "joao@email.com",
  "phone": "(11) 99999-9999",
  "password": "senha123",
  "address": {
    "street": "Rua das Flores",
    "complement": "Apto 101",
    "number": "123",
    "neighborhood": "Centro",
    "city": "São Paulo",
    "state": "SP",
    "country": "Brasil",
    "zipCode": "01234-567"
  },
  "role": 0
}
```

---

### Células (`/api/cells`)

| Método | Endpoint | Descrição | Auth Required |
|--------|----------|-----------|---------------|
| `POST` | `/` | Cria nova célula | ✅ |
| `GET` | `/{id}` | Busca célula por ID | ✅ |
| `PATCH` | `/{id}` | Atualiza célula | ✅ |
| `DELETE` | `/{id}` | Remove célula | ✅ |

#### Exemplo: Criar Célula
```http
POST /api/cells
Content-Type: application/json

{
  "name": "Célula Centro",
  "leaderId": "550e8400-e29b-41d4-a716-446655440000"
}
```

---

## 🏛 Modelos de Domínio

### User (Usuário)

```csharp
public class User : BaseEntity<Guid>
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public string Password { get; private set; }      // Hash BCrypt
    public Address Address { get; private set; }      // Value Object
    public UserRole Role { get; private set; }        // Admin, Leader, Member
    public IReadOnlyCollection<RefreshToken> RefreshTokens { get; }
}
```

### Cell (Célula)

```csharp
public class Cell : BaseEntity<Guid>
{
    public string Name { get; private set; }
    public Guid? LeaderId { get; private set; }
    public User Lider { get; private set; }
    public List<User> Members { get; private set; }
}
```

### Address (Endereço - Value Object)

```csharp
public class Address
{
    public string Street { get; }
    public string Complement { get; }
    public string Number { get; }
    public string Neighborhood { get; }
    public string City { get; }
    public string State { get; }
    public string Country { get; }
    public string ZipCode { get; }
}
```

### UserRole (Enum)

```csharp
public enum UserRole
{
    Admin = 0,    // Administrador do sistema
    Leader = 1,   // Líder de célula
    Member = 2    // Membro comum
}
```

---

## 🧪 Testes

### Testes Implementados

| Arquivo | Handler Testado | Cenários |
|---------|-----------------|----------|
| `GetUserByIdHandlerTests.cs` | `GetUserByGuidQueryHandler` | 2 testes |

#### Cenários Cobertos:

1. **`Handle_WhenUserDoesNotExist_ShouldReturnNotFound`**
   - Arrange: Mock retorna `null`
   - Act: Executa query com ID inexistente
   - Assert: `IsSuccess = false`, erro `UserErrors.Account.NotFound`

2. **`Handle_WhenUserExist_ShouldReturnUser`**
   - Arrange: Mock retorna usuário fake (Bogus)
   - Act: Executa query com ID válido
   - Assert: `IsSuccess = true`, retorna `UserResponse` correto

### Testes Pendentes

- [ ] `CreateUserCommandHandler`
- [ ] `UpdateUserCommandHandler`
- [ ] `DeleteUserCommandHandler`
- [ ] `LoginCommandHandler`
- [ ] `RefreshTokenCommandHandler`
- [ ] `CreateCellCommandHandler`
- [ ] `UpdateCellCommandHandler`
- [ ] `DeleteCellCommandHandler`
- [ ] `GetCellByGuidQueryHandler`

---

## 📊 Status do Desenvolvimento

### Progresso por Módulo

```
┌──────────────────────────────────────────────────────────────────┐
│                      PROGRESSO GERAL                              │
├──────────────────────────────────────────────────────────────────┤
│                                                                   │
│  Backend - Auth        ████████████████████████  100%  ✅        │
│  Backend - Users       ████████████████████████  100%  ✅        │
│  Backend - Cells       ████████████████████████  100%  ✅        │
│  Backend - Listagens   ░░░░░░░░░░░░░░░░░░░░░░░░    0%  ❌        │
│                                                                   │
│  Frontend - Auth       ████████████████████████  100%  ✅        │
│  Frontend - Home       ████████░░░░░░░░░░░░░░░░   30%  🔄        │
│  Frontend - Users      ░░░░░░░░░░░░░░░░░░░░░░░░    0%  ❌        │
│  Frontend - Cells      ░░░░░░░░░░░░░░░░░░░░░░░░    0%  ❌        │
│                                                                   │
│  Testes Unitários      ████░░░░░░░░░░░░░░░░░░░░   15%  🔄        │
│  Testes Integração     ░░░░░░░░░░░░░░░░░░░░░░░░    0%  ❌        │
│                                                                   │
│  ─────────────────────────────────────────────────────────────── │
│  TOTAL ESTIMADO        ██████████████░░░░░░░░░░   55%            │
│                                                                   │
└──────────────────────────────────────────────────────────────────┘
```

### Funcionalidades por Status

#### ✅ Completo
- [x] Autenticação JWT com Refresh Token
- [x] CRUD completo de Usuários (API)
- [x] CRUD completo de Células (API)
- [x] Result Pattern no Core
- [x] Validação com FluentValidation
- [x] Login no Frontend
- [x] Cadastro no Frontend
- [x] Guards de rotas
- [x] Interceptors HTTP

#### 🔄 Em Progresso
- [ ] Testes unitários (2 de ~20 estimados)
- [ ] Página Home (estrutura pronta, sem conteúdo)

#### ❌ Não Iniciado
- [ ] Listagem de usuários (`GET /api/users`)
- [ ] Listagem de células (`GET /api/cells`)
- [ ] Paginação nas listagens
- [ ] Filtros de busca
- [ ] Página de gerenciamento de células
- [ ] Página de perfil do usuário
- [ ] Testes de integração
- [ ] Docker/containerização
- [ ] CI/CD pipeline

---

## 🐛 Bugs Conhecidos

### 🔴 Alta Prioridade

#### Bug #1: Inconsistência no Logout (Frontend)
**Arquivo:** `amarservir-app/src/app/core/services/auth/auth.service.ts`

**Problema:** O método `logout()` tenta remover chaves incorretas do localStorage.

```typescript
// ❌ CÓDIGO ATUAL (INCORRETO)
logout() {
    localStorage.removeItem('access_token');  // Chave errada!
    localStorage.removeItem('refresh_token'); // Chave errada!
    this.router.navigate(['/login']);
}

// ✅ CÓDIGO CORRETO
logout() {
    localStorage.removeItem('token');         // Chave correta
    localStorage.removeItem('refreshToken');  // Chave correta
    this.router.navigate(['/login']);
}
```

**Impacto:** Após logout, os tokens permanecem no localStorage, causando comportamento inesperado.

---

### 🟡 Média Prioridade

#### Bug #2: Address com campo duplicado (Backend)
**Arquivo:** `tests/.../GetUserByIdHandlerTests.cs`

**Problema:** Na criação do Address fake, o campo `Neighborhood` recebe `f.Address.City()` ao invés de um bairro.

```csharp
// Linha 43-44 no Faker
f.Address.City(),        // Neighborhood - deveria ser outra coisa
f.Address.City(),        // City - correto
```

**Impacto:** Baixo - afeta apenas testes, não produção.

---

## 🚀 Próximos Passos

### Curto Prazo (Sprint Atual)
1. **Corrigir bug do logout** no AuthService
2. **Completar testes** do GetUserByGuidQueryHandler
3. **Implementar** `GET /api/users` (listar todos)
4. **Implementar** `GET /api/cells` (listar todas)

### Médio Prazo
5. **Criar página de Células** no frontend
6. **Criar página de Usuários** no frontend
7. **Adicionar paginação** nas listagens
8. **Implementar filtros** de busca

### Longo Prazo
9. **Testes de integração** com banco de teste
10. **Docker Compose** para ambiente local
11. **CI/CD** com GitHub Actions
12. **Deploy** em ambiente de staging

---

## 🖥 Como Executar

### Pré-requisitos
- .NET 9 SDK
- Node.js 18+
- MongoDB (local ou Atlas)

### Backend

```bash
# Navegar para o diretório da API
cd backend/Auth.API

# Restaurar pacotes
dotnet restore

# Configurar variáveis de ambiente (.env)
# MONGO_ROOT_USER=admin
# MONGO_ROOT_PASS=senha
# DATABASE_NAME=AmarEServirDB
# JWT_SECRET_KEY=sua_chave_secreta

# Executar
dotnet run
```

A API estará disponível em: `https://localhost:7xxx` ou `http://localhost:5xxx`

### Frontend

```bash
# Navegar para o diretório do app
cd amarservir-app

# Instalar dependências
npm install

# Executar em modo desenvolvimento
npm start
```

A aplicação estará disponível em: `http://localhost:4200`

### Testes

```bash
# Executar testes do backend
cd tests/AmarEServir.UnitTests
dotnet test

# Executar testes do frontend
cd amarservir-app
npm test
```

---

## 📝 Notas Adicionais

### Convenções de Código
- **Backend:** Seguir padrões C# da Microsoft
- **Frontend:** Prettier configurado com single quotes
- **Commits:** Usar Conventional Commits (`feat:`, `fix:`, `docs:`, etc.)

### Contato
- **Repositório:** https://github.com/lucianop-bs/AmarEServir
- **Branch principal:** `main`

---

> 📅 **Documento gerado em:** Junho/2025  
> 🔄 **Próxima revisão sugerida:** Após conclusão da Sprint atual
