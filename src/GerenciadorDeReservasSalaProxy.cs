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

        public virtual void AdicionarReserva(Reserva r)
        {
            if (r.Usuario.Nivel == NivelAcesso.Admin || r.Usuario.Nivel == NivelAcesso.Docente || r.Usuario.Nivel == NivelAcesso.Aluno)
            {
                Console.WriteLine($"[PROXY] Acesso autorizado para adição.");
                _gerenciadorReal.AdicionarReserva(r);
            }
            else
            {
                Console.WriteLine($"[PROXY] Acesso negado para adição.");
            }
        }
        public virtual void CancelarReserva(Reserva r)
        {
            if (r.Usuario.Nivel == NivelAcesso.Admin || r.Usuario.Nivel == NivelAcesso.Docente)
            {
                Console.WriteLine($"[PROXY] Acesso autorizado para cancelamento.");
                _gerenciadorReal.CancelarReserva(r);
            }
            else
            {
                Console.WriteLine($"[PROXY] Acesso negado para cancelamento.");
            }
        }
        public virtual List<Reserva> GetReservas() => _gerenciadorReal.GetReservas();
        public int GetTotalReservasAtivas() => _gerenciadorReal.GetTotalReservasAtivas();
        public void AddObserver(IObserver observer) => _gerenciadorReal.AddObserver(observer);
        public void RemoveObserver(IObserver observer) => _gerenciadorReal.RemoveObserver(observer);
        public void NotifyObservers(Reserva reserva) => _gerenciadorReal.NotifyObservers(reserva);
    }
}