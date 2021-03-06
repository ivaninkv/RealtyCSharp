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
        private string method;
        private string responseUrl;        
        private Dictionary<string, string> header = new Dictionary<string, string>();
        #endregion

        #region Конструкторы
        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="url">Url запроса.</param>
        /// <param name="method">Метод запрса. Возможные значения [POST, GET, PUT, PATCH, DELETE].</param>
        public CustomRequest(string url, string method = "GET")
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException(
                    string.Format($"Аргумент {nameof(url)} не может быть пустым."));
            }
            if (string.IsNullOrWhiteSpace(method))
            {
                throw new ArgumentException(
                    string.Format($"Аргумент {nameof(method)} не может быть пустым."));
            }
            Url = url;
            Method = method;
            UserAgent = Const.GetUserAgent();
        }

        /// <summary>
        /// Расширенный конструктор
        /// </summary>
        /// <param name="url">Url запроса.</param>
        /// <param name="request">Текст запроса.</param>
        /// <param name="method">Метод запрса. Возможные значения [POST, GET, PUT, PATCH, DELETE].</param>
        /// <param name="contentType">Используемый ContentType (формат ответа).</param>
        /// <param name="accept">Используемый Accept (формат запроса).</param>
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
        public string Url { get; set; }
        /// <summary>
        /// Текст запроса
        /// </summary>
        public string Request { get; set; }
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
        public string ContentType { get; set; }
        /// <summary>
        /// Используемый Accept (формат запроса)
        /// </summary>
        public string Accept { get; set; }
        /// <summary>
        /// Словарь заголовоков запроса (headers)
        /// </summary>
        public Dictionary<string, string> Header { get => header; }
        /// <summary>
        /// Используемый UserAgent
        /// </summary>
        public string UserAgent { get; set; }
        /// <summary>
        /// Использовать ли прокси-сервер
        /// </summary>
        public bool UseProxy { get; set; }
        /// <summary>
        /// Url ответа
        /// </summary>
        public string ResponseUrl { get => responseUrl; }
        #endregion

        #region Методы
        /// <summary>
        /// Добавляет header к запросу
        /// </summary>
        /// <param name="Key">Ключ</param>
        /// <param name="Value">Значение</param>
        public void AddHeader(string Key, string Value)
        {
            header.Add(Key, Value);
        }

        /// <summary>
        /// Отправка запроса
        /// </summary>
        /// <returns>Возвращает результат выполнения запроса.</returns>
        public string SendRequest()
        {
            HttpWebRequest req = WebRequest.CreateHttp(Url);
            req.Method = Method;
            req.ContentType = ContentType;
            req.Accept = Accept;

            if (UseProxy)
            {
                Proxy myProxy = new Proxy();
                WebProxy webProxy = new WebProxy(myProxy.ProxyString, myProxy.ProxyPort);
                req.Proxy = webProxy;
            }
                        
            foreach (KeyValuePair<string, string> kvp in header)
            {
                req.Headers.Add(kvp.Key, kvp.Value);
            }            

            if (Request != "")
            {
                StreamWriter sw = new StreamWriter(req.GetRequestStream());
                sw.Write(Request);
                sw.Close();
            }

            try
            {
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                responseUrl = res.ResponseUri.AbsoluteUri;
                if (res.StatusCode.ToString() == "OK")
                {
                    StreamReader sr = new StreamReader(res.GetResponseStream());
                    string answer = sr.ReadToEnd().Trim();
                    res.Close();
                    return answer;
                }
                else
                {
                    res.Close();
                    return "";
                }
            }
            catch (WebException e)
            {
                responseUrl = e.Message;
                return "";
            }
        }
        #endregion
    }
}
