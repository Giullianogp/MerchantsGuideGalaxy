using MerchantsGuideGalaxy.Services;
using System;

namespace MerchantsGuideGalaxy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Digite os valores a serem convertidos. Digite \"sair\" para fechar.");
            Console.WriteLine();

            while (true)
            {
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
