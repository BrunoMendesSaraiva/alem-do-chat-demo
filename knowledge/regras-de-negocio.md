# Regras de negócio

| #  | Regra                                                          | Origem                            |
|----|----------------------------------------------------------------|-----------------------------------|
| 1  | E-mail é obrigatório no cadastro                               | [[usuarios]]                      |
| 2  | E-mail precisa ter formato válido                              | [[usuarios]]                      |
| 3  | E-mail é único, sem diferenciar maiúsculas                     | [[usuarios]]                      |
| 4  | Senha tem no mínimo 8 caracteres                               | [[usuarios]]                      |
| 5  | Senha é guardada só como hash                                  | [[seguranca]]                     |
| 6  | Login inválido devolve erro genérico (`401`)                   | [[autenticacao]]                  |
| 7  | Token JWT expira em 30 minutos                                 | [[decisoes/ADR-002-token-30-min]] |
| 8  | Usuário só altera os próprios dados (id vem do token)          | [[seguranca]]                     |
| 9  | `PUT /profile` altera somente o nome                           | [[seguranca]]                     |
| 10 | Senha e token não vão para log                                 | [[seguranca]]                     |

Regras que o código ainda não cumpre: [[problemas-conhecidos]].
