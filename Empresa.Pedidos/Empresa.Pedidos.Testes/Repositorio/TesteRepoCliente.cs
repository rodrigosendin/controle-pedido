using System;
using System.Linq;
using NUnit.Framework;
using Kite.Base.Dominio.Repositorio;
using Kite.Base.Repositorio;
using Empresa.Pedidos.Dominio.Entidades;
using Empresa.Pedidos.Dominio.ObjetosValor;

namespace Empresa.Pedidos.Testes.Repositorio
{
    [TestFixture]
    public class TesteRepoCliente
    {
        #region Atributos
        private IRepositorioHelper _helper;

        private Localidade _localidade1;
        private Localidade _localidade2;

        private Cliente _cliente1;
        private Cliente _cliente2;
        private Cliente _cliente3;
        private Cliente _cliente4;
        #endregion

        [TestFixtureSetUp]
        public void Setup()
        {
            NHibernateHelper.CreateDb();
            _helper = new RepositorioHelper();

            // Populando Banco para poder Testar Consultas, Update e Delete
            _localidade1 = new Localidade { Cidade = "Piracicaba", Uf = "SP" };
            _localidade2 = new Localidade { Cidade = "Americana", Uf = "SP" };
            
            _cliente1 = new Cliente { Nome = "Tião Carreiro", Localidade = _localidade1, LimiteCredito = 100.00M };
            _cliente2 = new Cliente { Nome = "Pardinho", Localidade = _localidade1, LimiteCredito = 100.00M };
            _cliente3 = new Cliente { Nome = "Milionário", Localidade = _localidade2, LimiteCredito = 100.00M };
            _cliente4 = new Cliente { Nome = "Zé Rico", Localidade = _localidade2, LimiteCredito = 100.00M };
            using (var sessao = _helper.AbrirSessao())
            {
                var repo = sessao.GetRepositorio<Cliente>();
                try
                {
                    sessao.IniciaTransacao();
                    repo.Inclui(_cliente1);
                    repo.Inclui(_cliente2);
                    repo.Inclui(_cliente3);
                    repo.Inclui(_cliente4);
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
                var repo = sessao.GetRepositorio<Cliente>();
                try
                {
                    sessao.IniciaTransacao();
                    var fromDb = repo.Retorna(1);
                    Assert.AreEqual(fromDb, _cliente1);
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
                var repo = sessao.GetRepositorio<Cliente>();
                try
                {
                    sessao.IniciaTransacao();
                    var fromDb = repo.Consulta();
                    Assert.Greater(fromDb.Count(), 1);
                    foreach (var cliente in fromDb)
                        Console.WriteLine(cliente);
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
                var repo = sessao.GetRepositorio<Cliente>();
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
                    foreach (var Cliente in fromDb)
                    {
                        Console.WriteLine(Cliente.Id + "   " +
                            Cliente.Nome + "  " + Cliente.Teste);
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
            var cliente = new Cliente { Nome = "Saponga", Localidade = _localidade1, LimiteCredito = 10.00M };
            using (var sessao = _helper.AbrirSessao())
            {
                var repo = sessao.GetRepositorio<Cliente>();
                try
                {
                    sessao.IniciaTransacao();
                    repo.Inclui(cliente);
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
                var repo = sessao.GetRepositorio<Cliente>();
                try
                {
                    sessao.IniciaTransacao();
                    var fromDb = repo.Retorna(cliente.Id);
                    Assert.AreEqual(fromDb, cliente);
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
                var repo = sessao.GetRepositorio<Cliente>();
                try
                {
                    sessao.IniciaTransacao();
                    var fromDb = repo.Retorna(1);
                    fromDb.Nome = "Madalena";
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
                var repo = sessao.GetRepositorio<Cliente>();
                try
                {
                    sessao.IniciaTransacao();
                    var fromDb = repo.Retorna(1);
                    Assert.AreEqual("Madalena", fromDb.Nome);
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
                var repo = sessao.GetRepositorio<Cliente>();
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
                var repo = sessao.GetRepositorio<Cliente>();
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