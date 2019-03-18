using MerchantsGuideGalaxy.Services;
using System;

namespace MerchantsGuideGalaxy
{
    class Program
    {
        static void Main(string[] args)
        {
            

            while (true)
            {
                Console.WriteLine("Olá, Digite os valores. Digite \"sair\" para fechar.");
                var valor = Console.ReadLine();

                if (valor == "sair")
                {
                    break;
                }

                Console.WriteLine(LanguageService.Processar(valor));
            }
        }
    }
}
