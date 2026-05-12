using System;

namespace ReservaDeSalas
{
    public class Reserva 
    { 
        public string Id { get; set; }
        public string Detalhes { get; set; } 

        public Sala Sala { get; set; }

        public Usuario Usuario {get; set;}
        public DateTime Inicio {get; set;}
        public DateTime Fim {get; set;} 
    }
}