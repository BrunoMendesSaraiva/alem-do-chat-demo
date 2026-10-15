# Autenticação

## Login
- `POST /login` recebe `email` e `password`.
- O e-mail é buscado sem diferenciar maiúsculas (ver [[usuarios]]).
- A senha é comparada com o hash guardado (ver [[seguranca]]).
- Credenciais erradas ou usuário inexistente: `401`, sem dizer qual dos dois falhou.

## JWT
- Sucesso devolve `accessToken` e `expiresInSeconds`.
- Claims: `sub` (id do usuário) e `email`.
- Assinatura HMAC-SHA256; emissor e audiência `alem-do-chat-demo`.
- Por que JWT e não sessão: [[decisoes/ADR-001-jwt]].

## Duração: 30 minutos
- O token expira **30 minutos** após o login. Sem tolerância de relógio (`ClockSkew = 0`).
- Não há refresh token: ao expirar, o usuário faz login de novo.
- O motivo está em [[decisoes/ADR-002-token-30-min]].

## Onde está no código
- `Auth/LoginEndpoints.cs`, `Auth/TokenService.cs`, `Auth/TokenSettings.cs`.
- Os endpoints de perfil usam o `sub` do token para saber quem é o usuário (ver [[seguranca]]).
