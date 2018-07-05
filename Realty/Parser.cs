using System;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using System.Data;
using System.Threading;
using System.Text.RegularExpressions;

namespace Realty
{
    /// <summary>
    /// Базовый класс для работы с сайтами недвижимости
    /// </summary>
    public class Parser
    {
        #region Поля
        /// <summary>
        /// Корневой URL сайта
        /// </summary>
        public readonly string siteUrl;
        private DataTable dt = new DataTable();
        #endregion

        #region Конструкторы
        /// <summary>
        /// Конструктор, в качестве параметра принимает поисковую строку
        /// </summary>
        /// <param name="searchUrl">Поисковый запрос, который нужно распарсить.</param>
        public Parser(string searchUrl)
        {
            if (string.IsNullOrWhiteSpace(searchUrl))
            {
                throw new ArgumentException(
                    string.Format($"Аргумент {nameof(searchUrl)} не может быть пустым."));
            }
            SearchUrl = searchUrl;
            siteUrl = new Uri(SearchUrl).Host;            

            dt.Columns.Add("Floor", typeof(String));
            dt.Columns.Add("Link", typeof(String));
            dt.Columns.Add("Area", typeof(String));
            dt.Columns.Add("Address", typeof(String));
            dt.Columns.Add("Price", typeof(String));
        }

        /// <summary>
        /// Конструктор, в качестве параметра принимает поисковую строку и флаг использования прокси
        /// </summary>
        /// <param name="searchUrl">Поисковый запрос, который нужно распарсить.</param>
        /// <param name="useProxy">Использовать ли прокси.</param>
        public Parser(string searchUrl, bool useProxy) : this(searchUrl)
        {
            UseProxy = useProxy;
        }
        #endregion

        #region Свойства
        /// <summary>
        /// Поисковая строка
        /// </summary>
        public string SearchUrl { get; set; }
        /// <summary>
        /// Использовать прокси
        /// </summary>
        public bool UseProxy { get; set; }
        /// <summary>
        /// Задержка между запросами к сайту
        /// </summary>
        public int Delay_s { get; set; }
        /// <summary>
        /// Этаж
        /// </summary>
        public string Floor { get; set; }
        /// <summary>
        /// Ссылка на объявление
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// Площадь квартиры
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Цена квартиры
        /// </summary>
        public string Price { get; set; }
        #endregion

        #region Методы
        /// <summary>
        /// Парсит результаты запроса
        /// </summary>
        /// <returns>Дататейбл с результатами запроса</returns>
        public virtual DataTable Parse()
        {
            int pagenum = 1;
            dt.Clear();

            while (true)
            {
                Console.WriteLine($"Page - {pagenum}");

                CustomRequest customRequest = new CustomRequest(SearchUrl + $"&p={pagenum}", "", "GET", "", "") { UseProxy = UseProxy };
                string htmlPage = customRequest.SendRequest();
                if (SearchUrl + $"&p={pagenum}" == customRequest.ResponseUrl) { FillDT(htmlPage); }
                else { break; }
                if (Delay_s > 0) { Thread.Sleep(Delay_s * 1000); }
                pagenum += 1;                
            }

            return dt;
        }

        private void FillDT(string htmlPage)
        {
            HtmlParser parser = new HtmlParser();
            var page = parser.Parse(htmlPage);

            var floor_sel = page.QuerySelectorAll(Floor);
            var link_sel = page.QuerySelectorAll(Link);
            var area_sel = page.QuerySelectorAll(Area);
            var address_sel = page.QuerySelectorAll(Address);
            var price_sel = page.QuerySelectorAll(Price);

            for (int i = 0; i < floor_sel.Length; i++)
            {
                dt.Rows.Add(new String[] { floor_sel[i].TextContent.Trim().RemoveIncorrectChars(),
                                            siteUrl + Regex.Match(link_sel[i].OuterHtml, @"<a.*?href=(""|')(?<href>.*?)(""|').*?>(?<value>.*?)</a>",RegexOptions.IgnoreCase).Groups["href"].Value,
                                            area_sel[i].TextContent.Trim().RemoveIncorrectChars(),
                                            address_sel[i].TextContent.Trim().RemoveIncorrectChars(),
                                            price_sel[i].TextContent.Trim().RemoveIncorrectChars() });
            }
        }
        #endregion
    }
}
