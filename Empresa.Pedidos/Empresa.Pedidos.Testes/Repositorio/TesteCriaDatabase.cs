using NUnit.Framework;
using Kite.Base.Repositorio;
using Kite.Base.Util;
using System;
using Kite.Base.Dominio.Servicos;
using Kite.Base.Dominio.Entidades;

namespace Empresa.Pedidos.Testes.Repositorio
{
    [TestFixture]
    public class TesteCriaDatabase
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            Kernel.Start();
        }

        [Test]
        public void PodeCriarDatabase()
        {
            try
            {
                NHibernateHelper.CreateDb();

                var servicoUsuario = Kernel.Get<ServicoUsuario>();
                var usuario = new Usuario
                {
                    Nome = "Administrador",
                    Login = "admin",
                    Senha = "123"
                };

                servicoUsuario.Inclui(usuario);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }
    }
}