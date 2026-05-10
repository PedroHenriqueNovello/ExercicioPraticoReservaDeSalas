using System.Collections.Generic;

namespace ReservaDeSalas
{
    public class GerenciadorDeReservasSala
    {
        private GerenciadorDeReservasSala() { }
        private static GerenciadorDeReservasSala instance;
        private static readonly object _trava = new object();
        private List<Reserva> _reservas = new List<Reserva>();

        public static GerenciadorDeReservasSala getInstance()
        {
            lock (_trava)
            {
                if (instance == null) instance = new GerenciadorDeReservasSala();
                return instance;
            }
        }
        public void AdicionarReserva(Reserva r) => _reservas.Add(r);
        public List<Reserva> GetReservas() => _reservas;
    }
}
