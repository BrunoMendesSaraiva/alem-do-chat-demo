# Cola do apresentador

> Cada etapa da palestra é uma tag do git. Esta versão é a da tag **demo-3** (cola completa).

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

---

## Demo 2: base de conhecimento (Knowledge orienta, código valida)

Tag: `demo-2` (adiciona `knowledge/`, `AGENTS.md` e `CLAUDE.md`).

### Antes da demo

- [ ] **Gerar o grafo do Graphify a partir de `knowledge/`** e deixar aberto numa aba.
- [ ] Opcional: abrir `knowledge/` como vault no Obsidian (Graph view) como plano B.

### Prompt

`Como funciona a autenticação deste projeto?`

### O que se espera

- O agente segue o `AGENTS.md`/`CLAUDE.md`: abre `knowledge/index.md` → `autenticacao.md`
  → `decisoes/ADR-002-token-30-min.md` **antes** de abrir o código.
- Explica o **porquê dos 30 minutos**, que só existe na nota (o código não tem comentário):
  revisão de segurança pediu sessões curtas, JWT não pode ser revogado,
  60 minutos foi descartado pelo time de segurança, refresh token ficou para depois.
- Confirma no código: `ExpirationMinutes = 30` em `src/DemoApi/Auth/TokenSettings.cs`,
  uso em `TokenService.cs`, `ClockSkew = TimeSpan.Zero` e o teste
  `Login_ReturnsTokenThatExpiresIn30Minutes` em `LoginTests.cs`.

### Informação propositalmente desatualizada

- **Onde:** `knowledge/seguranca.md`, seção "Proteção do login".
- **O que diz:** "Após 5 tentativas de login erradas, a conta fica bloqueada por 15 minutos."
- **O que o código faz:** nada disso. `Auth/LoginEndpoints.cs` não conta tentativas nem bloqueia.
- **Uso na demo:** se o agente apontar a divergência sozinho, ótimo. Se não, pergunte:
  `O login bloqueia depois de várias tentativas erradas?`
  O esperado é ele ler a nota, conferir `LoginEndpoints.cs` e dizer que a nota está desatualizada.

---

## Demo 3: multiagentes (dev → reviewer → qa)

Tag: `demo-3` (adiciona `agents/dev.md`, `agents/reviewer.md`, `agents/qa.md`).

Cada agente roda numa sessão separada e recebe o seu arquivo de papel como instrução
(ex.: "Siga as instruções de `agents/reviewer.md`."). Rode na ordem e **não faça commit**
entre eles: o reviewer e o QA leem o `git diff` deixado pelo dev.

### Prompts

| Agente   | Instrução            | Prompt                                                                      |
|----------|----------------------|-----------------------------------------------------------------------------|
| dev      | `agents/dev.md`      | `Implemente a alteração de e-mail do usuário (PUT /users/{id}/email).`      |
| reviewer | `agents/reviewer.md` | `Analise a implementação feita pelo agente anterior.`                      |
| qa       | `agents/qa.md`       | `Crie cenários para tentar encontrar falhas.`                              |

### O que o reviewer provavelmente vai apontar

- **Autorização (o principal):** o id vem da URL (`/users/{id}/email`). Sem conferir se ele é
  igual ao `sub` do token, qualquer usuário logado altera o e-mail de outro (IDOR).
  Contraste com `/profile`, que sempre pega o id do token (`knowledge/seguranca.md`).
  Pior ainda se o dev esquecer o `RequireAuthorization()`.
- **Validação:** se o dev reaproveitar o `EmailValidator`, herda o bug do e-mail vazio
  (nesta tag a correção da Demo 1 **não** está aplicada). Se não reaproveitar, há regra duplicada.
- **Duplicidade de e-mail:** precisa checar sem diferenciar maiúsculas e ignorar o próprio usuário
  (trocar `ana@example.com` por `ANA@example.com` não deveria dar `409`).
- **Tratamento de erros:** `404` para usuário inexistente, `403` para outro usuário,
  `409` para e-mail em uso, `400` para e-mail inválido/vazio, corpo nulo sem `500`.

### O que se espera do QA

Testes para: e-mail válido, inválido, vazio, usuário inexistente e tentativa de alterar
outro usuário. Se o dev não tratou a autorização ou o e-mail vazio, **testes falhando**
são o resultado esperado e ótimo para a demo: o QA não corrige `src/`, só reporta.
