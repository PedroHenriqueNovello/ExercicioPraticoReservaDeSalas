using System;
using System.Collections.Generic;
using System.Data;

namespace ReservaDeSalas
{
    public class GerenciadorDeReservasSalaProxy : IGerenciadorDeReservas
    {
        protected readonly IGerenciadorDeReservas _gerenciadorReal;
        private List<Reserva>? _cacheReservas;
        private DateTime _cacheExpiracao;

        public GerenciadorDeReservasSalaProxy(IGerenciadorDeReservas gerenciadorReal)
        {
            _gerenciadorReal = gerenciadorReal;
            _cacheExpiracao = DateTime.MinValue;
        }

        public virtual void AdicionarReserva(Reserva r)
        {
            if (r.Usuario.Nivel == NivelAcesso.Admin || r.Usuario.Nivel == NivelAcesso.Docente || r.Usuario.Nivel == NivelAcesso.Aluno)
            {
                Console.WriteLine($"[PROXY] Acesso autorizado para adição.");
                _gerenciadorReal.AdicionarReserva(r);
                InvalidarCache();
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
                InvalidarCache();
            }
            else
            {
                Console.WriteLine($"[PROXY] Acesso negado para cancelamento.");
            }
        }
        public virtual List<Reserva> GetReservas()
        {
            if(_cacheReservas != null && DateTime.Now < _cacheExpiracao)
            {
                Console.WriteLine("[PROXY] Retornando dados do CACHE.");
                return _cacheReservas;
            }
            Console.WriteLine("[PROXY] Consultando Gerenciador Real...");
            _cacheReservas = _gerenciadorReal.GetReservas();
            _cacheExpiracao = DateTime.Now.AddSeconds(10);
            return _cacheReservas;
        }

        private void InvalidarCache()
        {
            _cacheReservas = null;
            _cacheExpiracao = DateTime.MinValue;
        }
        public int GetTotalReservasAtivas() => _gerenciadorReal.GetTotalReservasAtivas();
        public void AddObserver(IObserver observer) => _gerenciadorReal.AddObserver(observer);
        public void RemoveObserver(IObserver observer) => _gerenciadorReal.RemoveObserver(observer);
        public void NotifyObservers(Reserva reserva) => _gerenciadorReal.NotifyObservers(reserva);
    }
}