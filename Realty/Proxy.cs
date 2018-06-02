using System;
using System.IO;

namespace Realty
{
    /// <summary>
    /// Класс для работы с прокси-серверами
    /// </summary>
    public class Proxy
    {
        #region Конструкторы
        /// <summary>
        /// Читает список прокси из каталога с программой
        /// </summary>
        public Proxy()
        {
            string[] proxyList = File.ReadAllLines(@"proxy.txt");
            Random rnd = new Random();
            string[] proxy = proxyList[rnd.Next(0, proxyList.GetLength(0) - 1)].ToString().Split(':');
            ProxyString = proxy[0].ToString();
            ProxyPort = Convert.ToInt32(proxy[1].ToString());
        }
        #endregion

        #region Свойства
        /// <summary>
        /// Хост прокси-сервера
        /// </summary>
        public string ProxyString { get; set; }
        /// <summary>
        /// Порт прокси-сервера
        /// </summary>
        public int ProxyPort { get; set; }
        #endregion

    }
}
