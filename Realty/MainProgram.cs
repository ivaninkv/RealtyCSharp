using System;

namespace Realty
{
    /// <summary>
    /// Программа парсинга сайтов
    /// </summary>
    class MainProgram
    {
        static int Main(string[] args)
        {
            /*
            AddrSuggestion addrSuggestion = new AddrSuggestion(Const.DadataToken, Const.DadataAddrUrl);
            addrSuggestion.GetAddrSuggestion("г. Ступино, тургенева", 1);
            Console.WriteLine(addrSuggestion.City);
            Console.WriteLine(addrSuggestion.Street);
            */

            Console.WriteLine("Введите url для парсинга:");
            string url = Console.ReadLine();
            string host = new Uri(url).Host;
            if (host == "www.avito.ru")
            {
                var site = new Avito(url);
                var dt = site.Parse();
                Console.WriteLine($"Распарсено {dt.Rows.Count} объявлений.");
                dt.ToCSV("avito.csv");
            }
            else
                Console.WriteLine($"Не найден обработчик для сайта {host}.");

            Console.ReadKey();
            return 0;
        }
    }
}
