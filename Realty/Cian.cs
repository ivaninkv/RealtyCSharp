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
        private string searchUrl;

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

        /// <summary>
        /// Задает или получает поисковую строку
        /// </summary>
        public string SearchUrl { get => searchUrl; set => searchUrl = value; }

        /// <summary>
        /// Парсит результаты запроса
        /// </summary>
        public void Parse()
        {
            HtmlParser parser = new HtmlParser();
            IHtmlDocument document = parser.Parse(searchUrl);
            
            Console.WriteLine(document.DocumentElement.OuterHtml);
        }
    }
}
