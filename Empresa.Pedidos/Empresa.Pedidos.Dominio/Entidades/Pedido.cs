using Empresa.Pedidos.Dominio.ObjetosValor;
using Kite.Base.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Empresa.Pedidos.Dominio.Entidades
{
    public class Pedido : EntidadeBase
    {
        public Cliente              Cliente     { get; set; }
        public DateTime             DataPedido  { get; set; }
        public PedidoStatus         Status      { get; set; }
        public IList<PedidoItem>    Itens       { get; set; }

        public decimal Total
        {
            get
            {
                return Itens == null ? 0 : Itens.Sum(x => x.TotalItem);
            }
        }

        public bool TemLimiteCredito()
        {
            throw new NotImplementedException();
        }

        public bool TemEstoqueDisponivel()
        {
            throw new NotImplementedException();
        }

        public void AbateQuantidadeDoEstoque()
        {
            throw new NotImplementedException();
        }
    }
}