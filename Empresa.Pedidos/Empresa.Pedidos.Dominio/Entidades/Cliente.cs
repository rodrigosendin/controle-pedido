using Empresa.Pedidos.Dominio.ObjetosValor;
using Kite.Base.Dominio.Entidades;

namespace Empresa.Pedidos.Dominio.Entidades
{
    public class Cliente : EntidadeBase
    {
        public string       Nome            { get; set; }
        public string       Email           { get; set; }
        public string       Telefone        { get; set; }
        public decimal      LimiteCredito   { get; set; }
        public Localidade   Localidade      { get; set; }
    }
}
