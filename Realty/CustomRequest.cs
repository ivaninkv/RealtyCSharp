﻿using System.Net;
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
        #region Поля
        private string url;
        private string request;
        private string method;
        private string contentType;
        private string accept;
        private Dictionary<string, string> header = new Dictionary<string, string>();
        private string userAgent;
        #endregion

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
            UserAgent = Const.GetUserAgent();
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
            string accept = "application/xml") : this(url, method)
        {
            Request = request;            
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
                if (!(value == "POST" || value == "GET" || value == "PUT" || value == "PATCH" || value == "DELETE"))
                { throw new ArgumentException("Метод должен быть один из [POST, GET, PUT, PATCH, DELETE]"); };
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
        public Dictionary<string, string> Header { get => header; }
        /// <summary>
        /// Используемый UserAgent
        /// </summary>
        public string UserAgent { get => userAgent; set => userAgent = value; }
        #endregion

        #region Методы
        /// <summary>
        /// Добавляет header к запросу
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void AddHeader(string Key, string Value)
        {
            header.Add(Key, Value);
        }
        
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

            if (Request != "")
            {
                StreamWriter sw = new StreamWriter(req.GetRequestStream());
                sw.Write(Request);
                sw.Close();
            }            
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            HttpStatusCode httpStatus = res.StatusCode;            
            if (httpStatus.ToString() == "OK")
            {
                StreamReader sr = new StreamReader(res.GetResponseStream());
                return (sr.ReadToEnd().Trim());
            }
            return ("");
        }
        #endregion

    }
}
