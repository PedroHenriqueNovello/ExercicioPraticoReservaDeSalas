namespace ReservaDeSalas
{
    public interface ISubject
    {
        void AddObserver(IObserver observer);
        void RemoveObserver(IObserver observer);
        void NotifyObservers(Reserva reserva);
        int GetTotalReservasAtivas();
    }
}