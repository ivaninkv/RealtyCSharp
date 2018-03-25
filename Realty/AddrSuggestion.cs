using System;
using System.Net;
using System.IO;
using System.Xml;

namespace Realty
{
    internal class AddrSuggestion
    {
        private string token;
        private string url;
        private string city;
        private string street;

        /// <summary>
        /// конструктор с указанием токена для подключения к dadata
        /// </summary>
        /// <param name="token"></param>
        internal AddrSuggestion(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException(
                    string.Format("Аргумент {0} не может быть пустым.", nameof(token)));
            }
            Token = token;
        }

        /// <summary>
        /// конструктор с указанием токена и url для подключения к dadata
        /// </summary>
        /// <param name="token"></param>
        /// <param name="url"></param>
        internal AddrSuggestion(string token, string url) : this(token)
        {
            Url = url;
        }

        internal string Token { get => token; set => token = value; }
        internal string Url { get => url; set => url = value; }
        internal string City { get => city; }
        internal string Street { get => street; }

        internal void GetAddrSuggestion(string Addr)
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(Url);
            request.Method = "POST";
            request.ContentType = "application/xml";
            request.Accept = "application/xml";            
            request.Headers.Add("Authorization", "Token " + Token);
            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            string q = String.Format(Const.q, Addr, 1);
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
                    node = xmlDoc.SelectSingleNode(".//suggestions/data/city");
                    city = node.InnerText;
                    node = xmlDoc.SelectSingleNode(".//suggestions/data/street");
                    street = node.InnerText;
                }
            }
        }
    }
}
