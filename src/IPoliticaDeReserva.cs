using System.Collections.Generic;

namespace ReservaDeSalas
{
    public interface IPoliticaDeReserva{
        bool AprovarReserva(Reserva novaReserva, List<Reserva> reservasExistentes);
    }
}