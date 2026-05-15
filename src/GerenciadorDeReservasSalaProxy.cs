using System;
using System.Collections.Generic;
using System.Linq;

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

        public virtual bool AdicionarReserva(Reserva r, Usuario usuarioLogado)
        {
            string tipoSala = r.Sala.GetTipo();
            bool autorizado = false;

            if (usuarioLogado.Nivel == NivelAcesso.Admin || usuarioLogado.Nivel == NivelAcesso.Docente)
            {
                autorizado = true;
            }
            else if (usuarioLogado.Nivel == NivelAcesso.Aluno)
            {
                if (tipoSala == "Estudo Individual" || tipoSala == "Trabalho em Grupo")
                    autorizado = true;
                else
                    Console.WriteLine("[PROXY] Erro: Alunos não podem reservar Laboratórios.");
            }

            if (autorizado)
            {
                Console.WriteLine($"[PROXY] Acesso autorizado para adição.");
                bool sucesso = _gerenciadorReal.AdicionarReserva(r, usuarioLogado);
                if (sucesso) InvalidarCache();
                return sucesso;
            }

            Console.WriteLine($"[PROXY] Acesso negado para adição de reserva na sala '{r.Sala.Id}'.");
            return false;
        }

        public virtual bool CancelarReserva(Reserva r, Usuario usuarioLogado)
        {
            bool autorizado = false;

            if (usuarioLogado.Nivel == NivelAcesso.Admin)
            {
                autorizado = true;
            }
            else if (r.Usuario.Nome.Equals(usuarioLogado.Nome, StringComparison.OrdinalIgnoreCase))
            {
                autorizado = true;
            }
            else
            {
                Console.WriteLine($"[PROXY] Erro: Você só pode cancelar suas próprias reservas.");
            }

            if (autorizado)
            {
                Console.WriteLine($"[PROXY] Acesso autorizado para cancelamento.");
                bool sucesso = _gerenciadorReal.CancelarReserva(r, usuarioLogado);
                if (sucesso) InvalidarCache();
                return sucesso;
            }

            return false;
        }

        public virtual List<Reserva> GetReservas()
        {
            if (_cacheReservas != null && DateTime.Now < _cacheExpiracao)
            {
                Console.WriteLine("[PROXY] Retornando dados do CACHE.");
                return _cacheReservas;
            }
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
