using System;
using System.Collections.Generic;
using System.Linq;

namespace ReservaDeSalas
{
    class Program
    {
        static void Main(string[] args)
        {
            IGerenciadorDeReservas gerenciadorReal = GerenciadorDeReservasSala.GetInstance();
            IGerenciadorDeReservas gerenciador = new GerenciadorDeReservasSalaProxy(gerenciadorReal);

            Console.WriteLine("=== TESTES AUTOMÁTICOS DO PROXY ===");
            Console.WriteLine("\n[TESTE CACHE] Primeira consulta:");
            gerenciador.GetReservas();
            Console.WriteLine("[TESTE CACHE] Segunda consulta (deve vir do cache):");
            gerenciador.GetReservas();

            Usuario alunoTeste = new Usuario("Letícia") { Nivel = NivelAcesso.Aluno };
            Reserva reservaTeste = new Reserva { Id = "T-01", Usuario = alunoTeste, Sala = new SalaEstudoIndividual("X1") }; 
            Console.WriteLine("\n[TESTE SEGURANÇA] Cancelando como Aluno (deve ser negado):");
            gerenciador.CancelarReserva(reservaTeste);

            Console.WriteLine("\n=== FIM DOS TESTES - PRESSIONE QUALQUER TECLA PARA O MENU ===");
            Console.ReadKey();
        
            SalaFactory fabricaIndividual = new SalaIndividualFactory();
            SalaFactory fabricaGrupo = new SalaGrupoFactory();
            SalaFactory fabricaLab = new SalaLaboratorioFactory();

            List<Sala> salas = new List<Sala>
            {
                fabricaIndividual.CriarSala("A1"),
                fabricaGrupo.CriarSala("B1"),
                fabricaLab.CriarSala("L1")
            };

            IPoliticaDeReserva politica = new PoliticaPrimeiroChegar();

            while (true)
            {
                Console.Clear();    
                Console.WriteLine("=====================================");
                Console.WriteLine("  SISTEMA DE RESERVA DE SALAS - V1   ");
                Console.WriteLine("=====================================");
                Console.WriteLine($"Política Atual: {politica.GetType().Name}");
                Console.WriteLine("1. Criar Nova Reserva");
                Console.WriteLine("2. Listar Reservas Atuais");
                Console.WriteLine("3. Cancelar Reserva");
                Console.WriteLine("4. Alterar Política de Reserva");
                Console.WriteLine("5. Emitir Relatório Diário");
                Console.WriteLine("6. Sair");
                Console.Write("\nEscolha uma opção: ");

                string? opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        CriarReserva(gerenciador, salas, politica);
                        break;
                    case "2":
                        ListarReservas(gerenciador);
                        break;
                    case "3":
                        CancelarReserva(gerenciador);
                        break;
                    case "4":
                        politica = AlterarPolitica(politica);
                        break;
                    case "5":
                        EmitirRelatorioDiario(gerenciador);
                        break;
                    case "6":
                        Console.WriteLine("Encerrando o sistema...");
                        return;
                    default:
                        Console.WriteLine("Opção inválida! Pressione qualquer tecla para tentar novamente...");
                        Console.ReadKey();
                        break;
                }
            }

