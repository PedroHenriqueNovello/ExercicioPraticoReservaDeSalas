# Funcionalidade Adicional: Controle de Acesso e Cache(Padrão Proxy)

## 1. Descrição da Funcionalidade
Esta extensão do projeto introduz uma camada intermediária de inteligência no sistema de reservas através do padrão de projeto Proxy. A implementação foca em dois pilares:

**Controle de Acesso:** O sistema agora diferencia as permissões com base no `NivelAcesso` do usuário (Aluno, Docente ou Admin). Operações críticas, como o cancelamento de reservas, são restritas a usuários autorizados, enquanto a adição de reservas exige validação de nível antes de ser processada pelo objeto real.
**Cache de Dados:** Para otimizar o desempenho, a listagem de reservas é armazenada em cache por um período de 10 segundos. Consultas consecutivas dentro deste intervalo são servidas instantaneamente pelo Proxy, reduzindo o processamento no gerenciador principal. O cache é invalidado automaticamente sempre que uma nova reserva é adicionada ou cancelada.

## 2. Justificativa do Padrão de Projeto
A escolha do padrão Proxy é justificada pela necessidade de adicionar regras (segurança e otimização) de forma transparente. Ao utilizar o Proxy, conseguimos interceptar as chamadas ao `GerenciadorDeReservasSala` sem modificar seu código original, respeitando o Princípio de Responsabilidade Única e garantindo que a lógica de persistência de dados permaneça isolada da lógica de controle de acesso.

## 3. Verificação de Funcionamento
O sistema conta com um cenário de teste automático na inicialização (classe `Program`) que demonstra:
1.  A recuperação de dados via cache em chamadas consecutivas.
2.  O bloqueio de segurança ao tentar realizar operações sem o nível de privilégio necessário.