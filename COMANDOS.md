# Comandos da apresentação, na ordem

Todos os comandos são para o **PowerShell**. Siga de cima para baixo.

Duas pastas, com papéis diferentes:

| Pasta | Para quê | Pode resetar? |
|---|---|---|
| `$HOME\dev\alem-do-chat-demo` | **Onde as demos rodam.** O agente trabalha aqui. | Sim, entre cada demo |
| `$HOME\dev\alem-do-chat-demo-vault` | **Só para mostrar** o Obsidian e o grafo (branch `apresentacao`) | **Nunca.** Não rode demos nem `git clean` aqui |

O roteiro falado (prompts e o que esperar) está no `DEMO.md`. Este arquivo é só a sequência de comandos.

---

## 0. Uma vez por computador

Pré-requisitos: Git, .NET SDK 8 ou mais novo (com o runtime 8), Claude Code e Obsidian.

```powershell
mkdir $HOME\dev -Force
cd $HOME\dev

# Repositório das demos
git clone https://github.com/BrunoMendesSaraiva/alem-do-chat-demo.git

# Cópia só para mostrar o Obsidian e o grafo
git clone https://github.com/BrunoMendesSaraiva/alem-do-chat-demo.git alem-do-chat-demo-vault
cd alem-do-chat-demo-vault
git checkout apresentacao
```

Em um **PowerShell como administrador**, evite que o Windows Defender atrapalhe o `dotnet test`:

```powershell
Add-MpPreference -ExclusionPath "$HOME\dev\alem-do-chat-demo"
Add-MpPreference -ExclusionPath "$HOME\.nuget\packages"
```

No Obsidian: **Open folder as vault** → `$HOME\dev\alem-do-chat-demo-vault\knowledge`.

---

## 1. No dia, antes de subir no palco

```powershell
# Abre o grafo no navegador (funciona sem internet)
start $HOME\dev\alem-do-chat-demo-vault\graphify-out\graph.html

# Aquece o build e confere que tudo passa (espera: 12 testes aprovados)
cd $HOME\dev\alem-do-chat-demo
git checkout -f demo-1
git clean -fd
dotnet test
```

- [ ] O `graph.html` está aberto numa aba.
- [ ] O Obsidian está aberto no vault, com o Graph view pronto (`Ctrl+G`).
- [ ] O terminal está em `$HOME\dev\alem-do-chat-demo`, com fonte grande.

---

## 2. Demo 1: o agente entende e altera o código

```powershell
cd $HOME\dev\alem-do-chat-demo
git checkout -f demo-1
git clean -fd
claude
```

Prompts, um de cada vez:

```text
Analise esse projeto e me explique como funciona o cadastro de usuário.
```
```text
Onde está sendo feita a validação de e-mail?
```
```text
Adicione uma validação para impedir e-mail vazio e execute os testes.
```

Para encerrar a sessão: `/exit`

### (Opcional) Provar o bug antes da correção

Em **outro terminal**, antes do 3º prompt:

```powershell
cd $HOME\dev\alem-do-chat-demo
dotnet run --project src/DemoApi
```

Em um **terceiro terminal**:

```powershell
Invoke-WebRequest -UseBasicParsing -Method Post -Uri http://localhost:5080/users -ContentType "application/json" -Body '{"name":"Sem Email","email":"","password":"Senha@123"}' | Select-Object StatusCode, Content
```

Esperado: `StatusCode 201`. Depois, pare a API com `Ctrl+C` no terminal dela.

---

## 3. Demo 2: base de conhecimento

```powershell
cd $HOME\dev\alem-do-chat-demo
git checkout -f demo-2
git clean -fd
```

1. Mostre o **Obsidian** (Graph view) e o **graph.html** (cadeia do token de 30 min e o nó "Bloqueio após 5 tentativas").
2. Mostre o `CLAUDE.md`:
   ```powershell
   code CLAUDE.md
   ```
3. Abra o agente:
   ```powershell
   claude
   ```
   ```text
   Como funciona a autenticação deste projeto?
   ```
   Se ele não apontar sozinho a nota desatualizada:
   ```text
   O login bloqueia depois de várias tentativas erradas?
   ```

Para encerrar a sessão: `/exit`

---

## 4. Demo 3: multiagentes

```powershell
cd $HOME\dev\alem-do-chat-demo
git checkout -f demo-3
git clean -fd
```

Abra **3 terminais**, todos nesta pasta. Rode **um de cada vez** e espere cada um terminar.
**Não faça commit** entre eles: o reviewer e o QA leem o `git diff` deixado pelo dev.

**Terminal 1: dev**
```powershell
cd $HOME\dev\alem-do-chat-demo
claude
```
```text
Siga as instruções de agents/dev.md. Implemente a alteração de e-mail do usuário (PUT /users/{id}/email).
```

**Terminal 2: reviewer** (depois que o dev terminar)
```powershell
cd $HOME\dev\alem-do-chat-demo
claude
```
```text
Siga as instruções de agents/reviewer.md. Analise a implementação feita pelo agente anterior.
```

**Terminal 3: QA** (depois do reviewer)
```powershell
cd $HOME\dev\alem-do-chat-demo
claude
```
```text
Siga as instruções de agents/qa.md. Crie cenários para tentar encontrar falhas.
```

Para encerrar cada sessão: `/exit`

---

## Se algo der errado

| Situação | O que fazer |
|---|---|
| O agente bagunçou os arquivos | `/exit`, depois `git checkout -f demo-N` + `git clean -fd` e comece a demo de novo |
| `dotnet test` disse "Não há nenhum teste disponível" | É o Windows Defender. Rode `dotnet test` de novo |
| A porta 5080 já está em uso | Feche o terminal onde a API ficou rodando, ou `Stop-Process -Name DemoApi` |
| O `graph.html` abriu em branco | Confira se a pasta `graphify-out\vendor\` existe no clone (branch `apresentacao`) |

---

## Depois da apresentação

- **Não dê `git push`** a partir de nenhuma das duas pastas.
- Para remover a exclusão do Defender (PowerShell como administrador):
  ```powershell
  Remove-MpPreference -ExclusionPath "$HOME\dev\alem-do-chat-demo"
  Remove-MpPreference -ExclusionPath "$HOME\.nuget\packages"
  ```
