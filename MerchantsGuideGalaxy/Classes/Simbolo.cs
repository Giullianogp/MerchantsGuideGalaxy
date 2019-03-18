using MerchantsGuideGalaxy.Enum;
using System;

namespace MerchantsGuideGalaxy.Classes
{
    public class Simbolo
    {
        public string Nome { get; set; }
        public TipoValor Tipo { get; set; }
        public decimal Valor { get; set; }


        public override bool Equals(object simbolo)
        {
            return Nome == ((Simbolo)simbolo).Nome;
        }

        public decimal ToDecimal()
        {            
            if (!decimal.TryParse(Nome, out var result))
                throw new Exception("Valor não valido");
            else
                return result;
        }
    }
}
