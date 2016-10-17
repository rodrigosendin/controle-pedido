using Empresa.Pedidos.Dominio.ObjetosValor;
using Kite.Base.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Empresa.Pedidos.Dominio.Entidades
{
    public class Pedido : EntidadeBase, IAggregateRoot
    {
        public const decimal TOTAL_MINIMO = 100.00M;

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

        public Pedido()
        {
            Itens = new List<PedidoItem>();
        }

        public bool TemLimiteCredito()
        {
            return Cliente == null ? false : Cliente.LimiteCredito >= Total;
        }

        public bool TemEstoqueDisponivel(List<string> msgs)
        {
            var temEstoque = true;
            foreach(var item in Itens)
            {
                if(!item.TemEstoqueDisponivel())
                {
                    temEstoque = false;
                    msgs.Add(string.Format("Produto {0} não tem Estoque Disponível!", item.Produto));
                }
            }
            return temEstoque;
        }

        public bool TemTotalMinimo()
        {
            return Total >= TOTAL_MINIMO;
        }

        public void AbateQuantidadeDoEstoque()
        {
            throw new NotImplementedException();
        }

        public bool Valida(List<string> msgs)
        {
            var valido = true;

            if (!TemLimiteCredito())
            {
                valido = false;
                msgs.Add(string.Format("Cliente {0} não tem Limite de Crédito!", Cliente));                
            }

            if (!TemEstoqueDisponivel(msgs))
                valido = false;

            if (!TemTotalMinimo())
            {
                valido = false;
                msgs.Add(string.Format("Pedido não atingiu o Total mínimo de R$ {0}", TOTAL_MINIMO));
            }

            return valido;
        }

        public override string ToString()
        {
            return string.Format("[{0}] - {1} - {2:dd/MM/yyyy}", Id, Cliente, DataPedido);
        }
    }
}