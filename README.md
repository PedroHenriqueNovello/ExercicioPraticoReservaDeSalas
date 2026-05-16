# Sistema de Reserva de Salas

Este projeto implementa um sistema de reserva de salas utilizando C# e diversos padrões de projeto para garantir modularidade, extensibilidade e um controle de acesso robusto.

## Funcionalidades

- **Reserva de Salas:** Permite que usuários reservem diferentes tipos de salas (Estudo Individual, Trabalho em Grupo, Laboratório).
- **Políticas de Reserva:** Suporta diferentes políticas para aprovação de reservas (e.g., Primeiro a Chegar, Prioridade para Docentes).
- **Autenticação e Autorização:** Sistema de login e registro de usuários com diferentes níveis de acesso (Aluno, Docente, Admin).
- **Persistência de Usuários:** Dados dos usuários são salvos em um arquivo JSON (`usuarios.json`).
- **Controle de Acesso Refinado (Proxy):**
    - **Alunos:** Podem reservar apenas salas de Estudo Individual e Trabalho em Grupo. Não podem cancelar reservas de outros usuários.
    - **Docentes:** Podem reservar qualquer tipo de sala. Não podem cancelar reservas de outros usuários.
    - **Administradores:** Podem reservar qualquer tipo de sala e cancelar qualquer reserva.
- **Notificações:** Usuários são notificados sobre o status de suas reservas (via padrão Observer).
- **Extras de Reserva (Decorator):** Possibilidade de adicionar equipamentos multimídia ou serviço de limpeza a uma reserva.
- **Cache de Reservas (Proxy):** Otimização de desempenho para listagem de reservas, com cache de 10 segundos.
- **Relatórios:** Geração de relatório diário de reservas.
- **Interface de Linha de Comando (CLI):** Menu interativo para interação com o sistema.
# Sistema de Reserva de Salas

Este projeto implementa um sistema de reserva de salas utilizando C# e diversos padrões de projeto para garantir modularidade, extensibilidade e um controle de acesso robusto.

## Funcionalidades

- **Reserva de Salas:** Permite que usuários reservem diferentes tipos de salas (Estudo Individual, Trabalho em Grupo, Laboratório).
- **Políticas de Reserva:** Suporta diferentes políticas para aprovação de reservas (e.g., Primeiro a Chegar, Prioridade para Docentes).
- **Autenticação e Autorização:** Sistema de login e registro de usuários com diferentes níveis de acesso (Aluno, Docente, Admin).
- **Persistência de Usuários:** Dados dos usuários são salvos em um arquivo JSON (`usuarios.json`).
- **Controle de Acesso Refinado (Proxy):**
    - **Alunos:** Podem reservar apenas salas de Estudo Individual e Trabalho em Grupo. Não podem cancelar reservas de outros usuários.
    - **Docentes:** Podem reservar qualquer tipo de sala. Não podem cancelar reservas de outros usuários.
    - **Administradores:** Podem reservar qualquer tipo de sala e cancelar qualquer reserva.
- **Notificações:** Usuários são notificados sobre o status de suas reservas (via padrão Observer).
- **Extras de Reserva (Decorator):** Possibilidade de adicionar equipamentos multimídia ou serviço de limpeza a uma reserva.
- **Cache de Reservas (Proxy):** Otimização de desempenho para listagem de reservas, com cache de 10 segundos.
- **Relatórios:** Geração de relatório diário de reservas.
- **Interface de Linha de Comando (CLI):** Menu interativo para interação com o sistema.

## Padrões de Projeto Utilizados

- **Singleton:** `GerenciadorDeReservasSala` garante uma única instância para gerenciar todas as reservas.
- **Factory Method:** `SalaFactory` (e suas implementações `SalaIndividualFactory`, `SalaGrupoFactory`, `SalaLaboratorioFactory`) para criar diferentes tipos de salas de forma flexível.
- **Proxy:** `GerenciadorDeReservasSalaProxy` atua como um intermediário para adicionar controle de acesso (autenticação e autorização) e cache de listagem de reservas, sem modificar a lógica central do `GerenciadorDeReservasSala`.
- **Decorator:** `EquipamentoMultimidiaDecorator` e `ServicoDeLimpezaDecorator` permitem adicionar funcionalidades extras a uma reserva dinamicamente.
- **Observer:** `Usuario` implementa `IObserver` para receber notificações sobre atualizações de reservas, e `GerenciadorDeReservasSala` atua como `ISubject`.
- **Strategy:** `IPoliticaDeReserva` (e suas implementações `PoliticaPrimeiroChegar`, `PoliticaPrioridadeDocente`) permite trocar a lógica de aprovação de reservas em tempo de execução.

