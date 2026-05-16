using System;
using System.Text.Json.Serialization;

namespace ReservaDeSalas
{
    public enum NivelAcesso { Aluno, Docente, Admin }

    public class Usuario : IObserver
    {
        public string Nome { get; set; }
        public string Senha { get; set; }
        public bool IsDocente { get; set; }
        public NivelAcesso Nivel { get; set; }

        [JsonConstructor]
        public Usuario(string nome, string senha)
        {
            Nome = nome;
            Senha = senha;
            Nivel = NivelAcesso.Aluno;
        }

        public void Update(ISubject subject, Reserva reserva)
        {
            Console.WriteLine($"[NOTIFICAÇÃO] Para {Nome}: A reserva '{reserva.Id}' na sala '{reserva.Sala.Id}' ({reserva.Sala.GetTipo()}) foi atualizada/cancelada.");
        }
    }
}
