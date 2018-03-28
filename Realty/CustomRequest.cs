using System.Net;
using System.IO;
using System;

namespace Realty
{
    /// <summary>
    /// Класс для работы с запросами
    /// </summary>
    public class CustomRequest
    {
        private string url;
        private string request;
        private string method;
        private string ContentType;

        public string Url { get => url; set => url = value; }
    }
}
