using System;

namespace ReservaDeSalas
{
    public abstract class Sala
    {
        public string Id { get; protected set; }
        protected Sala(string id) { Id = id; }
        public abstract string GetTipo();
    }

    // Instanciando 3 subclasses de sala 
    public class SalaEstudoIndividual : Sala
    {
        public SalaEstudoIndividual(string id) : base(id) { }
        public override string GetTipo() => "Estudo Individual";
    }

    public class SalaTrabalhoGrupo : Sala
    {
        public SalaTrabalhoGrupo(string id) : base(id) { }
        public override string GetTipo() => "Trabalho em Grupo";
    }

    public class SalaLaboratorio : Sala
    {
        public SalaLaboratorio(string id) : base(id) { }
        public override string GetTipo() => "Laboratório";
    }

    public abstract class SalaFactory
    {
        public abstract Sala CriarSala(string id);
    }

    // Fábricas concretas
    public class SalaIndividualFactory : SalaFactory
    {
        public override Sala CriarSala(string id) => new SalaEstudoIndividual(id);
    }

    public class SalaGrupoFactory : SalaFactory
    {
        public override Sala CriarSala(string id) => new SalaTrabalhoGrupo(id);
    }

    public class SalaLaboratorioFactory : SalaFactory
    {
        public override Sala CriarSala(string id) => new SalaLaboratorio(id);
    }
}