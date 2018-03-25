using System;

namespace Realty
{
    class MainProgram
    {
        static int Main(string[] args)
        {
            AddrSuggestion addrSuggestion = new AddrSuggestion(Const.Token, Const.Url);
            addrSuggestion.GetAddrSuggestion("Московская область, Ленинский район, д. Калиновка, 57А");
            Console.WriteLine(addrSuggestion.City);
            Console.WriteLine(addrSuggestion.Street);
            Console.ReadKey();          

            return 0;
        }
    }
}
