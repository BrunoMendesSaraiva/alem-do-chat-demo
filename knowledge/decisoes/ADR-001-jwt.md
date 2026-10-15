# ADR-001: JWT em vez de sessão no servidor

**Status:** aceita · **Decidido por:** time de arquitetura, com o time de segurança

## Contexto
- A API é stateless e deve rodar em várias instâncias atrás de um balanceador.
- Não existe banco nem cache compartilhado para guardar sessões.
- Clientes previstos: front-end web e aplicativo mobile.

## Decisão
Usar JWT assinado (HMAC-SHA256) no header `Authorization: Bearer`.
O token carrega o id do usuário (`sub`) e o e-mail. Ver [[autenticacao]].

## Alternativas descartadas
- **Sessão no servidor com cookie:** exige estado compartilhado entre instâncias
  (sticky session ou Redis) e funciona pior no mobile.
- **Token opaco consultado a cada requisição:** precisa de um armazenamento central,
  que o projeto não tem.

## Consequências
- Não dá para revogar um token antes de ele expirar.
- Por isso a duração precisa ser curta: [[decisoes/ADR-002-token-30-min]].
