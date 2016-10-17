using Empresa.Pedidos.Dominio.Entidades;
using NUnit.Framework;
using System.Collections.Generic;
using System;

namespace Empresa.Pedidos.Testes.Modelo
{
    [TestFixture]
    public class TestePedidoModelo
    {
        [Test]
        public void Valida_Pedido_De_Cliente_Com_Limite_Credito()
        {
            // PREPARAÇÃO
            var cliente = new Cliente { LimiteCredito = 1200.00M };
            var pedido = new Pedido { Cliente = cliente };
            var item = new PedidoItem
            {
                Quantidade = 2,
                Valor = 50.00M
            };
            pedido.Itens.Add(item);

            // EXECUÇÃO
            var temCredito = pedido.TemLimiteCredito();

            // VALIDAÇÃO
            Assert.IsTrue(temCredito);
        }

        [Test]
        public void Invalida_Pedido_De_Cliente_Sem_Limite_Credito()
        {
            // PREPARAÇÃO
            var cliente = new Cliente { LimiteCredito = 20.00M };
            var pedido = new Pedido { Cliente = cliente };
            var item = new PedidoItem
            {
                Quantidade = 2,
                Valor = 50.00M
            };
            pedido.Itens.Add(item);

            // EXECUÇÃO
            var temCredito = pedido.TemLimiteCredito();

            // VALIDAÇÃO
            Assert.IsFalse(temCredito);
        }

        [Test]
        public void Valida_Item_De_Produto_Com_Estoque()
        {
            // PREPARAÇÃO
            var produto = new Produto { Estoque = 100 };
            var item = new PedidoItem
            {
                Produto = produto,
                Quantidade = 2
            };
            
            // EXECUÇÃO
            var temEstoque = item.TemEstoqueDisponivel();

            // VALIDAÇÃO
            Assert.IsTrue(temEstoque);
        }

        [Test]
        public void Invalida_Item_De_Produto_Sem_Estoque()
        {
            // PREPARAÇÃO
            var produto = new Produto { Estoque = 0 };
            var item = new PedidoItem
            {
                Produto = produto,
                Quantidade = 2
            };

            // EXECUÇÃO
            var temEstoque = item.TemEstoqueDisponivel();

            // VALIDAÇÃO
            Assert.IsFalse(temEstoque);
        }

        [Test]
        public void Valida_Pedido_De_Produtos_Com_Estoque()
        {
            // PREPARAÇÃO
            var produto1 = new Produto { Nome = "Laranja", Estoque = 100 };
            var produto2 = new Produto { Nome = "Banana", Estoque = 50 };
            var item1 = new PedidoItem
            {
                Produto = produto1,
                Quantidade = 2
            };
            var item2 = new PedidoItem
            {
                Produto = produto1,
                Quantidade = 10
            };

            var pedido = new Pedido();
            pedido.Itens.Add(item1);
            pedido.Itens.Add(item2);

            // EXECUÇÃO
            var msgs = new List<string>();
            var temEstoque = pedido.TemEstoqueDisponivel(msgs);
            ImprimeMensagens(msgs);

            // VALIDAÇÃO
            Assert.IsTrue(temEstoque);
        }

        [Test]
        public void Invalida_Pedido_De_Produtos_Sem_Estoque()
        {
            // PREPARAÇÃO
            var produto1 = new Produto { Nome = "Laranja", Estoque = 100 };
            var produto2 = new Produto { Nome = "Banana", Estoque = 5 };
            var item1 = new PedidoItem
            {
                Produto = produto1,
                Quantidade = 2
            };
            var item2 = new PedidoItem
            {
                Produto = produto2,
                Quantidade = 10
            };

            var pedido = new Pedido();
            pedido.Itens.Add(item1);
            pedido.Itens.Add(item2);

            // EXECUÇÃO
            var msgs = new List<string>();
            var temEstoque = pedido.TemEstoqueDisponivel(msgs);
            ImprimeMensagens(msgs);

            // VALIDAÇÃO
            Assert.IsFalse(temEstoque);
        }

        [Test]
        public void Valida_Pedido_Com_Total_Acima_De_100_Reais()
        {
            // PREPARAÇÃO
            var item1 = new PedidoItem
            {
                Quantidade = 2,
                Valor = 10.00M
            };
            var item2 = new PedidoItem
            {
                Quantidade = 10,
                Valor = 10.00M
            };

            var pedido = new Pedido();
            pedido.Itens.Add(item1);
            pedido.Itens.Add(item2);

            // EXECUÇÃO
            var temTotalMinimo = pedido.TemTotalMinimo();

            // VALIDAÇÃO
            Assert.IsTrue(temTotalMinimo);
        }

        [Test]
        public void Invalida_Pedido_Com_Total_Abaixo_De_100_Reais()
        {
            // PREPARAÇÃO
            var item1 = new PedidoItem
            {
                Quantidade = 2,
                Valor = 10.00M
            };
            var pedido = new Pedido();
            pedido.Itens.Add(item1);

            // EXECUÇÃO
            var temTotalMinimo = pedido.TemTotalMinimo();

            // VALIDAÇÃO
            Assert.IsFalse(temTotalMinimo);
        }

        [Test]
        public void Pedido_E_Valido()
        {
            // PREPARAÇÃO
            var cliente = new Cliente { Nome = "Tião Carreiro", LimiteCredito = 1200.00M };
            var pedido = new Pedido { Cliente = cliente };
            var produto1 = new Produto { Nome = "Laranja", Estoque = 100 };
            var produto2 = new Produto { Nome = "Banana", Estoque = 50 };
            var item1 = new PedidoItem
            {
                Produto = produto1,
                Quantidade = 2,
                Valor = 50.00M
            };
            var item2 = new PedidoItem
            {
                Produto = produto1,
                Quantidade = 10,
                Valor = 20.00M
            };
            pedido.Itens.Add(item1);
            pedido.Itens.Add(item2);

            // EXECUÇÃO
            var msgs = new List<string>();
            var valido = pedido.Valida(msgs);
            ImprimeMensagens(msgs);

            // VALIDAÇÃO
            Assert.IsTrue(valido);
        }

        [Test]
        public void Pedido_Nao_E_Valido()
        {
            // PREPARAÇÃO
            var cliente = new Cliente { Nome = "Tião Carreiro", LimiteCredito = 0.00M };
            var pedido = new Pedido { Cliente = cliente };
            var produto1 = new Produto { Nome = "Laranja", Estoque = 1 };
            var produto2 = new Produto { Nome = "Banana", Estoque = 1 };
            var item1 = new PedidoItem
            {
                Produto = produto1,
                Quantidade = 2,
                Valor = 1.00M
            };
            var item2 = new PedidoItem
            {
                Produto = produto2,
                Quantidade = 10,
                Valor = 1.00M
            };
            pedido.Itens.Add(item1);
            pedido.Itens.Add(item2);

            // EXECUÇÃO
            var msgs = new List<string>();
            var valido = pedido.Valida(msgs);
            ImprimeMensagens(msgs);

            // VALIDAÇÃO
            Assert.IsFalse(valido);
        }

        private void ImprimeMensagens(List<string> msgs)
        {
            foreach (var msg in msgs)
                Console.WriteLine(msg);
        }
    }
}
