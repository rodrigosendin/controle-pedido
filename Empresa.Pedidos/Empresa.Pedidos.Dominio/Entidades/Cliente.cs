using Empresa.Pedidos.Dominio.ObjetosValor;
using Kite.Base.Dominio.Entidades;

namespace Empresa.Pedidos.Dominio.Entidades
{
    public class Cliente : EntidadeBase, IAggregateRoot
    {
        public string       Nome            { get; set; }
        public string       Email           { get; set; }
        public string       Telefone        { get; set; }
        public decimal      LimiteCredito   { get; set; }
        public Localidade   Localidade      { get; set; }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(Nome) ? Nome : base.ToString();
        }
    }
}