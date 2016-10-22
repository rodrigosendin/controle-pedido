using System;
using System.Linq;
using NUnit.Framework;
using Kite.Base.Repositorio;
using Kite.Base.Util;
using Empresa.Pedidos.Dominio.Entidades;
using Kite.Base.Dominio.Servicos;
using Empresa.Pedidos.Dominio.ObjetosValor;
using Empresa.Pedidos.Dominio;
using System.Collections.Generic;
using Kite.Base.Dominio.Entidades;

namespace Empresa.Pedidos.Testes.Repositorio
{
    [TestFixture]
    public class TesteServicoPedido
    {
        #region Atributos
        private ServicoPedido _servico;

        private Produto _produto1;
        private Produto _produto2;

        private Cliente _cliente1;
        private Cliente _cliente2;

        private Pedido _pedido1;
        private Pedido _pedido2;
        private Pedido _pedido3;
        private Pedido _pedido4;
        #endregion

        [TestFixtureSetUp]
        public void Setup()
        {            
            Kernel.Start();
            _servico = Kernel.Get<ServicoPedido>();

            NHibernateHelper.CreateDb();

            // Cria um novo usuário
            var servicoUsuario = Kernel.Get<ServicoUsuario>();
            var usuario = new Usuario
            {
                Nome = "Administrador",
                Login = "admin",
                Senha = "123"
            };
            servicoUsuario.Inclui(usuario);

            // Populando Banco para poder Testar Consultas, Update e Delete
            var localidade1 = new Localidade { Cidade = "Piracicaba", Uf = "SP" };
            var localidade2 = new Localidade { Cidade = "Americana", Uf = "SP" };

            _cliente1 = new Cliente { Nome = "Tião Carreiro", Localidade = localidade1, LimiteCredito = 1000.00M };
            _cliente2 = new Cliente { Nome = "Pardinho", Localidade = localidade1, LimiteCredito = 1000.00M };

            _produto1 = new Produto { Nome = "Laranja", Estoque = 10000, Valor = 0.50M };
            _produto2 = new Produto { Nome = "Banana",  Estoque = 20000, Valor = 1.50M };

            _pedido1 = new Pedido { DataPedido = DateTime.Now, Cliente = _cliente1 };
            _pedido1.Itens.Add(new PedidoItem { Produto = _produto1, Quantidade = 100, Valor = _produto1.Valor });
            _pedido1.Itens.Add(new PedidoItem { Produto = _produto2, Quantidade = 100, Valor = _produto2.Valor });

            _pedido2 = new Pedido { DataPedido = DateTime.Now.AddDays(1), Cliente = _cliente2 };
            _pedido2.Itens.Add(new PedidoItem { Produto = _produto1, Quantidade = 100, Valor = _produto1.Valor });
            _pedido2.Itens.Add(new PedidoItem { Produto = _produto2, Quantidade = 100, Valor = _produto2.Valor });

            _pedido3 = new Pedido { DataPedido = DateTime.Now.AddDays(2), Cliente = _cliente1 };
            _pedido3.Itens.Add(new PedidoItem { Produto = _produto1, Quantidade = 100, Valor = _produto1.Valor });
            _pedido3.Itens.Add(new PedidoItem { Produto = _produto2, Quantidade = 100, Valor = _produto2.Valor });

            _pedido4 = new Pedido { DataPedido = DateTime.Now.AddDays(3), Cliente = _cliente2 };
            _pedido4.Itens.Add(new PedidoItem { Produto = _produto1, Quantidade = 100, Valor = _produto1.Valor });
            _pedido4.Itens.Add(new PedidoItem { Produto = _produto2, Quantidade = 100, Valor = _produto2.Valor });

            var servicoProduto = Kernel.Get<Servico<Produto>>();
            var servicoCliente = Kernel.Get<Servico<Cliente>>();

            servicoProduto.Inclui(_produto1);
            servicoProduto.Inclui(_produto2);
            servicoCliente.Inclui(_cliente1);
            servicoCliente.Inclui(_cliente2);
            _servico.Inclui(_pedido1);
            _servico.Inclui(_pedido2);
            _servico.Inclui(_pedido3);
            _servico.Inclui(_pedido4);
        }

        [Test]
        public void Pode_Retornar()
        {
            var fromDb = _servico.Retorna(1);
            Assert.AreEqual(fromDb, _pedido1);
            Console.WriteLine(fromDb);

            ImprimeMensagens(_servico.Mensagens);
        }

        [Test]
        public void Pode_Consultar()
        {
            var fromDb = _servico.Consulta();
            Assert.Greater(fromDb.Count(), 1);
            foreach (var pedido in fromDb)
                Console.WriteLine(pedido);

            ImprimeMensagens(_servico.Mensagens);
        }

        [Test]
        public void Pode_Consultar_Paginado()
        {
            var pagina = _servico.ConsultaPaginada(0);
            Assert.Greater(pagina.Resultado.Count, 1);
            foreach (var pedido in pagina.Resultado)
                Console.WriteLine(pedido);

            ImprimeMensagens(_servico.Mensagens);
        }

        [Test]
        public void Pode_Incluir()
        {
            var pedido = new Pedido { DataPedido = DateTime.Now.AddDays(4), Cliente = _cliente2 };
            pedido.Itens.Add(new PedidoItem { Produto = _produto1, Quantidade = 100, Valor = _produto1.Valor });
            pedido.Itens.Add(new PedidoItem { Produto = _produto2, Quantidade = 100, Valor = _produto2.Valor });

            var ok = _servico.Inclui(pedido);
            Assert.IsTrue(ok);

            ImprimeMensagens(_servico.Mensagens);
        }

        [Test]
        public void Pode_Alterar()
        {
            var fromDb = _servico.Retorna(1);
            fromDb.DataPedido = DateTime.Today.AddDays(5);
            var ok = _servico.Altera(fromDb);
            Assert.IsTrue(ok);

            ImprimeMensagens(_servico.Mensagens);
        }

        [Test]
        public void Pode_Excluir()
        {
            var ok = _servico.Exclui(4);
            Assert.IsTrue(ok);

            ImprimeMensagens(_servico.Mensagens);
        }

        [Test]
        public void Nao_Pode_Incluir_Pedido_Invalido()
        {
            _cliente1.LimiteCredito = 0.00M;
            _produto1.Estoque = 0;

            var pedido = new Pedido { DataPedido = DateTime.Now, Cliente = _cliente1 };
            pedido.Itens.Add(new PedidoItem { Produto = _produto1, Quantidade = 10, Valor = _produto1.Valor });
            var ok = _servico.Inclui(pedido);

            Assert.IsFalse(ok);
            ImprimeMensagens(_servico.Mensagens);

            _cliente1.LimiteCredito = 1000.00M;
            _produto1.Estoque = 10000;
        }

        [Test]
        public void Pode_Abater_Estoque_Dos_Produtos_Do_Pedido()
        {
            _servico.AbateEstoque(_pedido1.Id);
            var pedidoAtualizado = _servico.Retorna(_pedido1.Id);

            Assert.AreEqual(9900, pedidoAtualizado.Itens[0].Produto.Estoque);
            Assert.AreEqual(19900, pedidoAtualizado.Itens[1].Produto.Estoque);

            ImprimeMensagens(_servico.Mensagens);
        }
        
        private void ImprimeMensagens(List<string> msgs)
        {
            foreach (var msg in msgs)
                Console.WriteLine(msg);
        }
    }
}