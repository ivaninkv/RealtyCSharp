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
        private string searchUrl;        
        private int delay;
        private string floor;
        private string link;
        private string area;
        private string address;
        private string price;
        private DataTable dt = new DataTable();
        #endregion

        #region Конструкторы
        /// <summary>
        /// Конструктор, в качестве параметра принимает поисковую строку
        /// </summary>
        /// <param name="searchurl">Поисковый запрос, который нужно распарсить.</param>
        public Parser(string searchurl)
        {
            if (string.IsNullOrWhiteSpace(searchurl))
            {
                throw new ArgumentException(
                    string.Format($"Аргумент {nameof(searchurl)} не может быть пустым."));
            }
            SearchUrl = searchurl;
            siteUrl = new Uri(SearchUrl).Host;

            dt.Columns.Add("Floor", typeof(String));
            dt.Columns.Add("Link", typeof(String));
            dt.Columns.Add("Area", typeof(String));
            dt.Columns.Add("Address", typeof(String));
            dt.Columns.Add("Price", typeof(String));
        }
        #endregion

        #region Свойства
        /// <summary>
        /// Поисковая строка
        /// </summary>
        public string SearchUrl { get => searchUrl; set => searchUrl = value; }
        /// <summary>
        /// Задержка между запросами к сайту
        /// </summary>
        public int Delay { get => delay; set => delay = value; }
        /// <summary>
        /// Этаж
        /// </summary>
        public string Floor { get => floor; set => floor = value; }
        /// <summary>
        /// Ссылка на объявление
        /// </summary>
        public string Link { get => link; set => link = value; }
        /// <summary>
        /// Площадь квартиры
        /// </summary>
        public string Area { get => area; set => area = value; }
        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get => address; set => address = value; }
        /// <summary>
        /// Цена квартиры
        /// </summary>
        public string Price { get => price; set => price = value; }
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

                CustomRequest customRequest = new CustomRequest(searchUrl + $"&p={pagenum}", "", "GET", "", "") { UseProxy = false };
                string htmlPage = customRequest.SendRequest();
                if (SearchUrl + $"&p={pagenum}" == customRequest.ResponseUrl) { FillDT(htmlPage); }
                else { break; }
                if (delay > 0) { Thread.Sleep(delay * 1000); }
                pagenum += 1;

                //break; // test
            }

            return dt;
        }

        private void FillDT(string htmlPage)
        {
            HtmlParser parser = new HtmlParser();
            var page = parser.Parse(htmlPage);
            
            var floor_sel = page.QuerySelectorAll(floor);
            var link_sel = page.QuerySelectorAll(link);
            var area_sel = page.QuerySelectorAll(area);
            var address_sel = page.QuerySelectorAll(address);
            var price_sel = page.QuerySelectorAll(price);

            for (int i = 0; i < floor_sel.Length; i++)
            {
                dt.Rows.Add(new String[] { floor_sel[i].TextContent.Trim(),
                                            siteUrl + Regex.Match(link_sel[i].OuterHtml, @"<a.*?href=(""|')(?<href>.*?)(""|').*?>(?<value>.*?)</a>",RegexOptions.IgnoreCase).Groups["href"].Value,
                                            area_sel[i].TextContent.Trim(),
                                            address_sel[i].TextContent.Trim(),
                                            price_sel[i].TextContent.Trim() });
            }            
        }
        #endregion
    }
}
