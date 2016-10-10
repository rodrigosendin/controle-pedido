using System.ComponentModel;

namespace Empresa.Pedidos.Dominio.ObjetosValor
{
    public enum PedidoStatus
    {
        Aberto = 1,

        [Description("Análise de Crédito")]
        Analise = 2,

        Faturamento = 3,

        Pago = 4,

        Cancelado = 5
    }
}