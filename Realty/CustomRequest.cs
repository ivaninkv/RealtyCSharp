using System.Net;
using System.IO;
using System;
using System.Collections.Generic;

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
        private string contentType;
        private string accept;
        private Dictionary<string, string> header;

        #region Конструкторы
        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        public CustomRequest(string url, string method = "GET")
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException(
                    string.Format("Аргумент {0} не может быть пустым.", nameof(url)));
            }
            if (string.IsNullOrWhiteSpace(method))
            {
                throw new ArgumentException(
                    string.Format("Аргумент {0} не может быть пустым.", nameof(method)));
            }
            Url = url;
            Method = method;
        }

        /// <summary>
        /// Расширенноый конструктор
        /// </summary>
        /// <param name="url"></param>
        /// <param name="request"></param>
        /// <param name="method"></param>
        /// <param name="contentType"></param>
        /// <param name="accept"></param>
        public CustomRequest(string url, 
            string request, 
            string method, 
            string contentType = "application/xml", 
            string accept = "application/xml") : this(url, request)
        {
            Method = method;
            ContentType = contentType;
            Accept = accept;
        }
        #endregion

        #region Свойства
        /// <summary>
        /// Url по которму отправить запрос
        /// </summary>
        public string Url { get => url; set => url = value; }
        /// <summary>
        /// Текст запроса
        /// </summary>
        public string Request { get => request; set => request = value; }
        /// <summary>
        /// Используемый метод
        /// </summary>
        public string Method
        {
            get => method;
            set
            {
                if (value != "POST" || value != "GET" || value != "PUT" || value != "PATCH" || value != "DELETE")
                { throw new ArgumentException(string.Format("Аргумент {0} не может быть пустым.", nameof(value))); };
                method = value;
            }
        }
        /// <summary>
        /// Используемый ContentType (формат ответа)
        /// </summary>
        public string ContentType { get => contentType; set => contentType = value; }
        /// <summary>
        /// Используемый Accept (формат запроса)
        /// </summary>
        public string Accept { get => accept; set => accept = value; }
        /// <summary>
        /// Словарь заголовоков запроса (headers)
        /// </summary>
        public Dictionary<string, string> Header { get => header; set => header = value; }
        #endregion

        #region Методы
        /// <summary>
        /// Отправка запроса
        /// </summary>
        /// <returns></returns>
        public string SendRequest()
        {
            HttpWebRequest req = WebRequest.CreateHttp(Url);
            req.Method = Method;
            req.ContentType = ContentType;
            req.Accept = Accept;            
            foreach (KeyValuePair<string, string> kvp in Header)
            {
                req.Headers.Add(kvp.Key, kvp.Value);                
            }

            StreamWriter sw = new StreamWriter(req.GetRequestStream());
            sw.Write(Request);
            sw.Close();
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            HttpStatusCode httpStatus = res.StatusCode;
            Console.WriteLine(String.Format("HttpStatusCode = {0}", httpStatus.ToString()));
            if (res.StatusDescription == "OK")
            {
                StreamReader sr = new StreamReader(res.GetResponseStream());
                return (sr.ReadToEnd().Trim());
            }
            return ("");

            #endregion
        }
}
