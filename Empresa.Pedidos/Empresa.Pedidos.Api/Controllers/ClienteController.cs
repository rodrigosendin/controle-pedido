using Empresa.Pedidos.Dominio.Entidades;
using Kite.Base.Api.Filters;
using Kite.Base.Dominio.Servicos;
using Kite.Base.Dominio.Util;
using Kite.Base.Util;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Empresa.Pedidos.Api.Controllers
{
    [Autorizacao]
    public class ClienteController : ApiController
    {
        private Servico<Cliente> _servico;

        public ClienteController()
        {
            _servico = Kernel.Get<Servico<Cliente>>();
        }

        public IHttpActionResult Get(long id)
        {
            try
            {
                var cliente = _servico.Retorna(id);
                if (cliente == null)
                    return NotFound();

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public IHttpActionResult Get()
        {
            return Ok(_servico.Consulta());
        }


        [Route("api/cliente/{id}/pedidos")]
        public IList<Pedido> GetPedidos(long id)
        {
            var servico = Kernel.Get<Servico<Pedido>>();
            return servico.Consulta(x => x.Cliente.Id == id);
        }

        public virtual IHttpActionResult GetPage(int page)
        {
            var result = _servico.ConsultaPaginada(page);

            if (result == null)
                return BadRequest(_servico.Mensagens.ToMessageBoxString());

            var helper = new UrlHelper(Request);

            if (page > 0)
                result.Anterior = helper.Link("DefaultApi", new { page = page - 1 });
            else
                result.Anterior = string.Empty;

            if (page+1 < result.TotalPaginas)
                result.Proxima = helper.Link("DefaultApi", new { page = page + 1 });
            else
                result.Proxima = string.Empty;

            return Ok(result);
        }

        public virtual IHttpActionResult Post([FromBody]Cliente cliente)
        {
            try
            {
                var ok = _servico.Inclui(cliente);
                if (ok == false)
                    return BadRequest(_servico.Mensagens.ToMessageBoxString());

                var helper = new UrlHelper(Request);
                var location = helper.Link("DefaultApi", new { id = cliente.Id });

                return Created(location, cliente);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public virtual IHttpActionResult Put([FromBody]Cliente cliente)
        {
            try
            {
                var existe = _servico.Retorna(cliente.Id) != null;
                if (existe == false)
                    return NotFound();

                var ok = _servico.Altera(cliente);
                if (ok == false)
                    return BadRequest(_servico.Mensagens.ToMessageBoxString());

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public virtual IHttpActionResult Delete(long id)
        {
            try
            {
                var entidade = _servico.Retorna(id);
                if (entidade == null)
                    return NotFound();

                var ok = _servico.Exclui(id);
                if (ok == false)
                    return BadRequest(_servico.Mensagens.ToMessageBoxString());

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}