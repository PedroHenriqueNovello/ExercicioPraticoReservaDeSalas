using System.Collections.Generic;
using System.Linq;

namespace ReservaDeSalas
{
    public class PoliticaPrioridadeDocente :  IPoliticaDeReserva
    {
        public bool AprovarReserva(Reserva nova, List<Reserva> reservasExistentes)
        {
            if (nova.Usuario != null && nova.Usuario.IsDocente)
            {
                var conflitos = reservasExistentes.Where(r => r.Sala.Id == nova.Sala.Id &&nova.Inicio < r.Fim &&nova.Fim > r.Inicio && !r.Usuario.IsDocente).ToList();

                foreach (var reservaConflitante in conflitos)
                {
                    reservasExistentes.Remove(reservaConflitante);
                    reservaConflitante.Detalhes = $"CANCELADA POR PRIORIDADE DOCENTE (Reserva {nova.Id})";
                }

                return true;
            }

            foreach (var r in reservasExistentes)
            {
                if (r.Sala.Id == nova.Sala.Id && nova.Inicio < r.Fim && nova.Fim > r.Inicio)
                {
                    return false;
                }
            }
            return true;
        }
        
    }
}