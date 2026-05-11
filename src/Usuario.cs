using System;

namespace ReservaDeSalas
{
    public class Usuario : IObserver
    {
        public string Nome { get; private set; }

        public Usuario(string nome) => Nome = nome;

        public void Update(ISubject subject, Reserva reserva)
        {
            Console.WriteLine($"Processando atualização para o usuário: {Nome}");
        }
    }
}