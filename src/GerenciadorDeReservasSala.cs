using System.Collections.Generic;
using System.Linq;

namespace ReservaDeSalas
{
    public class GerenciadorDeReservasSala : IGerenciadorDeReservas
    {
        private GerenciadorDeReservasSala() { }
        private static GerenciadorDeReservasSala instance = null!;
        private static readonly object _trava = new object();
        private List<Reserva> _reservas = new List<Reserva>();
        private List<IObserver> _observers = new List<IObserver>();

        public static GerenciadorDeReservasSala GetInstance()
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
        }

        public int GetTotalReservasAtivas() => _reservas.Count;

        public bool AdicionarReserva(Reserva r, Usuario usuarioLogado) 
        {
            _reservas.Add(r);
            NotifyObservers(r);
            return true;
        }

        public bool CancelarReserva(Reserva r, Usuario usuarioLogado)
        {
            if (_reservas.Remove(r))
            {
                r.Detalhes = "CANCELADA";
                NotifyObservers(r);
                return true;
            }
            return false;
        }

        public List<Reserva> GetReservas() => _reservas;
    }
}
