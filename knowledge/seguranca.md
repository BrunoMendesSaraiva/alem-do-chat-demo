# Segurança

## Hash de senha
- Senhas nunca são guardadas em texto puro.
- PBKDF2 com SHA-256, salt aleatório de 16 bytes e 100.000 iterações (`Auth/PasswordHasher.cs`).
- A comparação usa tempo constante (`CryptographicOperations.FixedTimeEquals`).

## Proteção do login
- Após **5 tentativas de login erradas**, a conta fica bloqueada por 15 minutos.
- Mensagem de erro genérica: não revela se o e-mail existe. Ver [[autenticacao]].

## Um usuário só altera os próprios dados
- O id do usuário vem **sempre do token** (claim `sub`), nunca da URL ou do corpo.
- Por isso `/profile` não recebe id: cada um só vê e altera o próprio perfil.
- Qualquer endpoint novo que receba um id na rota precisa conferir se ele é o mesmo do token.
- `PUT /profile` altera **somente o nome**. E-mail e senha não mudam por ali. Ver [[usuarios]].

## O que não pode ir para log
- Senha (nem em texto puro, nem o hash).
- Token JWT completo.
- Corpo inteiro de requisições de `/login` e `/users`.
- E-mail só quando for indispensável para investigação.
