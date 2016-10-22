using Empresa.Pedidos.Dominio.Entidades;
using Kite.Base.Dominio.Servicos;
using Kite.Base.Dominio.Util;
using Kite.Base.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Empresa.Pedidos.Api.Controllers
{
    public class ProdutoController : ApiController
    {
        private Servico<Produto> _servico;

        public ProdutoController()
        {
            _servico = Kernel.Get<Servico<Produto>>();
        }

        public Produto Get(long id)
        {
            var produto = _servico.Retorna(id);
            return produto;
        }

        public IList<Produto> Get()
        {
            var produtos = _servico.Consulta();
            return produtos;
        }

        public IList<Produto> Get(bool ordenarPorNome)
        {
            var produtos = ordenarPorNome
                ? _servico.Consulta().OrderBy(x => x.Nome).ToList()
                : _servico.Consulta();
            return produtos;
        }

        [Route("api/produtosPorNome/{nome}")]
        public IList<Produto> GetProdutosPorNome(string nome)
        {
            var produtos = _servico.Consulta(x => x.Nome.Contains(nome));
            return produtos;
        }

        public virtual IHttpActionResult Post([FromBody]Produto produto)
        {
            try
            {
                var ok = _servico.Inclui(produto);
                if (ok == false)
                    return BadRequest(_servico.Mensagens.ToMessageBoxString());

                var helper = new UrlHelper(Request);
                var location = helper.Link("DefaultApi", new { id = produto.Id });

                return Created(location, produto);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public virtual IHttpActionResult Put([FromBody]Produto produto)
        {
            try
            {
                var existe = _servico.Retorna(produto.Id) != null;
                if (existe == false)
                    return NotFound();

                var ok = _servico.Altera(produto);
                if (ok == false)
                    return BadRequest(_servico.Mensagens.ToMessageBoxString());

                return Ok(produto);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
