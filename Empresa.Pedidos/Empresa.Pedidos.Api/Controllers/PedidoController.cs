using Empresa.Pedidos.Dominio;
using Empresa.Pedidos.Dominio.Entidades;
using Kite.Base.Api.Controllers;
using Kite.Base.Util;

namespace Empresa.Pedidos.Api.Controllers
{
    public class PedidoController : ControllerBase<Pedido>
    {
        public PedidoController()
        {
            Servico = Kernel.Get<ServicoPedido>();
        }
    }
}