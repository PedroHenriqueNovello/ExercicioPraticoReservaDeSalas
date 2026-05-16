# Funcionalidade Adicional: Controle de Acesso e Cache (Padrão Proxy)

## 1. Descrição da Funcionalidade
Esta extensão do projeto introduz uma camada intermediária de inteligência no sistema de reservas através do padrão de projeto Proxy. A implementação foca em dois pilares:

**Controle de Acesso Refinado:** O sistema agora diferencia as permissões com base no `NivelAcesso` do usuário (Aluno, Docente ou Admin) e na propriedade da reserva. As regras são as seguintes:

- **Alunos:**
    - Podem reservar apenas salas do tipo "Estudo Individual" e "Trabalho em Grupo".
    - Não podem cancelar reservas feitas por outros usuários.
- **Docentes:**
    - Podem reservar qualquer tipo de sala, incluindo "Laboratório".
    - Não podem cancelar reservas feitas por outros usuários.
- **Administradores:**
    - Podem reservar qualquer tipo de sala.
    - Podem cancelar *qualquer* reserva, independentemente de quem a fez.

**Cache de Dados:** Para otimizar o desempenho, a listagem de reservas é armazenada em cache por um período de 10 segundos. Consultas consecutivas dentro deste intervalo são servidas instantaneamente pelo Proxy, reduzindo o processamento no gerenciador principal. O cache é invalidado automaticamente sempre que uma nova reserva é adicionada ou cancelada.

## 2. Justificativa do Padrão de Projeto
A escolha do padrão Proxy é justificada pela necessidade de adicionar regras (segurança e otimização) de forma transparente. Ao utilizar o Proxy, conseguimos interceptar as chamadas ao `GerenciadorDeReservasSala` sem modificar seu código original, respeitando o Princípio de Responsabilidade Única e garantindo que a lógica de persistência de dados permaneça isolada da lógica de controle de acesso.

## 3. Verificação de Funcionamento
O sistema conta com um cenário de teste automático na inicialização (classe `Program`) que demonstra:
1.  A recuperação de dados via cache em chamadas consecutivas.
2.  O bloqueio de segurança ao tentar realizar operações sem o nível de privilégio necessário.

Além disso, o menu interativo permite testar todas as regras de controle de acesso para adição e cancelamento de reservas, bem como a alteração de políticas (restrita a administradores).
