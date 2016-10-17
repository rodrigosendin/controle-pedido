using NUnit.Framework;
using Kite.Base.Repositorio;

namespace Empresa.Pedidos.Testes.Repositorio
{
    [TestFixture]
    public class TesteCriaDatabase
    {
        [Test]
        public void CriaDatabase()
        {
            NHibernateHelper.CreateDb();
        }
    }
}