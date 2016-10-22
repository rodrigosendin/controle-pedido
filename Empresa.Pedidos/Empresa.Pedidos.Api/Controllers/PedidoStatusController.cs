using System.Web.Http;
using Kite.Base.Dominio.Util;
using Kite.Base.Api.Filters;
using Empresa.Pedidos.Dominio.ObjetosValor;

namespace Empresa.Pedidos.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>	
	[Autorizacao]
    public class PedidoStatusController : ApiController
    {
    	/// <summary>
	    /// 
	    /// </summary>    	
        public IHttpActionResult Get()
        {
            var ovs = EnumTools.RetornaLista<PedidoStatus>();
            return Ok(ovs);
        }
    }
}