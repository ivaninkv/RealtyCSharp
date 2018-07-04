using System;
using System.Data;
using System.Linq;

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
            Console.ReadKey();
            return 0;
            //*/

            Console.WriteLine("Выберите режим работы:");
            Console.WriteLine("0 - выход.");
            Console.WriteLine("1 - простой парсинг.");
            Console.WriteLine("2 - анализ цен.");

            int choise = 0;

            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int input))
                    choise = input;
                if (new int[] { 0, 1, 2 }.Contains(choise))
                    break;
                else
                    Console.WriteLine("Некорректный ввод.");
            }

            switch (choise)
            {
                case 1:
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
                    break;

                case 2:
                    //var result = from row in dt.AsEnumerable()
                    //             group row by row.Field<int>("Floor_All") into grp
                    //             select new
                    //             {
                    //                 Floor_All = grp.Key,
                    //                 MemberCount = grp.Count()
                    //             };
                    //foreach (var t in result)
                    //    Console.WriteLine(t.Floor_All + " " + t.MemberCount);
                    break;

                default:
                    return 0;

            }
            Console.ReadKey();
            return 0;
        }
    }
}
