using System.Collections.Generic;

namespace ReservaDeSalas
{
    public interface IGerenciadorDeReservas : ISubject
    {
        void AdicionarReserva(Reserva r);
        void CancelarReserva(Reserva r);
        List<Reserva> GetReservas();
    }
}