## Configuração e Execução

### Pré-requisitos

- .NET SDK 9.0 ou superior.

### Como Executar

1. **Navegue até o diretório do projeto:**
   ```bash
   cd ../ExercicioPraticoReservaDeSalas2
   ```
2. **Compile o projeto:**
   ```bash
   dotnet build
   ```
3. **Execute a aplicação:**
   ```bash
   dotnet run
   ```

### Uso do Sistema

Ao iniciar a aplicação, você será apresentado a um menu de login/registro:

1. **Login:** Utilize um usuário existente. Por padrão, um usuário `admin` com senha `admin123` é criado automaticamente na primeira execução, caso não exista.
2. **Registrar Novo Usuário:** Crie novos usuários com diferentes níveis de acesso (Aluno, Docente, Admin).

Após o login, o menu principal permitirá:

- **Criar Nova Reserva:** Faça uma reserva, escolhendo a sala, data, horário e adicionando extras (se desejar). As permissões de sala são validadas aqui.
- **Listar Reservas Atuais:** Visualize todas as reservas ativas.
- **Cancelar Reserva:** Cancele uma reserva existente. As permissões de cancelamento são validadas aqui.
- **Alterar Política de Reserva:** Apenas administradores podem alterar a política de reserva (Primeiro a Chegar ou Prioridade para Docentes).
- **Emitir Relatório Diário:** Gere um relatório das reservas para uma data específica.
- **Logout:** Desconecta o usuário atual.
- **Sair:** Encerra a aplicação.

## Estrutura do Projeto

```
ExercicioPraticoReservaDeSalas2/
├── src/
│   ├── Program.cs                  # Ponto de entrada da aplicação, menu e fluxo principal.
│   ├── Usuario.cs                  # Define a entidade Usuário e seu nível de acesso.
│   ├── UsuarioService.cs           # Gerencia a persistência e autenticação de usuários (JSON).
│   ├── IGerenciadorDeReservas.cs   # Interface para o gerenciador de reservas.
│   ├── GerenciadorDeReservasSala.cs# Implementação concreta do gerenciador de reservas (Singleton).
│   ├── GerenciadorDeReservasSalaProxy.cs # Proxy para controle de acesso e cache.
│   ├── IObserver.cs, ISubject.cs   # Interfaces para o padrão Observer.
│   ├── Reserva.cs                  # Entidade Reserva.
│   ├── Sala.cs, SalaFactory.cs     # Classes base e fábrica para diferentes tipos de salas.
│   ├── SalaEstudoIndividual.cs, SalaGrupo.cs, SalaLaboratorio.cs # Implementações de salas.
│   ├── IPoliticaDeReserva.cs       # Interface para políticas de reserva.
│   ├── PoliticaPrimeiroChegar.cs, PoliticaPrioridadeDocente.cs # Implementações de políticas.
│   ├── EquipamentoMultimidiaDecorator.cs, ServicoDeLimpezaDecorator.cs # Decorators para extras.
│   └── ... (outros arquivos de classes)
├── docs/
│   ├── funcionalidade_adicional.md # Documentação detalhada do padrão Proxy.
│   └── diagrama_sequencia.png # Diagrama de sequência completo do Projeto
├── usuarios.json                   # Arquivo de persistência dos usuários.
└── ExercicioPraticoReservaDeSalas2.sln # Arquivo de solução do Visual Studio.
```

## Melhorias Futuras

- **Persistência de Reservas:** Atualmente, as reservas são voláteis e perdidas ao encerrar a aplicação. Implementar persistência (e.g., em JSON, XML ou banco de dados) para as reservas.
- **Validação de Conflitos de Horário:** Refinar a lógica de validação para cenários mais complexos (e.g., reservas parciais, sobreposição de datas).
- **Interface Gráfica:** Desenvolver uma interface de usuário mais amigável (e.g., via ASP.NET Core, WPF ou Blazor).
- **Testes Unitários:** Adicionar testes unitários para as classes de lógica de negócio e padrões de projeto.
- **Notificações Assíncronas:** Implementar um sistema de notificação mais robusto, talvez com filas de mensagens ou eventos.
- **Gerenciamento de Salas:** Adicionar funcionalidades para criar, editar e remover salas dinamicamente.

### Autores
- Letícia Abraão Moreira
- Pedro Henrique Novello D'Elia Porto de Almeida
- Enrico Manzolli Bertoni (Extensão)