            static void CriarReserva(IGerenciadorDeReservas gerenciador, List<Sala> salas, IPoliticaDeReserva politica)
            {
                Console.Clear();
                Console.WriteLine("--- NOVA RESERVA ---");
                Console.Write("Nome do Usuário: ");
                string? nomeUsuario = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(nomeUsuario))
                {
                    Console.WriteLine("Nome de usuário inválido! Pressione qualquer tecla para voltar ao menu...");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Nível de Acesso (0: Aluno, 1: Docente, 2: Admin):");
                if (!int.TryParse(Console.ReadLine(), out int nivel) || !Enum.IsDefined(typeof(NivelAcesso), nivel))
                {
                    Console.WriteLine("Nível de acesso inválido! Pressione qualquer tecla para voltar ao menu...");
                    Console.ReadKey();
                    return;
                }

                Usuario usuario = new Usuario(nomeUsuario);
                usuario.Nivel = (NivelAcesso)nivel;
                usuario.IsDocente = (nivel == (int)NivelAcesso.Docente);

                gerenciador.AddObserver(usuario);

                Console.WriteLine("\nSalas Disponíveis:");
                for (int i = 0; i < salas.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {salas[i].Id} ({salas[i].GetTipo()})");
                }
                Console.Write("\nEscolha a sala (número): ");
                if (!int.TryParse(Console.ReadLine(), out int salaIdx) || salaIdx < 1 || salaIdx > salas.Count)
                {
                    Console.WriteLine("Sala inválida! Pressione qualquer tecla para voltar ao menu...");
                    Console.ReadKey();
                    return;
                }
                Sala salaEscolhida = salas[salaIdx - 1];

                Console.Write("Data da Reserva (DD/MM/AAAA): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime dataReserva))
                {
                    Console.WriteLine("Data inválida! Pressione qualquer tecla para voltar ao menu...");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Horário de Início (HH:mm): ");
                if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan inicio))
                {
                    Console.WriteLine("Horário de início inválido! Pressione qualquer tecla para voltar ao menu...");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Horário de Fim (HH:mm): ");
                if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan fim) || fim <= inicio)
                {
                    Console.WriteLine("Horário de fim inválido ou anterior ao início! Pressione qualquer tecla para voltar ao menu...");
                    Console.ReadKey();
                    return;
                }

                DateTime inicioReserva = dataReserva.Date.Add(inicio);
                DateTime fimReserva = dataReserva.Date.Add(fim);

                if (inicioReserva < DateTime.Now)
                {
                    Console.WriteLine("Não é possível criar reservas no passado! Pressione qualquer tecla para voltar ao menu...");
                    Console.ReadKey();
                    return;
                }

                Reserva novaReserva = new Reserva
                {
                    Id = $"RES-" + Guid.NewGuid().ToString().Substring(0, 4).ToUpper(),
                    Sala = salaEscolhida,
                    Usuario = usuario,
                    Inicio = inicioReserva,
                    Fim = fimReserva,
                    Detalhes = "Reserva criada via terminal"
                };

                while (true)
                {
                    Console.Write("\nDeseja adicionar extras? (s/n): ");
                    string? resposta = Console.ReadLine()?.Trim().ToLower();
                    if (resposta == "s")
                    {
                        Console.WriteLine("1. Equipamento Multimídia");
                        Console.WriteLine("2. Serviço de Limpeza");
                        Console.Write("Escolha (número): ");
                        string? extra = Console.ReadLine();
                        switch (extra)
                        {
                            case "1":
                                novaReserva = new EquipamentoMultimidiaDecorator(novaReserva);
                                break;
                            case "2":
                                novaReserva = new ServicoDeLimpezaDecorator(novaReserva);
                                break;
                            default:
                                Console.WriteLine("Opção inválida! Pressione qualquer tecla para tentar novamente...");
                                Console.ReadKey();
                                continue;
                        }
                    }
                    else if (resposta == "n")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Resposta inválida! Pressione qualquer tecla para tentar novamente...");
                        Console.ReadKey();
                    }
                }

