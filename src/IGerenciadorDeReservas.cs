using System.Collections.Generic;

namespace ReservaDeSalas
{
    public interface IGerenciadorDeReservas : ISubject
    {
        bool AdicionarReserva(Reserva r, Usuario usuarioLogado);
        bool CancelarReserva(Reserva r, Usuario usuarioLogado);
        List<Reserva> GetReservas();
    }
}
