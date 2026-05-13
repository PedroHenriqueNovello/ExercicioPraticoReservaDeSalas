using System;
using System.Collections.Generic;

namespace ReservaDeSalas
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=====================================");
            Console.WriteLine("  SISTEMA DE RESERVA DE SALAS - V1   ");
            Console.WriteLine("=====================================\n");

            // 1. SINGLETON: Instância única do Gerenciador
            var gerenciador = GerenciadorDeReservasSala.getInstance();

            // 2. FACTORY METHOD: Criando salas
            SalaFactory fabricaIndividual = new SalaIndividualFactory();
            Sala salaA1 = fabricaIndividual.CriarSala("A1");

            // 3. Criando os Usuários
            Usuario aluno = new Usuario("Pedro Henrique") { IsDocente = false };
            Usuario professor = new Usuario("Profa. Letícia") { IsDocente = true };

            // 4. OBSERVER: Inscrevendo usuários para receberem notificações
            gerenciador.AddObserver(aluno);
            gerenciador.AddObserver(professor);

            // 5. STRATEGY: Escolhendo a regra de colisão
            IPoliticaDeReserva politica = new PoliticaPrimeiroChegar();

            // 6. Simulando uma Reserva
            Reserva reserva1 = new Reserva
            {
                Id = "RES-001",
                Sala = salaA1,
                Usuario = aluno,
                Inicio = DateTime.Now,
                Fim = DateTime.Now.AddHours(2),
                Detalhes = "Estudos para a prova"
            };

            Console.WriteLine("[SISTEMA] Tentando processar a reserva...\n");

            // Validando a reserva com a Política de Colisão
            if (politica.AprovarReserva(reserva1, gerenciador.GetReservas()))
            {
                // Se aprovada, o gerenciador adiciona e notifica os Observers automaticamente!
                gerenciador.AdicionarReserva(reserva1);
            }
            else
            {
                Console.WriteLine("[ERRO] Conflito de horário detectado!");
            }

            Console.WriteLine("\n[SISTEMA] Pressione qualquer tecla para fechar...");
            Console.ReadKey();
        }
    }
}