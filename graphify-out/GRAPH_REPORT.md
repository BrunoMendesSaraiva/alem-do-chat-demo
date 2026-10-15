# Graph Report - knowledge  (2026-10-14)

## Corpus Check
- Corpus is ~1,263 words - fits in a single context window. You may not need a graph.

## Summary
- 43 nodes · 105 edges · 7 communities (6 shown, 1 thin omitted)
- Extraction: 89% EXTRACTED · 10% INFERRED · 1% AMBIGUOUS · INFERRED: 11 edges (avg confidence: 0.84)
- Token cost: 58,814 input · 0 output

## Community Hubs (Navigation)
- Decisões JWT e expiração
- Arquitetura da API
- Autenticação e login
- Cadastro de usuários
- Mapa e regras de negócio
- Perfil e identidade do token
- Hash de senha

## God Nodes (most connected - your core abstractions)
1. `Segurança` - 13 edges
2. `Autenticação` - 12 edges
3. `Usuários` - 12 edges
4. `Arquitetura` - 10 edges
5. `Tabela de regras de negócio (1-10)` - 10 edges
6. `ADR-002: token expira em 30 minutos` - 9 edges
7. `Mapa das notas` - 8 edges
8. `Problemas conhecidos` - 8 edges
9. `ADR-001: JWT em vez de sessão no servidor` - 7 edges
10. `Regras de negócio` - 7 edges

## Surprising Connections (you probably didn't know these)
- `Sem refresh token` --semantically_similar_to--> `Refresh token (adiado)`  [INFERRED] [semantically similar]
  problemas-conhecidos.md → decisoes/ADR-002-token-30-min.md
- `Autenticação` --references--> `ADR-001: JWT em vez de sessão no servidor`  [EXTRACTED]
  autenticacao.md → decisoes/ADR-001-jwt.md
- `Autenticação` --references--> `ADR-002: token expira em 30 minutos`  [EXTRACTED]
  autenticacao.md → decisoes/ADR-002-token-30-min.md
- `Mapa das notas` --references--> `ADR-001: JWT em vez de sessão no servidor`  [EXTRACTED]
  index.md → decisoes/ADR-001-jwt.md
- `ADR-002: token expira em 30 minutos` --references--> `Problemas conhecidos`  [EXTRACTED]
  decisoes/ADR-002-token-30-min.md → problemas-conhecidos.md

## Hyperedges (group relationships)
- **Fluxo de autenticação JWT** — autenticacao_post_login, seguranca_pbkdf2_password_hasher, autenticacao_tokenservice, autenticacao_jwt, autenticacao_duracao_30_minutos, arquitetura_requireauthorization, seguranca_id_do_token_sub [INFERRED 0.85]
- **Cadeia de decisão JWT, irrevogabilidade, 30 min, sem refresh** — decisoes_adr_001_jwt_decisao_jwt, decisoes_adr_001_jwt_token_irrevogavel, decisoes_adr_002_token_30_min_decisao_30_minutos, decisoes_adr_002_token_30_min_refresh_token, problemas_conhecidos_sem_refresh_token [EXTRACTED 1.00]
- **Regras de validação do cadastro** — usuarios_post_users, usuarios_email_obrigatorio, usuarios_emailvalidator, usuarios_email_unico_case_insensitive, usuarios_senha_minimo_8 [EXTRACTED 1.00]

## Communities (7 total, 1 thin omitted)

### Community 0 - "Decisões JWT e expiração"
Cohesion: 0.33
Nodes (8): ADR-001: JWT em vez de sessão no servidor, Sessão no servidor com cookie (descartada), JWT não pode ser revogado antes de expirar, Token opaco (descartado), ADR-002: token expira em 30 minutos, Alternativa 60 minutos (descartada), Refresh token (adiado), Sem refresh token

### Community 1 - "Arquitetura da API"
Cohesion: 0.39
Nodes (6): Arquitetura, InMemoryUserRepository, ASP.NET Core Minimal API (.NET 8), RequireAuthorization (401 sem token válido), Testes de integração com WebApplicationFactory, Usuários de exemplo (ana, bruno, carla)

### Community 2 - "Autenticação e login"
Cohesion: 0.52
Nodes (6): Autenticação, Duração do token: 30 minutos (ClockSkew = 0), JWT (HMAC-SHA256, claims sub e email), POST /login, TokenService / TokenSettings, Bloqueio após 5 tentativas erradas por 15 min

### Community 3 - "Cadastro de usuários"
Cohesion: 0.57
Nodes (7): Bug: cadastro aceita e-mail vazio, E-mail obrigatório, E-mail único sem diferenciar maiúsculas (409), EmailValidator, POST /users (cadastro), Senha com no mínimo 8 caracteres, Usuários

### Community 4 - "Mapa e regras de negócio"
Cohesion: 0.67
Nodes (4): Mapa das notas, Problemas conhecidos, Não existe alteração de e-mail, Regras de negócio

### Community 6 - "Hash de senha"
Cohesion: 1.00
Nodes (3): Comparação em tempo constante (FixedTimeEquals), PasswordHasher (PBKDF2-SHA256, 100k iterações, salt 16 bytes), Segurança

## Ambiguous Edges - Review These
- `POST /login` → `Bloqueio após 5 tentativas erradas por 15 min`  [AMBIGUOUS]
  seguranca.md · relation: conceptually_related_to

## Knowledge Gaps
- **1 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `Arquitetura` connect `Arquitetura da API` to `Autenticação e login`, `Cadastro de usuários`, `Mapa e regras de negócio`, `Hash de senha`?**
  _High betweenness centrality (0.215) - this node is a cross-community bridge._
- **Why does `Segurança` connect `Hash de senha` to `Decisões JWT e expiração`, `Arquitetura da API`, `Autenticação e login`, `Cadastro de usuários`, `Mapa e regras de negócio`, `Perfil e identidade do token`?**
  _High betweenness centrality (0.197) - this node is a cross-community bridge._
- **Why does `Autenticação` connect `Autenticação e login` to `Decisões JWT e expiração`, `Arquitetura da API`, `Cadastro de usuários`, `Mapa e regras de negócio`, `Hash de senha`?**
  _High betweenness centrality (0.188) - this node is a cross-community bridge._

### Low-confidence Hints
_AMBIGUOUS edges — the extractor was unsure. Verify before acting on these._

- **What is the exact relationship between `POST /login` and `Bloqueio após 5 tentativas erradas por 15 min`?**
  _Edge tagged AMBIGUOUS (relation: conceptually_related_to) - confidence is low._