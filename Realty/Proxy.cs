﻿using System;
using System.IO;

namespace Realty
{
    /// <summary>
    /// Класс для работы с прокси-серверами
    /// </summary>
    public class Proxy
    {
        private string proxyString;

        /// <summary>
        /// Читает список прокси из каталога с программой
        /// </summary>
        public Proxy()
        {
            string[] proxyList = File.ReadAllLines(@"proxy.txt");            
            Random rnd = new Random();
            ProxyString = "http://" + proxyList[rnd.Next(0, proxyList.GetLength(0) - 1)].ToString();            
        }

        /// <summary>
        /// Строка прокси-сервера
        /// </summary>
        public string ProxyString { get => proxyString; set => proxyString = value; }     
    }
}
