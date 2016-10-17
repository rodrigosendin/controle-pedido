using System;
using System.Linq;
using NUnit.Framework;
using Kite.Base.Dominio.Repositorio;
using Kite.Base.Repositorio;
using Kite.Base.Util;
using Empresa.Pedidos.Dominio.Entidades;

namespace Empresa.Pedidos.Testes.Repositorio
{
    [TestFixture]
    public class TesteRepoProduto
    {
        #region Atributos
        private IRepositorioHelper _helper;
        private Produto _produto1;
        private Produto _produto2;
        private Produto _produto3;
        private Produto _produto4;
        #endregion

        [TestFixtureSetUp]
        public void Setup()
        {
            Kernel.Start();
            _helper = Kernel.Get<IRepositorioHelper>();
            //_helper = new RepositorioHelper();

            NHibernateHelper.CreateDb();

            // Populando Banco para poder Testar Consultas, Update e Delete
            _produto1 = new Produto { Nome = "Laranja", Estoque = 100, Valor = 0.50M };
            _produto2 = new Produto { Nome = "Banana",  Estoque = 200, Valor = 1.50M };
            _produto3 = new Produto { Nome = "Pêra",    Estoque = 300, Valor = 2.50M };
            _produto4 = new Produto { Nome = "Abacaxi", Estoque = 400, Valor = 3.50M };
            using (var sessao = _helper.AbrirSessao())
            {
                var repo = sessao.GetRepositorio<Produto>();
                try
                {
                    sessao.IniciaTransacao();
                    repo.Inclui(_produto1);
                    repo.Inclui(_produto2);
                    repo.Inclui(_produto3);
                    repo.Inclui(_produto4);
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
                var repo = sessao.GetRepositorio<Produto>();
                try
                {
                    sessao.IniciaTransacao();
                    var fromDb = repo.Retorna(1);
                    Assert.AreEqual(fromDb, _produto1);
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
                var repo = sessao.GetRepositorio<Produto>();
                try
                {
                    sessao.IniciaTransacao();
                    var fromDb = repo.Consulta();
                    Assert.Greater(fromDb.Count(), 1);
                    foreach (var produto in fromDb)
                        Console.WriteLine(produto);
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
                var repo = sessao.GetRepositorio<Produto>();
                try
                {
                    sessao.IniciaTransacao();
                    var fromDb = from x in repo.Consulta()
                                 where x.Id > 1
                                 select new
                                 {
                                     x.Nome,
                                     x.Id,
                                     Teste = x.Id + " - " + x.Nome
                                 };

                    Assert.Greater(fromDb.Count(), 1);

                    Console.WriteLine(fromDb.Count());
                    foreach (var produto in fromDb)
                    {
                        Console.WriteLine(produto.Id + "   " +
                            produto.Nome + "  " + produto.Teste);
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
            var produto = new Produto { Nome = "Jaboticaba" };
            using (var sessao = _helper.AbrirSessao())
            {
                var repo = sessao.GetRepositorio<Produto>();
                try
                {
                    sessao.IniciaTransacao();
                    repo.Inclui(produto);
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
                var repo = sessao.GetRepositorio<Produto>();
                try
                {
                    sessao.IniciaTransacao();
                    var fromDb = repo.Retorna(produto.Id);
                    Assert.AreEqual(fromDb, produto);
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
                var repo = sessao.GetRepositorio<Produto>();
                try
                {
                    sessao.IniciaTransacao();
                    var fromDb = repo.Retorna(1);
                    fromDb.Nome = "Limão";
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
                var repo = sessao.GetRepositorio<Produto>();
                try
                {
                    sessao.IniciaTransacao();
                    var fromDb = repo.Retorna(1);
                    Assert.AreEqual("Limão", fromDb.Nome);
                    Console.WriteLine(fromDb.Nome);
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
                var repo = sessao.GetRepositorio<Produto>();
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
                var repo = sessao.GetRepositorio<Produto>();
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