using Empresa.Pedidos.Dominio.Entidades;
using Kite.Base.Dominio.Servicos;
using Kite.Base.Util;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

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
    }
}