                Console.WriteLine("\n[SISTEMA] Tentando processar a reserva...\n");
                if (politica.AprovarReserva(novaReserva, gerenciador.GetReservas()))
                {
                    gerenciador.AdicionarReserva(novaReserva);
                    Console.WriteLine("[SUCESSO] Reserva aprovada e adicionada com sucesso!");
                    Console.WriteLine($"Descrição: {novaReserva.GetDescricao()}");
                }
                else
                {
                    Console.WriteLine("[ERRO] Conflito de horário detectado!");
                }

                Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
                Console.ReadKey();
            }

            static void ListarReservas(IGerenciadorDeReservas gerenciador)
            {
                Console.Clear();
                Console.WriteLine("--- RESERVAS ATIVAS ---");
                var reservas = gerenciador.GetReservas();
                if (reservas.Count == 0)
                {
                    Console.WriteLine("Nenhuma reserva ativa.");
                }
                else
                {
                    foreach (var r in reservas)
                    {
                        string tipoUsuario = r.Usuario.IsDocente ? "Docente" : "Aluno";
                        Console.WriteLine($"ID: {r.Id} | Sala: {r.Sala.Id} | Usuário: {r.Usuario.Nome} ({tipoUsuario})");
                        Console.WriteLine($"   Data: {r.Inicio:dd/MM/yyyy} | Horário: {r.Inicio:HH:mm} - {r.Fim:HH:mm}");
                        Console.WriteLine($"   Detalhes: {r.GetDescricao()}");
                        Console.WriteLine("-----------------------------------");
                    }
                }
                Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
                Console.ReadKey();
            }

            static void CancelarReserva(IGerenciadorDeReservas gerenciador)
            {
                Console.Clear();
                Console.WriteLine("--- CANCELAR RESERVA ---");
                var reservas = gerenciador.GetReservas();
                if (reservas.Count == 0)
                {
                    Console.WriteLine("Nenhuma reserva ativa para cancelar.");
                    Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
                    Console.ReadKey();
                    return;
                }

                for (int i = 0; i < reservas.Count; i++)
                {
                    var r = reservas[i];
                    string tipoUsuario = r.Usuario.IsDocente ? "Docente" : "Aluno";
                    Console.WriteLine($"{i + 1}. ID: {r.Id} | Sala: {r.Sala.Id} | Usuário: {r.Usuario.Nome} ({tipoUsuario}) | Horário: {r.Inicio:HH:mm} - {r.Fim:HH:mm}");
                }
                Console.Write("\nEscolha a reserva para cancelar (número): ");
                if (!int.TryParse(Console.ReadLine(), out int reservaIdx) || reservaIdx < 1 || reservaIdx > reservas.Count)
                {
                    Console.WriteLine("Reserva inválida! Pressione qualquer tecla para voltar ao menu...");
                    Console.ReadKey();
                    return;
                }
                Reserva reservaSelecionada = reservas[reservaIdx - 1];
                gerenciador.CancelarReserva(reservaSelecionada);
                Console.WriteLine("[SUCESSO] Reserva cancelada com sucesso!");
                Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
                Console.ReadKey();
            }

            static IPoliticaDeReserva AlterarPolitica(IPoliticaDeReserva politicaAtual)
            {
                Console.Clear();
                Console.WriteLine("--- ALTERAR POLÍTICA DE RESERVA ---");
                Console.WriteLine("1. Primeiro a Chegar");
                Console.WriteLine("2. Prioridade para Docentes");
                Console.Write("\nEscolha a nova política (número): ");
                string? opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        Console.WriteLine("Política alterada para: Primeiro a Chegar");
                        return new PoliticaPrimeiroChegar();
                    case "2":
                        Console.WriteLine("Política alterada para: Prioridade Docentes");
                        return new PoliticaPrioridadeDocente();
                    default:
                        Console.WriteLine("Opção inválida! Mantendo política atual. Pressione qualquer tecla para voltar ao menu...");
                        Console.ReadKey();
                        return politicaAtual;
                }
            }

            static void EmitirRelatorioDiario(IGerenciadorDeReservas gerenciador)
            {
                Console.Clear();
                Console.WriteLine("--- RELATÓRIO DIÁRIO DE RESERVAS ---");
                Console.Write("Informe a data para o relatório (DD/MM/AAAA): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime dataRelatorio))
                {
                    Console.WriteLine("Data inválida! Pressione qualquer tecla para voltar ao menu...");
                    Console.ReadKey();
                    return;
                }
                var reservasDoDia = gerenciador.GetReservas().Where(r => r.Inicio.Date == dataRelatorio.Date).ToList();
                Console.WriteLine($"\nReservas para {dataRelatorio:dd/MM/yyyy}:");
                Console.WriteLine("-----------------------------------");
                if (reservasDoDia.Count == 0)
                {
                    Console.WriteLine("Nenhuma reserva para esta data.");
                }
                else
                {
                    foreach (var r in reservasDoDia)
                    {
                        string tipoUsuario = r.Usuario.IsDocente ? "Docente" : "Aluno";
                        Console.WriteLine($"ID: {r.Id} | Sala: {r.Sala.Id} | Usuário: {r.Usuario.Nome} ({tipoUsuario})");
                        Console.WriteLine($"   Horário: {r.Inicio:HH:mm} - {r.Fim:HH:mm}");
                        Console.WriteLine($"   Detalhes: {r.GetDescricao()}");
                        Console.WriteLine("-----------------------------------");
                    }
                }
                Console.WriteLine("---------------------------------");
                Console.WriteLine("Total de Reservas: " + reservasDoDia.Count);
                Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
                Console.ReadKey();
            }
        }
    }
}
