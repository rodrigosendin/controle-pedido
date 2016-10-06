using Kite.Base.Dominio.Entidades;

namespace Empresa.Pedidos.Dominio.Entidades
{
    public class Produto : EntidadeBase
    {
        public string   Nome    { get; set; }
        public decimal  Valor   { get; set; }
        public int      Estoque { get; set; }
    }
}

