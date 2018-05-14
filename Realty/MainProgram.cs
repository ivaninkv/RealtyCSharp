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


            var site = new Avito("https://www.avito.ru/odintsovo/kvartiry/sdam/na_dlitelnyy_srok/1-komnatnye?pmax=999999&pmin=1&view=list");
            var dt = site.Parse();
            Console.WriteLine(dt.Rows.Count);
            dt.ToCSV("test.csv");
            Console.WriteLine("Finish.");

            Console.ReadKey();
            return 0;
        }
    }
}
