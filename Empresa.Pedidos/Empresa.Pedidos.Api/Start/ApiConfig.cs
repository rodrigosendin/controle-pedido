using System.Web.Http;
using Owin;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Empresa.Pedidos.Api.Start
{
    public class ApiConfig
    {
        public static void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            // Configuração das Rotas por Atributos
            config.MapHttpAttributeRoutes();

            // Configuração de Rotas Padrão do WebApi 
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Removendo o formatter de xml para melhorar o uso pelo browser 
            // -- comentar/remover essa linha se quiser suportar XML
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            //
            // Configurações  para Formatação JSON
            //
            var json = config.Formatters.JsonFormatter;

            // Configurando CamelCase
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Ignorando Auto-Referencias
            json.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            // Configurando DateTime zone para Local
            json.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;

            // Habilitanto CORS
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            // Configurando o WebApi
            app.UseWebApi(config);
        }
    }
}