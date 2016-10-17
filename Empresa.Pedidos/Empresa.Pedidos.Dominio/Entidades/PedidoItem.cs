using Kite.Base.Dominio.Entidades;
using System;

namespace Empresa.Pedidos.Dominio.Entidades
{
    public class PedidoItem : EntidadeBase
    {
        public Pedido   Pedido      { get; set; }
        public Produto  Produto     { get; set; }
        public int      Quantidade  { get; set; }
        public decimal  Valor       { get; set; }

        public decimal  TotalItem
        {
            get
            {
                return Math.Round(Quantidade * Valor);
            }
        }

        public bool TemEstoqueDisponivel()
        {
            return Produto == null ? false : Produto.Estoque >= Quantidade;
        }

        public void AbateQuantidadeDoEstoque()
        {
            throw new NotImplementedException();
        }
    }
}