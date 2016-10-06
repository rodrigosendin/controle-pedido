using Empresa.Pedidos.Dominio.ObjetosValor;
using NUnit.Framework;
using System;

namespace Empresa.Pedidos.Testes
{
    [TestFixture]
    public class TesteLocalidade
    {
        [Test]
        public void PodeCalcularDistancia()
        {
            var piracicaba = new Localidade { Latitude = -22.7252800, Longitude = -47.6491700 };
            var marilia    = new Localidade { Latitude = -22.2138900, Longitude = -49.9458300 };

            var distancia = piracicaba.CalcularDistancia(marilia);
            Console.WriteLine(distancia);

            Assert.AreEqual(243, distancia);
        }
    }
}

