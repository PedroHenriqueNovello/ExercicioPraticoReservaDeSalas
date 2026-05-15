using System.Collections.Generic;

namespace ReservaDeSalas
{
    public interface IGerenciadorDeReservas
    {
        void AdicionarReserva(Reserva r);
        void CancelarReserva(Reserva r);
        List<Reserva> GetReservas();
        int GetTotalReservasAtivas();
    }
}