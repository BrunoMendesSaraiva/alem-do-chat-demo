# Arquitetura

ASP.NET Core Minimal API (.NET 8), sem controllers e sem banco de dados.

## Camadas
- **Endpoints**: recebem a requisição, validam e devolvem `IResult`.
- **Serviços/utilitários**: `TokenService`, `PasswordHasher`, `EmailValidator`.
- **Repositório**: `InMemoryUserRepository`, uma lista em memória com 3 usuários de exemplo.
  Os dados somem quando a API reinicia.

## Pastas (`src/DemoApi`)
| Pasta      | Conteúdo                                         | Nota             |
|------------|--------------------------------------------------|------------------|
| `Users/`   | cadastro, modelo `User`, repositório, validação  | [[usuarios]]     |
| `Auth/`    | login, geração de JWT, configurações, hash       | [[autenticacao]] |
| `Profile/` | leitura e alteração do perfil do usuário logado  | [[seguranca]]    |

`Program.cs` só registra serviços, autenticação e endpoints.

## Fluxo de uma requisição
1. A requisição chega ao Kestrel.
2. O middleware de autenticação lê o `Authorization: Bearer <token>`, se houver.
3. O endpoint recebe o corpo já desserializado e os serviços por injeção de dependência.
4. O endpoint valida, consulta o repositório e devolve o resultado.
5. Rotas com `RequireAuthorization()` retornam `401` sem token válido.

Testes de integração em `tests/DemoApi.Tests` usam `WebApplicationFactory`.
