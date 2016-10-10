using NUnit.Framework;
using System;

namespace Empresa.Pedidos.Testes
{
    [TestFixture]
    public class TesteHelloWorld
    {
        [Test]
        public void TestarHelloWorld()
        {
            Console.WriteLine("Hello World NUnit");
        }

        [Test]
        public void TestarSoma()
        {
            var soma = 2 + 2;
            Assert.AreEqual(4, soma);
        }
    }
}