using NUnit.Framework;
using System;

namespace Empresa.Pedidos.Testes
{
    [TestFixture]
    public class TesteHelloWorld
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            // Preparar o Teste Aqui!
        }

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

        [Test]
        public void TestarSoma2()
        {
            var soma = 2 + 2;
            Assert.AreEqual(5, soma);
        }
    }
}