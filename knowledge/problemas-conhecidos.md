# Problemas conhecidos

## Cadastro aceita e-mail vazio
- **Situação:** `POST /users` com `"email": ""` (ou só espaços) retorna `201 Created`.
- **Regra violada:** e-mail é obrigatório ([[usuarios]]).
- **Causa provável:** `EmailValidator` só valida o formato quando o e-mail foi preenchido.
- **Teste:** não existe teste para esse caso.
- **Status:** aberto.

## Não existe alteração de e-mail
- **Situação:** o usuário não consegue trocar o e-mail depois do cadastro.
- **Cuidado ao implementar:** respeitar as regras de [[usuarios]] e a regra de que
  cada um só altera os próprios dados ([[seguranca]]).
- **Status:** planejado.

## Sem refresh token
- Ao expirar (30 min), o usuário precisa fazer login de novo.
- Decisão consciente, ver [[decisoes/ADR-002-token-30-min]].
- **Status:** adiado.
