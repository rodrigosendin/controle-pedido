using System;
using System.Web.Http;
using Kite.Base.Dominio.ViewModels;
using Kite.Base.Api.Token;
using Kite.Base.Util;
using Kite.Base.Dominio.Servicos;

namespace Empresa.Pedidos.Api.Controllers
{
    public class TokenController : ApiController
    {
    	private readonly ServicoUsuario _servico;

    	public TokenController()
    	{
    		_servico = Kernel.Get<ServicoUsuario>();
    	}

		public IHttpActionResult Post([FromBody]LoginRequest login)
        {
            try
            {
                var usuario = _servico.Login(login.Login, login.Senha);
                if (usuario != null)
                    return Ok(usuario.GerarTokenString());

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }            
        }
    }
}