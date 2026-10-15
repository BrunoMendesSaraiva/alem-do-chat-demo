# Cola do apresentador

> Cada etapa da palestra é uma tag do git. Esta versão é a da tag **demo-1**.
> As seções da Demo 2 e da Demo 3 entram nas tags seguintes (a cola completa está em `demo-3`).

## Voltar a uma etapa (entre os ensaios)

```bash
git checkout -f demo-1   # ou demo-2, demo-3
git clean -fd
```

`checkout -f` descarta as alterações feitas pelo agente; `clean -fd` apaga os arquivos novos que ele criou.

---

## Demo 1: o agente entende e altera o código

Tag: `demo-1` (só código e testes, sem `knowledge/`, sem `AGENTS.md`/`CLAUDE.md`).

### Prompts, nesta ordem

1. `Analise esse projeto e me explique como funciona o cadastro de usuário.`
2. `Onde está sendo feita a validação de e-mail?`
3. `Adicione uma validação para impedir e-mail vazio e execute os testes.`

### O que se espera

1. O agente lê `Program.cs` e `Users/UserEndpoints.cs` e descreve o fluxo do `POST /users`:
   formato do e-mail (`EmailValidator`) → senha com pelo menos 8 caracteres →
   e-mail duplicado sem diferenciar maiúsculas (`InMemoryUserRepository.EmailExists`) →
   hash da senha (`PasswordHasher`) → grava no repositório em memória → `201 Created`.
2. Aponta `src/DemoApi/Users/EmailValidator.cs` e, idealmente, percebe sozinho que
   e-mail vazio ou só com espaços retorna `true` (passa na validação).
3. Altera a validação (no `EmailValidator` ou no `UserEndpoints`), cria um teste para
   e-mail vazio (talvez também só com espaços), roda `dotnet test` e mostra tudo verde.
   Antes da alteração: 12 testes. Depois: 13 ou mais.

### Se precisar provar o problema ao vivo

```bash
dotnet run --project src/DemoApi
curl -i -X POST http://localhost:5080/users -H "Content-Type: application/json" \
  -d '{"name":"Sem Email","email":"","password":"Senha@123"}'
# Antes da correção: HTTP/1.1 201 Created
```
