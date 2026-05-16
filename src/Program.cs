using System;
using System.Collections.Generic;
using System.Linq;

namespace ReservaDeSalas
{
    class Program
    {
        private static Usuario? _usuarioLogado = null;
        private static UsuarioService _usuarioService = new UsuarioService();

        static void Main(string[] args)
        {
            IGerenciadorDeReservas gerenciadorReal = GerenciadorDeReservasSala.GetInstance();
            IGerenciadorDeReservas gerenciador = new GerenciadorDeReservasSalaProxy(gerenciadorReal);

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
                if (_usuarioLogado == null)
                {
                    MenuLogin();
                    continue;
                }

                Console.Clear();
                Console.WriteLine("=====================================");
                Console.WriteLine("  SISTEMA DE RESERVA DE SALAS - V2   ");
                Console.WriteLine("=====================================");
                Console.WriteLine($"Usuário: {_usuarioLogado.Nome} ({_usuarioLogado.Nivel})");
                Console.WriteLine($"Política Atual: {politica.GetType().Name}");
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("1. Criar Nova Reserva");
                Console.WriteLine("2. Listar Reservas Atuais");
                Console.WriteLine("3. Cancelar Reserva");
                Console.WriteLine("4. Alterar Política de Reserva");
                Console.WriteLine("5. Emitir Relatório Diário");
                Console.WriteLine("6. Logout");
                Console.WriteLine("7. Sair");
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
                        _usuarioLogado = null;
                        Console.WriteLine("Logout realizado com sucesso!");
                        break;
                    case "7":
                        return;
                    default:
                        Console.WriteLine("Opção inválida!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void MenuLogin()
        {
            Console.Clear();
            Console.WriteLine("=== BEM-VINDO AO SISTEMA DE RESERVAS ===");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Registrar Novo Usuário");
            Console.WriteLine("3. Sair");
            Console.Write("\nEscolha: ");

            string? opt = Console.ReadLine();
            if (opt == "1")
            {
                Console.Write("Usuário: ");
                string? nome = Console.ReadLine();
                Console.Write("Senha: ");
                string? senha = Console.ReadLine();
                _usuarioLogado = _usuarioService.Autenticar(nome ?? "", senha ?? "");
                if (_usuarioLogado == null)
                {
                    Console.WriteLine("Credenciais inválidas! Pressione qualquer tecla...");
                    Console.ReadKey();
                }
            }
            else if (opt == "2")
            {
                Console.Write("Novo Usuário: ");
                string? nome = Console.ReadLine();
                Console.Write("Senha: ");
                string? senha = Console.ReadLine();
                Console.WriteLine("Nível (0: Aluno, 1: Docente, 2: Admin): ");
                if (int.TryParse(Console.ReadLine(), out int nivel) && Enum.IsDefined(typeof(NivelAcesso), nivel))
                {
                    if (_usuarioService.Registrar(nome ?? "", senha ?? "", (NivelAcesso)nivel))
                        Console.WriteLine("Usuário registrado! Faça login.");
                    else
                        Console.WriteLine("Erro: Usuário já existe.");
                }
                else
                {
                    Console.WriteLine("Nível inválido.");
                }
                Console.ReadKey();
            }
            else if (opt == "3") Environment.Exit(0);
            else
            {
                Console.WriteLine("Opção inválida! Pressione qualquer tecla...");
                Console.ReadKey();
            }
        }

        static void CriarReserva(IGerenciadorDeReservas gerenciador, List<Sala> salas, IPoliticaDeReserva politica)
        {
            Console.Clear();
            Console.WriteLine("--- NOVA RESERVA ---");
            
            Console.WriteLine("Salas Disponíveis:");
            for (int i = 0; i < salas.Count; i++)
                Console.WriteLine($"{i + 1}. {salas[i].Id} ({salas[i].GetTipo()})");

            Console.Write("\nEscolha a sala: ");
            if (!int.TryParse(Console.ReadLine(), out int salaIdx) || salaIdx < 1 || salaIdx > salas.Count)
            {
                Console.WriteLine("Sala inválida!");
                Console.ReadKey();
                return;
            }
            Sala salaEscolhida = salas[salaIdx - 1];

            Console.Write("Data (DD/MM/AAAA): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime data))
            {
                Console.WriteLine("Data inválida!");
                Console.ReadKey();
                return;
            }
            Console.Write("Início (HH:mm): ");
            if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan inicio))
            {
                Console.WriteLine("Horário de início inválido!");
                Console.ReadKey();
                return;
            }
            Console.Write("Fim (HH:mm): ");
            if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan fim) || fim <= inicio)
            {
                Console.WriteLine("Horário de fim inválido ou anterior ao início!");
                Console.ReadKey();
                return;
            }

            DateTime inicioReserva = data.Date.Add(inicio);
            DateTime fimReserva = data.Date.Add(fim);

