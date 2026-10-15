# Usuários

## Cadastro (`POST /users`)
Recebe `name`, `email` e `password`. Sucesso devolve `201` com `id`, `name` e `email`.

## Regras
- **E-mail é obrigatório.** Todo usuário precisa de um e-mail para login e contato.
  ⚠ Hoje o código não garante isso: ver [[problemas-conhecidos]].
- **Formato do e-mail** é validado por `Users/EmailValidator.cs`. Inválido: `400`.
- **E-mail único**, sem diferenciar maiúsculas: `Ana@Example.com` e `ana@example.com`
  são o mesmo e-mail. Duplicado: `409`.
- **Senha com no mínimo 8 caracteres.** Menor: `400`.
- A senha é guardada como hash (ver [[seguranca]]).

## Alterações
- O nome é alterado via `PUT /profile` (só o próprio usuário, ver [[seguranca]]).
- Ainda **não existe** endpoint para alterar e-mail.

## Usuários de exemplo
`ana@example.com`, `bruno@example.com`, `carla@example.com`, todos com a senha `Senha@123`.

Relacionadas: [[autenticacao]], [[regras-de-negocio]].
