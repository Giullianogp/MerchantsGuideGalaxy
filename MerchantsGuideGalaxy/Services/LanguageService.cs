using MerchantsGuideGalaxy.Classes;
using MerchantsGuideGalaxy.Enum;
using System.Collections.Generic;
using System.Linq;

namespace MerchantsGuideGalaxy.Services
{
    public class LanguageService
    {
        public string Processar(string valor)
        {
            var simbolos = InterpretadorService.IdentificarComando(valor);

            
        }

        
    }
}
