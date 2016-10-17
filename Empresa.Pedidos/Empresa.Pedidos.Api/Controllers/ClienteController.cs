using Empresa.Pedidos.Dominio.Entidades;
using Kite.Base.Dominio.Servicos;
using Kite.Base.Util;
using System.Collections.Generic;
using System.Web.Http;

namespace Empresa.Pedidos.Api.Controllers
{
    public class ClienteController : ApiController
    {
        private Servico<Cliente> _servico;

        public ClienteController()
        {
            _servico = Kernel.Get<Servico<Cliente>>();
        }

        public IHttpActionResult Get(long id)
        {
            var cliente = _servico.Retorna(id);

            if (cliente == null)
                return NotFound();

            return Ok(cliente);
        }

        public IList<Cliente> Get()
        {
            return _servico.Consulta();
        }

        [Route("api/cliente/{id}/pedidos")]
        public IList<Pedido> GetPedidos(long id)
        {
            var servico = Kernel.Get<Servico<Pedido>>();
            return servico.Consulta(x => x.Cliente.Id == id);
        }
    }
}