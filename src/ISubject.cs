namespace ReservaDeSalas
{
    public interface ISubject
    {
        void AddObserver(IObserver observer);
        void RemoveObserver(IObserver observer);
        void NotifyObservers(Reserva reserva);
        
        // Método para permitir o PULL de dados
        int GetTotalReservasAtivas();
    }
}