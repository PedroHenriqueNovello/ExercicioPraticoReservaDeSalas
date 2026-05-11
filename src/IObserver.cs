namespace ReservaDeSalas
{
    public interface IObserver
    {
        void Update(ISubject subject, Reserva reserva);
    }
}