using System;
using System.Net;
using System.IO;
using System.Xml;

namespace Realty
{
    /// <summary>
    /// Класс для работы с сервисом dadata.ru
    /// </summary>
    public class AddrSuggestion
    {
        private string token;
        private string url;
        private string city;
        private string street;

        /// <summary>
        /// Конструктор с указанием токена для подключения к dadata
        /// </summary>
        /// <param name="token"></param>
        public AddrSuggestion(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException(
                    string.Format("Аргумент {0} не может быть пустым.", nameof(token)));
            }
            Token = token;
        }

        /// <summary>
        /// Конструктор с указанием токена и url для подключения к dadata
        /// </summary>
        /// <param name="token"></param>
        /// <param name="url"></param>
        public AddrSuggestion(string token, string url) : this(token)
        {
            Url = url;
        }

        /// <summary>
        /// Токен для подключения к dadata
        /// </summary>
        public string Token { get => token; set => token = value; }
        /// <summary>
        /// Url подсказок по адресу
        /// </summary>
        public string Url { get => url; set => url = value; }
        /// <summary>
        /// Распарсенный город, только для чтения
        /// </summary>
        public string City { get => city; }
        /// <summary>
        /// Распарсенная улица, только для чтения
        /// </summary>
        public string Street { get => street; }

        /// <summary>
        /// Получить подсказку по адресу, образец адреса и количество посдсказок
        /// </summary>
        /// <param name="Addr"></param>
        /// <param name="qty"></param>
        public void GetAddrSuggestion(string Addr, int qty = 1)
        {
            HttpWebRequest request = WebRequest.CreateHttp(Url);
            request.Method = "POST";
            request.ContentType = "application/xml";
            request.Accept = "application/xml";            
            request.Headers.Add("Authorization", "Token " + Token);
            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            string q = String.Format(Const.q, Addr, qty);
            sw.Write(q);
            sw.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusDescription == "OK")
            {
                XmlDocument xmlDoc = new XmlDocument();
                StreamReader sr = new StreamReader(response.GetResponseStream());
                string xmlString = sr.ReadToEnd().Trim();
                if (xmlString != "")
                {                    
                    xmlDoc.LoadXml(xmlString);
                    XmlNode node;
                    node = xmlDoc.SelectSingleNode(".//suggestions/data/city_with_type");
                    if (node != null)
                    {
                        if (node.InnerText != "") { city = node.InnerText; }
                        else
                        {
                            node = xmlDoc.SelectSingleNode(".//suggestions/data/settlement_with_type");
                            if (node != null) { city = node.InnerText; }
                        }
                    }
                    node = xmlDoc.SelectSingleNode(".//suggestions/data/street");
                    if (node != null) { street = node.InnerText; }
                }
            }
        }
    }
}
