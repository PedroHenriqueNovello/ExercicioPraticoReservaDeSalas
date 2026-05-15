using System.Collections.Generic;
using System.Linq;

namespace ReservaDeSalas
{
    public class GerenciadorDeReservasSala : IGerenciadorDeReservas
    {
        private GerenciadorDeReservasSala() { }
        private static GerenciadorDeReservasSala instance;
        private static readonly object _trava = new object();
        private List<Reserva> _reservas = new List<Reserva>();
        private List<IObserver> _observers = new List<IObserver>();

        public static GerenciadorDeReservasSala getInstance()
        {
            lock (_trava)
            {
                if (instance == null) instance = new GerenciadorDeReservasSala();
                return instance;
            }
        }

        public void AddObserver(IObserver observer)
        {
            if (!_observers.Contains(observer)) _observers.Add(observer);
        }
        public void RemoveObserver(IObserver observer) => _observers.Remove(observer);
        
        public void NotifyObservers(Reserva reserva)
        {
            foreach (var obs in _observers) obs.Update(this, reserva);
            if (reserva.Usuario is IObserver criador && !_observers.Contains(criador))
            {
                criador.Update(this, reserva);
            }
        }

        
        public int GetTotalReservasAtivas() => _reservas.Count;

        public void AdicionarReserva(Reserva r) 
        {
            _reservas.Add(r);
            NotifyObservers(r);
        }
        public void CancelarReserva(Reserva r)
        {
            if (_reservas.Remove(r))
            {
                r.Detalhes = "CANCELADA";
                NotifyObservers(r);
            }
        }
        public List<Reserva> GetReservas() => _reservas;
    }
}
