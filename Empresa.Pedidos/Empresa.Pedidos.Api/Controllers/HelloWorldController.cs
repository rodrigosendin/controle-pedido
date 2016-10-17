using System.Web.Http;

namespace Empresa.Pedidos.Api.Controllers
{
    public class HelloWorldController : ApiController
    {
        public string[] Get()
        {
            return new [] {"hello", "world"};
        }
    }
}
