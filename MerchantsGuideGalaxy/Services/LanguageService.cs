using MerchantsGuideGalaxy.Utils;

namespace MerchantsGuideGalaxy.Services
{
    public static class LanguageService
    {
        private const string ERROR = "I have no idea what you are talking about";

        public static string Processar(string valor)
        {
            if (!valor.IsRomanValid())
            {
                return ERROR;
            }

            return valor.RomanToInt().ToString();
        }

    }
}
