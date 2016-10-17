using System;
using System.Linq;
using NUnit.Framework;
using Kite.Base.Repositorio;
using Empresa.Pedidos.Dominio.Entidades;
using Empresa.Pedidos.Dominio.ObjetosValor;
using Kite.Base.Dominio.Servicos;
using Kite.Base.Util;

namespace Empresa.Pedidos.Testes.Repositorio
{
    [TestFixture]
    public class TesteServicoCliente
    {
        #region Atributos
        private Servico<Cliente> _servico;

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
            Kernel.Start();
            _servico = Kernel.Get<Servico<Cliente>>();

            NHibernateHelper.CreateDb();

            // Populando Banco para poder Testar Consultas, Update e Delete
            _localidade1 = new Localidade { Cidade = "Piracicaba", Uf = "SP" };
            _localidade2 = new Localidade { Cidade = "Americana", Uf = "SP" };
            
            _cliente1 = new Cliente { Nome = "Tião Carreiro", Localidade = _localidade1, LimiteCredito = 100.00M };
            _cliente2 = new Cliente { Nome = "Pardinho", Localidade = _localidade1, LimiteCredito = 100.00M };
            _cliente3 = new Cliente { Nome = "Milionário", Localidade = _localidade2, LimiteCredito = 100.00M };
            _cliente4 = new Cliente { Nome = "Zé Rico", Localidade = _localidade2, LimiteCredito = 100.00M };
            _servico.Inclui(_cliente1);
            _servico.Inclui(_cliente2);
            _servico.Inclui(_cliente3);
            _servico.Inclui(_cliente4);
        }

        [Test]
        public void Pode_Retornar()
        {
            var fromDb = _servico.Retorna(1);
            Assert.AreEqual(fromDb, _cliente1);
            Console.WriteLine(_servico);
        }

        [Test]
        public void Pode_Consultar()
        {
            var fromDb = _servico.Consulta();
            Assert.Greater(fromDb.Count(), 1);
            foreach (var cliente in fromDb)
                Console.WriteLine(cliente);
        }

        [Test]
        public void Pode_Consultar_Paginado()
        {
            var pagina = _servico.ConsultaPaginada(0);
            Assert.Greater(pagina.Resultado.Count, 1);
            foreach (var cliente in pagina.Resultado)
                Console.WriteLine(cliente);
        }

        [Test]
        public void Pode_Incluir()
        {
            var cliente = new Cliente { Nome = "Saponga", Localidade = _localidade1, LimiteCredito = 10.00M };
            var ok = _servico.Inclui(cliente);
            Assert.IsTrue(ok);
        }

        [Test]
        public void Pode_Alterar()
        {
            var fromDb = _servico.Retorna(1);
            fromDb.Nome = "Madalena";
            var ok = _servico.Altera(fromDb);
            Assert.IsTrue(ok);
        }

        [Test]
        public void Pode_Excluir()
        {
            var ok = _servico.Exclui(4);
            Assert.IsTrue(ok);
        }
    }
}