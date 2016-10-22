using Empresa.Pedidos.Dominio;
using Kite.Base.Dominio.Util;
using Kite.Base.Util;
using System;
using System.Web.Http;

namespace Empresa.Pedidos.Api.Controllers
{
    public class PedidoAbateEstoqueController : ApiController
    {
        public virtual IHttpActionResult Post(int id)
        {
            var servico = Kernel.Get<ServicoPedido>();
            try
            {
                var ok = servico.AbateEstoque(id);
                if (ok == false)
                    return BadRequest(servico.Mensagens.ToMessageBoxString());

                var pedido = servico.Retorna(id);
                                
                return Ok(pedido);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
