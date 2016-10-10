using Kite.Base.Dominio.ObjetosValor;
using System;

namespace Empresa.Pedidos.Dominio.ObjetosValor
{
    public class Localidade : ObjetoValor<Localidade> 
    {
        public string Cidade    { get; set; }
        public string Uf        { get; set; }
        public string Endereco  { get; set; }
        public string Bairro    { get; set; }
        public double Longitude { get; set; }
        public double Latitude  { get; set; }

        public double CalcularDistancia(Localidade localidade)
        {
            var baseRad = Math.PI * Latitude / 180;
            var targetRad = Math.PI * localidade.Latitude / 180;
            var theta = Longitude - localidade.Longitude;
            var thetaRad = Math.PI * theta / 180;

            double dist =
                Math.Sin(baseRad) * Math.Sin(targetRad) + Math.Cos(baseRad) *
                Math.Cos(targetRad) * Math.Cos(thetaRad);
            dist = Math.Acos(dist);

            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            return Math.Round(dist * 1.609344); // KM
        }
    }
}