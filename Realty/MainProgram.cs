using System;

namespace Realty
{
    class MainProgram
    {
        static int Main(string[] args)
        {
            AddrSuggestion addrSuggestion = new AddrSuggestion(Const.Token, Const.Url);
            addrSuggestion.GetAddrSuggestion("г. Ступино, тургенева", 1);
            Console.WriteLine(addrSuggestion.City);
            Console.WriteLine(addrSuggestion.Street);

            
            var c = new Cian("https://github.com/AngleSharp/AngleSharp/wiki/Documentation");
            c.Parse();
            
            Console.ReadKey();          
            
            return 0;
        }
    }
}
