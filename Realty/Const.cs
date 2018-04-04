using System;
using System.Collections;

namespace Realty
{
    /// <summary>
    /// Статический класс для констант
    /// </summary>
    internal static class Const
    {
        /// <summary>
        /// константы для использования в приложении
        /// </summary>
        internal const string DadataToken = "dba3afebfa4352ef55d908aed30f87780a46949e";
        internal const string DadataAddrUrl = "https://suggestions.dadata.ru/suggestions/api/4_1/rs/suggest/address";
        internal const string DadataAddrQuery = "<req><query>{0}</query><count>{1}</count></req>";


        /// <summary>
        /// Получение рандомного useragent'а
        /// </summary>
        internal static string GetUserAgent()
        {
            ArrayList arrayList = new ArrayList();
            arrayList.Add("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36");
            arrayList.Add("Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/40.0.2214.93 Safari/537.36");
            arrayList.Add("Mozilla/5.0 (Windows; U; Windows NT 5.2; ru-RU) AppleWebKit/534.4 (KHTML, like Gecko) Chrome/6.0.481.0 Safari/534.4");
            arrayList.Add("Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)");
            arrayList.Add("Mozilla/5.0 (compatible, MSIE 11, Windows NT 6.3; Trident/7.0;  rv:11.0) like Gecko");
            arrayList.Add("Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; AS; rv:11.0) like Gecko");

            Random rnd = new Random();            
            return arrayList[rnd.Next(0, arrayList.Count - 1)].ToString();
        }
    }
}
