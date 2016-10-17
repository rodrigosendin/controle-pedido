using Empresa.Pedidos.Dominio.Entidades;
using Kite.Base.Dominio.Servicos;
using Kite.Base.Dominio.Repositorio;
using System.Collections.Generic;
using System;

namespace Empresa.Pedidos.Dominio
{
    public class ServicoPedido : Servico<Pedido>
    {
        public ServicoPedido(IRepositorioHelper helper) : base(helper)
        {
        }

        public override bool Valida(Pedido entidade)
        {
            Mensagens = new List<string>();
            var ok = entidade.Valida(Mensagens);
            return ok;
        }

        public bool AbateEstoque(long pedidoId)
        {
            var pedido = Retorna(pedidoId);

            using (var sessao = Helper.AbrirSessao())
            {
                var repo = sessao.GetRepositorio<Produto>();
                try
                {
                    sessao.IniciaTransacao();
                    foreach(var item in pedido.Itens)
                    {
                        if (item.Produto.Estoque < item.Quantidade)
                            throw new Exception(string.Format("Produto {0} não tem estoque disponível!", item.Produto));

                        item.Produto.Estoque = item.Produto.Estoque - item.Quantidade;
                        repo.Altera(item.Produto);
                    }
                    sessao.ComitaTransacao();
                }
                catch (Exception ex)
                {
                    sessao.RollBackTransacao();
                    Mensagens.Add(ex.Message);
                    return false;
                }
            }
            return true;
        }
    }
}
