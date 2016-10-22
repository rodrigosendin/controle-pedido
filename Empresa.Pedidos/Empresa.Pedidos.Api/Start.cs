using Owin;
using Empresa.Pedidos.Api.Start;
using Kite.Base.Util;
using Kite.Base.Api.Token;

namespace Empresa.Pedidos.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Kernel.Start();
            ApiConfig.Configuration(app);
            ApiToken.GerarSecret();
        }
    }
}