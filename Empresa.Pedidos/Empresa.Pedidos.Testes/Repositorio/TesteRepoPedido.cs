using Empresa.Pedidos.Dominio.Entidades;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using Kite.Base.Dominio.Repositorio;
using Kite.Base.Repositorio;
using Empresa.Pedidos.Dominio.ObjetosValor;
using System.Linq;

namespace Empresa.Pedidos.Testes.Repositorio
{
    [TestFixture]
    public class TesteRepoPedido
    {
        #region Atributos
        private IRepositorioHelper _helper;

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
            NHibernateHelper.CreateDb();
            _helper = new RepositorioHelper();

            // Populando Banco para poder Testar Consultas, Update e Delete
            var localidade1 = new Localidade { Cidade = "Piracicaba", Uf = "SP" };
            var localidade2 = new Localidade { Cidade = "Americana", Uf = "SP" };

            _cliente1 = new Cliente { Nome = "Tião Carreiro",   Localidade = localidade1, LimiteCredito = 1000.00M };
            _cliente2 = new Cliente { Nome = "Pardinho",        Localidade = localidade1, LimiteCredito = 1000.00M };

            _produto1 = new Produto { Nome = "Laranja", Estoque = 100, Valor = 0.50M };
            _produto2 = new Produto { Nome = "Banana",  Estoque = 200, Valor = 1.50M };

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

            using (var sessao = _helper.AbrirSessao())
            {
                var repoProduto = sessao.GetRepositorio<Produto>();
                var repoCliente = sessao.GetRepositorio<Cliente>();
                var repoPedido  = sessao.GetRepositorio<Pedido>();
                try
                {
                    sessao.IniciaTransacao();
                    repoProduto.Inclui(_produto1);
                    repoProduto.Inclui(_produto2);
                    repoCliente.Inclui(_cliente1);
                    repoCliente.Inclui(_cliente2);
                    repoPedido.Inclui(_pedido1);
                    repoPedido.Inclui(_pedido2);
                    repoPedido.Inclui(_pedido3);
                    repoPedido.Inclui(_pedido4);
                    sessao.ComitaTransacao();
                }
                catch (Exception)
                {
                    sessao.RollBackTransacao();
                    throw;
                }
            }
        }

        [Test]
        public void Pode_Retornar()
        {
            using (var sessao = _helper.AbrirSessao())
            {
                var repo = sessao.GetRepositorio<Pedido>();
                try
                {
                    sessao.IniciaTransacao();
                    var fromDb = repo.Retorna(1);
                    Assert.AreEqual(fromDb, _pedido1);
                    Console.WriteLine(fromDb);
                    sessao.ComitaTransacao();
                }
                catch (Exception)
                {
                    sessao.RollBackTransacao();
                    throw;
                }
            }
        }

        [Test]
        public void Pode_Consultar()
        {
            using (var sessao = _helper.AbrirSessao())
            {
                var repo = sessao.GetRepositorio<Pedido>();
                try
                {
                    sessao.IniciaTransacao();
                    var fromDb = repo.Consulta();
                    Assert.Greater(fromDb.Count(), 1);
                    foreach (var pedido in fromDb)
                        Console.WriteLine(pedido);
                    sessao.ComitaTransacao();
                }
                catch (Exception)
                {
                    sessao.RollBackTransacao();
                    throw;
                }
            }
        }

        [Test]
        public void Pode_Consultar_Projecao()
        {
            using (var sessao = _helper.AbrirSessao())
            {
                var repo = sessao.GetRepositorio<Pedido>();
                try
                {
                    sessao.IniciaTransacao();
                    var fromDb = from x in repo.Consulta()
                                 where x.Id > 1
                                 select new
                                 {
                                     x.Cliente.Nome,
                                     x.Id,
                                     Teste = x.Id + " - " + x.Cliente.Nome
                                 };

                    Assert.Greater(fromDb.Count(), 1);

                    Console.WriteLine(fromDb.Count());
                    foreach (var pedido in fromDb)
                    {
                        Console.WriteLine(pedido.Id + " " +
                            pedido.Nome + " " + pedido.Teste);
                    }
                    sessao.ComitaTransacao();
                }
                catch (Exception)
                {
                    sessao.RollBackTransacao();
                    throw;
                }
            }
        }

        [Test]
        public void Pode_Incluir()
        {
            var pedido = new Pedido { DataPedido = DateTime.Now.AddDays(4), Cliente = _cliente2 };
            pedido.Itens.Add(new PedidoItem { Produto = _produto1, Quantidade = 100, Valor = _produto1.Valor });
            pedido.Itens.Add(new PedidoItem { Produto = _produto2, Quantidade = 100, Valor = _produto2.Valor });

            using (var sessao = _helper.AbrirSessao())
            {
                var repo = sessao.GetRepositorio<Pedido>();
                try
                {
                    sessao.IniciaTransacao();
                    repo.Inclui(pedido);
                    sessao.ComitaTransacao();
                }
                catch (Exception)
                {
                    sessao.RollBackTransacao();
                    throw;
                }
            }

            // Assert
            using (var sessao = _helper.AbrirSessao())
            {
                var repo = sessao.GetRepositorio<Pedido>();
                try
                {
                    sessao.IniciaTransacao();
                    var fromDb = repo.Retorna(pedido.Id);
                    Assert.AreEqual(fromDb, pedido);
                    sessao.ComitaTransacao();
                }
                catch (Exception)
                {
                    sessao.RollBackTransacao();
                    throw;
                }
            }
        }

        [Test]
        public void Pode_Alterar()
        {
            using (var sessao = _helper.AbrirSessao())
            {
                var repo = sessao.GetRepositorio<Pedido>();
                try
                {
                    sessao.IniciaTransacao();
                    var fromDb = repo.Retorna(1);
                    fromDb.DataPedido = DateTime.Today.AddDays(5);
                    repo.Altera(fromDb);
                    sessao.ComitaTransacao();

                }
                catch (Exception)
                {
                    sessao.RollBackTransacao();
                    throw;
                }
            }

            using (var sessao = _helper.AbrirSessao())
            {
                var repo = sessao.GetRepositorio<Pedido>();
                try
                {
                    sessao.IniciaTransacao();
                    var fromDb = repo.Retorna(1);
                    Assert.AreEqual(DateTime.Today.AddDays(5), fromDb.DataPedido);
                    Console.WriteLine(fromDb.DataPedido);
                    sessao.ComitaTransacao();

                }
                catch (Exception)
                {
                    sessao.RollBackTransacao();
                    throw;
                }
            }
        }

        [Test]
        public void Pode_Excluir()
        {
            using (var sessao = _helper.AbrirSessao())
            {
                var repo = sessao.GetRepositorio<Pedido>();
                try
                {
                    sessao.IniciaTransacao();
                    var fromDb = repo.Retorna(4);
                    repo.Exclui(fromDb);
                    sessao.ComitaTransacao();
                }
                catch (Exception)
                {
                    sessao.RollBackTransacao();
                    throw;
                }
            }

            using (var sessao = _helper.AbrirSessao())
            {
                var repo = sessao.GetRepositorio<Pedido>();
                try
                {
                    sessao.IniciaTransacao();
                    var fromDb = repo.Retorna(4);
                    Assert.IsNull(fromDb);
                    sessao.ComitaTransacao();
                }
                catch (Exception)
                {
                    sessao.RollBackTransacao();
                    throw;
                }
            }
        }
    }
}