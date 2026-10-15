# Instruções para agentes

1. **Consulte `knowledge/` antes de investigar o código.** Comece por `knowledge/index.md`
   e siga os `[[links]]` até as notas do assunto (regras, decisões, problemas conhecidos).
2. **Confirme no código tudo o que uma nota afirmar.** A nota explica o "porquê";
   o código é a fonte da verdade sobre o que acontece hoje.
3. **Se encontrar divergência entre nota e código, aponte-a** na resposta:
   diga o que a nota afirma, o que o código faz e em qual arquivo.
   Não corrija a nota nem o código sem pedido explícito.

Knowledge orienta, código valida.

## Comandos
- Build: `dotnet build`
- Testes: `dotnet test`
- Rodar a API: `dotnet run --project src/DemoApi` (porta 5080)
