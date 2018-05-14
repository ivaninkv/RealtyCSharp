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
            AddrSuggestion addrSuggestion = new AddrSuggestion(Const.DadataToken, Const.DadataAddrUrl);
            addrSuggestion.GetAddrSuggestion("г. Ступино, тургенева", 1);
            Console.WriteLine(addrSuggestion.City);
            Console.WriteLine(addrSuggestion.Street);



            var c = new Cian("https://www.cian.ru/cat.php?currency=2&deal_type=sale&engine_version=2&maxprice=2000000&minprice=1&offer_type=flat&region=4593&room1=1");
            c.Parse();


            Console.ReadKey();

            return 0;
        }
    }
}
