using System.Collections.Generic;

namespace ReservaDeSalas
{
    public class PoliticaPrimeiroChegar : IPoliticaDeReserva
    {
        public bool AprovarReserva(Reserva nova, List<Reserva> reservasExistentes)
        {
            foreach (var r in reservasExistentes)
            {
                if (r.Sala.Id == nova.Sala.Id && nova.Inicio < r.Fim && nova.Fim > r.Inicio)
                {
                    return false; //detectada colisão, portanto n pode reservar a sala 
                }
            }

            return true; 
        }
    }
}