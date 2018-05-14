using System;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using System.Data;

namespace Realty
{
    /// <summary>
    /// Базовый класс для работы с сайтами недвижимости
    /// </summary>
    class Parser
    {
        #region Поля
        private string searchUrl;
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
        }
        #endregion

        #region Свойства
        /// <summary>
        /// Поисковая строка
        /// </summary>
        public string SearchUrl { get => searchUrl; set => searchUrl = value; }
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
        public DataTable Parse()
        {                        
            int pagenum = 1;
            dt.Clear();

            while (true)
            {
                CustomRequest customRequest = new CustomRequest(searchUrl + $"&p={pagenum}", "", "GET", "", "") { UseProxy = false };
                string htmlPage = customRequest.SendRequest();
                if (SearchUrl + $"&p={pagenum}" == customRequest.ResponseUrl) { FillDT(htmlPage); }
                else { break; }
            }

            return dt;
        }

        private void FillDT(string htmlPage)
        {
            HtmlParser parser = new HtmlParser();
            var page = parser.Parse(htmlPage);
            var CssSel = page.QuerySelectorAll(".address-links--3Qx74");
            
            foreach (var item in CssSel)
            {
                Console.WriteLine(item.Text());
                
            }
            Console.WriteLine("Finish");
        }
        #endregion
    }
}
