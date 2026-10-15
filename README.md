# alem-do-chat-demo

API de exemplo usada nas demos da palestra **"Além do chat"** sobre agentes de IA.

.NET 8 · ASP.NET Core Minimal API · xUnit · repositório em memória (sem banco).

## Como rodar

```bash
dotnet run --project src/DemoApi
```

A API sobe em `http://localhost:5080`.

| Método | Rota       | O que faz                                 |
|--------|------------|-------------------------------------------|
| POST   | `/users`   | Cadastro (`name`, `email`, `password`)    |
| POST   | `/login`   | Devolve um JWT (`email`, `password`)      |
| GET    | `/profile` | Dados do usuário logado                   |
| PUT    | `/profile` | Altera o nome do usuário logado           |

Usuários já cadastrados (senha `Senha@123`):
`ana@example.com`, `bruno@example.com`, `carla@example.com`.

Exemplo:

```bash
curl -X POST http://localhost:5080/login \
  -H "Content-Type: application/json" \
  -d '{"email":"ana@example.com","password":"Senha@123"}'
```

## Como rodar os testes

```bash
dotnet test
```
