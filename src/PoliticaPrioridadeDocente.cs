using System.Collections.Generic;

namespace ReservaDeSalas
{
    public class PoliticaPrioridadeDocente :  IPoliticaDeReserva
    {
        public bool AprovarReserva(Reserva nova, List<Reserva> reservasExistentes)
        {
            //se for docente, ignora a colisão e aprova a reserva da sala, sobrescrevendo a reserva do aluno
            if (nova.Usuario != null && nova.Usuario.IsDocente)
            {
                return true;
            }

            //se for aluno, aplica a regra normal de não colisão
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