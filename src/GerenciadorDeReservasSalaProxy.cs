using System;
using System.Collections.Generic;

namespace ReservaDeSalas
{
    public class GerenciadorDeReservasSala : IGerenciadorDeReservas
    {
        protected readonly IGerenciadorDeReservas _gerenciadorReal;

        public GerenciadorDeReservasSalaProxy(IGerenciadorDeReservas gerenciadorReal)
        {
            _gerenciadorReal = gerenciadorReal;
        }

        public virtual void AdicionarReserva(Reserva r) => _gerenciadorReal.AdicionarReserva(r);
        public virtual void CancelarReserva(Reserva r) => _gerenciadorReal.CancelarReserva(r);
        public virtual List<Reserva> GetReservas() => _gerenciadorReal.GetReservas();
        public int GetTotalReservasAtivas() => _gerenciadorReal.GetTotalReservasAtivas();
        public void AddObserver(IObserver observer) => _gerenciadorReal.AddObserver(observer);
        public void RemoveObserver(IObserver observer) => _gerenciadorReal.RemoveObserver(observer);
        public void NotifyObservers(Reserva reserva) => _gerenciadorReal.NotifyObservers(reserva);
    }
}