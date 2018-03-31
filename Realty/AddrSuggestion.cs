using System;
using System.Xml;

namespace Realty
{
    /// <summary>
    /// Класс для работы с сервисом dadata.ru
    /// </summary>
    public class AddrSuggestion
    {
        #region Поля
        private string token;
        private string url;
        private string city;
        private string street;
        #endregion

        #region Конструкторы
        /// <summary>
        /// Конструктор с указанием токена для подключения к dadata
        /// </summary>
        /// <param name="token">Токен для подключения к dadata</param>
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
        /// <param name="token">Токен для подключения к dadata</param>
        /// <param name="url">Url сервиса dadata</param>
        public AddrSuggestion(string token, string url) : this(token)
        {
            Url = url;
        }
        #endregion

        #region Свойства
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
        #endregion

        #region Методы
        /// <summary>
        /// Получить подсказку по адресу, образец адреса и количество посдсказок
        /// </summary>
        /// <param name="Addr">Адрес для которого нужно искать подсказки</param>
        /// <param name="qty">Количество подсказок</param>
        public void GetAddrSuggestion(string Addr, int qty = 1)
        {
            string q = String.Format(Const.DadataAddrQuery, Addr, qty);
            CustomRequest customRequest = new CustomRequest(Url, q, "POST");
            customRequest.AddHeader("Authorization", "Token " + Token);
            string xmlString = customRequest.SendRequest();
            if (xmlString != "")
            {
                XmlDocument xmlDoc = new XmlDocument();
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
        
        
        #endregion

    }
}
