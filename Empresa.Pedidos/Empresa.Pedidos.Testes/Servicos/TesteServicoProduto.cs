using System;
using System.Linq;
using NUnit.Framework;
using Kite.Base.Repositorio;
using Kite.Base.Util;
using Empresa.Pedidos.Dominio.Entidades;
using Kite.Base.Dominio.Servicos;

namespace Empresa.Pedidos.Testes.Repositorio
{
    [TestFixture]
    public class TesteServicoProduto
    {
        #region Atributos
        private Servico<Produto> _servico;

        private Produto _produto1;
        private Produto _produto2;
        private Produto _produto3;
        private Produto _produto4;
        #endregion

        [TestFixtureSetUp]
        public void Setup()
        {
            Kernel.Start();
            _servico = Kernel.Get<Servico<Produto>>();

            NHibernateHelper.CreateDb();

            // Populando Banco para poder Testar Consultas, Update e Delete
            _produto1 = new Produto { Nome = "Laranja", Estoque = 100, Valor = 0.50M };
            _produto2 = new Produto { Nome = "Banana",  Estoque = 200, Valor = 1.50M };
            _produto3 = new Produto { Nome = "Pêra",    Estoque = 300, Valor = 2.50M };
            _produto4 = new Produto { Nome = "Abacaxi", Estoque = 400, Valor = 3.50M };

            _servico.Inclui(_produto1);
            _servico.Inclui(_produto2);
            _servico.Inclui(_produto3);
            _servico.Inclui(_produto4);
        }

        [Test]
        public void Pode_Retornar()
        {
            var fromDb = _servico.Retorna(1);
            Assert.AreEqual(fromDb, _produto1);
            Console.WriteLine(fromDb);
        }

        [Test]
        public void Pode_Consultar()
        {
            var fromDb = _servico.Consulta();
            Assert.Greater(fromDb.Count(), 1);
            foreach (var produto in fromDb)
                Console.WriteLine(produto);
        }

        [Test]
        public void Pode_Consultar_Paginado()
        {
            var pagina = _servico.ConsultaPaginada(0);
            Assert.Greater(pagina.Resultado.Count, 1);
            foreach (var produto in pagina.Resultado)
                Console.WriteLine(produto);
        }

        [Test]
        public void Pode_Incluir()
        {
            var produto = new Produto { Nome = "Jaboticaba" };
            var ok = _servico.Inclui(produto);
            Assert.IsTrue(ok);
        }

        [Test]
        public void Pode_Alterar()
        {
            var fromDb = _servico.Retorna(1);
            fromDb.Nome = "Limão";
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