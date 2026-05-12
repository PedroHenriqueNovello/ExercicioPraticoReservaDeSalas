namespace ReservaDeSalas
{
    public class Usuario
    {
        public string Nome {get; set;}
        public bool IsDocente {get; set;}
    }
    public class Reserva 
    { 
        public string Id { get; set; }
        public string Detalhes { get; set; }

        public Sala Sala {get; set;}
        public Usuario Usuario {get; set;}
        public DateTime Inicio {get; set;}
        public DateTime Fim {get; set;} 
    }

    
}