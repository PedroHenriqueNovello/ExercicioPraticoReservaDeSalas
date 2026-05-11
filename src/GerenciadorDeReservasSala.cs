using System.Collections.Generic;

namespace ReservaDeSalas
{
    public class GerenciadorDeReservasSala : ISubject
    {
        private GerenciadorDeReservasSala() { }
        private static GerenciadorDeReservasSala instance;
        private static readonly object _trava = new object();
        private List<Reserva> _reservas = new List<Reserva>();

        // Lista de observadores
        private List<IObserver> _observers = new List<IObserver>();


        public static GerenciadorDeReservasSala getInstance()
        {
            lock (_trava)
            {
                if (instance == null) instance = new GerenciadorDeReservasSala();
                return instance;
            }
        }

        // Métodos do ISubject
        public void AddObserver(IObserver observer) => _observers.Add(observer);
        public void RemoveObserver(IObserver observer) => _observers.Remove(observer);
        
        public void NotifyObservers(Reserva reserva)
        {
            foreach (var obs in _observers) obs.Update(this, reserva);
        }

        
        public int GetTotalReservasAtivas() => _reservas.Count;

        public void AdicionarReserva(Reserva r) => _reservas.Add(r);
        public List<Reserva> GetReservas() => _reservas;
    }
}