            if (inicioReserva < DateTime.Now)
            {
                Console.WriteLine("Erro: Não é possível reservar no passado.");
                Console.ReadKey();
                return;
            }

            Reserva nova = new Reserva
            {
                Id = "RES-" + Guid.NewGuid().ToString().Substring(0, 4).ToUpper(),
                Sala = salaEscolhida,
                Usuario = _usuarioLogado!,
                Inicio = inicioReserva,
                Fim = fimReserva,
                Detalhes = "Reserva via terminal"
            };

            // Aplicação de Extras (Decorator)
            Console.Write("Adicionar Extras? (s/n): ");
            string? respostaExtra = Console.ReadLine()?.Trim().ToLower();
            if (respostaExtra == "s")
            {
                Console.WriteLine("1. Equipamento Multimídia, 2. Serviço de Limpeza");
                string? extra = Console.ReadLine();
                if (extra == "1") nova = new EquipamentoMultimidiaDecorator(nova);
                else if (extra == "2") nova = new ServicoDeLimpezaDecorator(nova);
                else Console.WriteLine("Opção de extra inválida. Nenhum extra adicionado.");
            }

            if (politica.AprovarReserva(nova, gerenciador.GetReservas()))
            {
                if (gerenciador.AdicionarReserva(nova, _usuarioLogado!))
                {
                    Console.WriteLine("\n[SUCESSO] Reserva realizada!");
                    Console.WriteLine($"Descrição: {nova.GetDescricao()}");
                }
            }
            else Console.WriteLine("\n[ERRO] Conflito de horário ou política não aprovada!");

            Console.ReadKey();
        }

        static void ListarReservas(IGerenciadorDeReservas gerenciador)
        {
            Console.Clear();
            Console.WriteLine("--- RESERVAS ATIVAS ---");
            var reservas = gerenciador.GetReservas();
            if (!reservas.Any())
            {
                Console.WriteLine("Nenhuma reserva ativa.");
            }
            else
            {
                foreach (var r in reservas)
                {
                    Console.WriteLine($"ID: {r.Id} | Sala: {r.Sala.Id} | Usuário: {r.Usuario.Nome}");
                    Console.WriteLine($"Horário: {r.Inicio:dd/MM HH:mm} - {r.Fim:HH:mm}");
                    Console.WriteLine($"Descrição: {r.GetDescricao()}");
                    Console.WriteLine("-----------------------------------");
                }
            }
            Console.ReadKey();
        }

        static void CancelarReserva(IGerenciadorDeReservas gerenciador)
        {
            Console.Clear();
            Console.WriteLine("--- CANCELAR RESERVA ---");
            var reservas = gerenciador.GetReservas();

            if (!reservas.Any())
            {
                Console.WriteLine("Nenhuma reserva ativa para cancelar.");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < reservas.Count; i++)
                Console.WriteLine($"{i + 1}. {reservas[i].Id} - {reservas[i].Sala.Id} ({reservas[i].Usuario.Nome})");

            Console.Write("\nEscolha a reserva para cancelar (número): ");
            if (int.TryParse(Console.ReadLine(), out int idx) && idx > 0 && idx <= reservas.Count)
            {
                Reserva reservaParaCancelar = reservas[idx - 1];
                if (gerenciador.CancelarReserva(reservaParaCancelar, _usuarioLogado!))
                    Console.WriteLine("Cancelado com sucesso!");
                else
                    Console.WriteLine("Falha no cancelamento (verifique permissões ou se a reserva existe).");
            }
            else
            {
                Console.WriteLine("Opção inválida!");
            }
            Console.ReadKey();
        }

        static IPoliticaDeReserva AlterarPolitica(IPoliticaDeReserva atual)
        {
            if (_usuarioLogado?.Nivel != NivelAcesso.Admin)
            {
                Console.WriteLine("Apenas Administradores podem alterar a política do sistema!");
                Console.ReadKey();
                return atual;
            }
            Console.Clear();
            Console.WriteLine("1. Primeiro a Chegar\n2. Prioridade Docente");
            Console.Write("\nEscolha a nova política (número): ");
            string? opt = Console.ReadLine();
            
            switch (opt)
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
                    return atual;
            }
        }

        static void EmitirRelatorioDiario(IGerenciadorDeReservas gerenciador)
        {
            Console.Clear();
            Console.Write("Informe a data para o relatório (DD/MM/AAAA): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime dt))
            {
                var doDia = gerenciador.GetReservas().Where(r => r.Inicio.Date == dt.Date).ToList();
                Console.WriteLine($"Relatório {dt:dd/MM/yyyy} - Total: {doDia.Count}");
                if (!doDia.Any())
                {
                    Console.WriteLine("Nenhuma reserva para esta data.");
                }
                else
                {
                    foreach (var r in doDia) Console.WriteLine($"- {r.Id}: {r.Sala.Id} ({r.Usuario.Nome})");
                }
            }
            else
            {
                Console.WriteLine("Data inválida!");
            }
            Console.ReadKey();
        }
    }
}
