using System;

namespace ReservaDeSalas
{
    public class Usuario : IObserver
    {
        public string Nome { get; private set; }
        public bool IsDocente { get; set; }
        public NivelAcesso Nivel { get; set; }

        public Usuario(string nome)
        {
            Nome = nome;
            Nivel = NivelAcesso.Aluno;
        }

        public void Update(ISubject subject, Reserva reserva)
        {
            Console.WriteLine($"Para {Nome}: A reserva '{reserva.Id}' na sala tipo '{reserva.Sala.GetTipo()}' foi atualizada.");
        }
    }
}