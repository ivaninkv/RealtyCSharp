using System;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;

namespace Realty
{
    /// <summary>
    /// Класс для работы с сайтом cian.ru
    /// </summary>
    public class Cian
    {
        #region Поля
        private string searchUrl;
        #endregion

        #region Конструкторы
        /// <summary>
        /// Конструктор, в качестве параметра принимает поисковую строку
        /// </summary>
        /// <param name="searchurl">Поисковый запрос, который нужно распарсить.</param>
        public Cian(string searchurl)
        {
            if (string.IsNullOrWhiteSpace(searchurl))
            {
                throw new ArgumentException(
                    string.Format("Аргумент {0} не может быть пустым.", nameof(searchurl)));
            }
            SearchUrl = searchurl;
        }
        #endregion

        #region Свойства
        /// <summary>
        /// Поисковая строка
        /// </summary>
        public string SearchUrl { get => searchUrl; set => searchUrl = value; }
        #endregion

        #region Методы
        /// <summary>
        /// Парсит результаты запроса
        /// </summary>
        public void Parse()
        {
            CustomRequest customRequest = new CustomRequest(searchUrl, "", "GET", "", "")
            {
                UseProxy = false
            };
            string htmlPage = customRequest.SendRequest();
            if (SearchUrl == customRequest.ResponseUrl) { Console.WriteLine("Equal"); }
            else { Console.WriteLine("Not equal"); }

            HtmlParser parser = new HtmlParser();
            var page = parser.Parse(htmlPage);
            var CssSel = page.QuerySelectorAll(".address-path--12tl2");
            int i = 1;
            foreach (var item in CssSel)
            {                
                Console.WriteLine(String.Format("{0} - {1}", i, item.Text()));                
                i++;
            }            
            Console.WriteLine("Finish");            
        }
        #endregion
    }
}
