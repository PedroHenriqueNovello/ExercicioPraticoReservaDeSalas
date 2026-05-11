using System;

namespace ReservaDeSalas
{
    public class Reserva 
    { 
        public string Id { get; set; }
        public string Detalhes { get; set; } 
        public Sala Sala { get; set; }
        public DateTime DataHora { get; set; }
    }
}