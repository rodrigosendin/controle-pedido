using System;
using System.Web.Http;
using System.Linq;
using Kite.Base.Util;
using Kite.Base.Api.Controllers;
using Kite.Base.Api.Filters;
using Kite.Base.Api.Token;
using Kite.Base.Dominio.Entidades;
using Kite.Base.Dominio.Servicos;

namespace Empresa.Pedidos.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>	
	[Autorizacao]
    public class UsuarioController : ControllerBase<Usuario>
    {
	    /// <summary>
	    /// 
	    /// </summary>
	    public UsuarioController()
	    {
            Servico = Kernel.Get<ServicoUsuario>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trocaSenha"></param>
        /// <returns></returns>
        [Route("api/trocasenha")]
        public IHttpActionResult PostTrocaSenha([FromBody]TrocaSenhaRequest trocaSenha)
        {
            try
            {
                var token = ActionContext.RecuperarToken();
                var servico = Servico as ServicoUsuario;
                if(servico.TrocaSenha(token.Login, trocaSenha.SenhaAntiga, trocaSenha.SenhaNova))
                    return Ok();
                    
                return BadRequest(servico.Mensagens.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }            
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TrocaSenhaRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public string SenhaAntiga { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SenhaNova   { get; set; }
    }
}