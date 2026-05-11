using System;

namespace ReservaDeSalas
{
    public class Usuario : IObserver
    {
        public string Nome { get; private set; }

        public Usuario(string nome) => Nome = nome;

        public void Update(ISubject subject, Reserva reserva)
        {
            // PUSH
            Console.WriteLine($"Para {Nome}: A reserva '{reserva.Id}' na sala tipo '{reserva.Sala.GetTipo()}' foi atualizada.");

            // PULL
            int total = subject.GetTotalReservasAtivas();
            Console.WriteLine($"O sistema agora possui {total} reserva(s) no total.");
        }
    }
}