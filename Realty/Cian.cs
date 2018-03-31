using System;
using AngleSharp.Dom.Html;
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
        /// <param name="searchurl"></param>
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
            CustomRequest customRequest = new CustomRequest(searchUrl, "", "GET", "", "");
            HtmlParser parser = new HtmlParser();
            IHtmlDocument document = parser.Parse(customRequest.SendRequest());
            
            Console.WriteLine(document.DocumentElement.OuterHtml);
        }
        #endregion
    }
}
