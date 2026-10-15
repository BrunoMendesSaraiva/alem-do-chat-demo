# ADR-002: token expira em 30 minutos

**Status:** aceita · **Decidido por:** time de segurança, com aprovação da liderança técnica

## Contexto
- A revisão de segurança apontou que tokens longos aumentam o estrago de um token vazado.
- Como JWT não pode ser revogado ([[decisoes/ADR-001-jwt]]), a única defesa é expirar cedo.
- A revisão pediu **sessões curtas** até existir um mecanismo de renovação.

## Decisão
O token expira **30 minutos** após o login, sem tolerância de relógio.
No código: `TokenSettings.ExpirationMinutes`. Ver [[autenticacao]].

## Alternativas descartadas
- **60 minutos:** proposto pelo time de produto para reduzir logins repetidos.
  Descartado pelo time de segurança: dobra a janela de uso de um token vazado.
- **Refresh token:** resolveria o incômodo do usuário, mas exige armazenar e revogar
  tokens. **Ficou para depois** (ver [[problemas-conhecidos]]).

## Consequências
- O usuário faz login de novo a cada 30 minutos de uso.
- Mudar esse valor exige nova revisão de segurança ([[seguranca]]).
