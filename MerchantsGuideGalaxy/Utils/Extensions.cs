using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MerchantsGuideGalaxy.Utils
{
    public static class Extensions
    {
        public static int RomanToInt(this string simbolo)
        {
            int valorTotal = 0;
            int valorAnterior = 0;

            foreach (var c in simbolo)
            {
                if (!RomanDictionary.ContainsKey(c))
                {
                    return 0;
                }

                var valor = RomanDictionary[c];

                valorTotal += valor;

                if (valorAnterior != 0 && valorAnterior < valor)
                {
                    if (valorAnterior == 1 && (valor == 5 || valor == 10) || valorAnterior == 10 && (valor == 50 || valor == 100) || valorAnterior == 100 && (valor == 500 || valor == 1000))
                    {
                        valorTotal -= 2 * valorAnterior;
                    }
                    else
                    {
                        return 0;
                    }
                }

                valorAnterior = valor;
            }
            return valorTotal;
        }

        private static Dictionary<char, int> RomanDictionary
            => new Dictionary<char, int> { { 'I', 1 }, { 'V', 5 }, { 'X', 10 }, { 'L', 50 }, { 'C', 100 }, { 'D', 500 }, { 'M', 1000 } };

        public static bool IsRomanValid(this string simbolo)
        {
            return Regex.Match(simbolo, @"^M{0,3}(CM|CD|D?C{0,3})(XC|XL|L?X{0,3})(IX|IV|V?I{0,3})$").Success;
        }
    }
}
