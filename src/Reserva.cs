using System;

namespace ReservaDeSalas
{
    public class Reserva 
    { 
        public virtual string Id { get; set; } = string.Empty;
        public virtual string Detalhes { get; set; } = string.Empty;

        public virtual Sala Sala { get; set; } = null!;

        public virtual Usuario Usuario {get; set;} = null!;
        public virtual DateTime Inicio {get; set;} = DateTime.Now;
        public virtual DateTime Fim {get; set;} = DateTime.Now.AddHours(1);
        public virtual string GetDescricao() => Detalhes; 
    }

    public abstract class ReservaDecorator : Reserva
    {
        protected Reserva _reserva;

        public ReservaDecorator(Reserva reserva)
        {
            this._reserva = reserva;
        }

        public override string Id { get => _reserva.Id; set => _reserva.Id = value; }
        public override string Detalhes { get => _reserva.Detalhes; set => _reserva.Detalhes = value; }
        public override Sala Sala { get => _reserva.Sala; set => _reserva.Sala = value; }
        public override Usuario Usuario { get => _reserva.Usuario; set => _reserva.Usuario = value; }
        public override DateTime Inicio { get => _reserva.Inicio; set => _reserva.Inicio = value; }
        public override DateTime Fim { get => _reserva.Fim; set => _reserva.Fim = value; }

        public override string GetDescricao() => _reserva.GetDescricao();
    }

    public class EquipamentoMultimidiaDecorator : ReservaDecorator
    {
        public EquipamentoMultimidiaDecorator(Reserva reserva) : base(reserva) { }
        public override string GetDescricao() => base.GetDescricao() + " + Equipamento Multimídia";
    }

    public class ServicoDeLimpezaDecorator : ReservaDecorator
    {
        public ServicoDeLimpezaDecorator(Reserva reserva) : base(reserva) { }
        public override string GetDescricao() => base.GetDescricao() + " + Serviço de Limpeza";
    }
}