using System.Data;

namespace Realty
{
    /// <summary>
    /// Класс для работы с сайтом cian.ru
    /// </summary>
    public class Cian : Parser
    {
        #region Конструкторы
        /// <summary>
        /// Конструктор, в качестве параметра принимает поисковую строку
        /// </summary>
        /// <param name="searchUrl">Поисковый запрос, который нужно распарсить.</param>
        public Cian(string searchUrl) : base(searchUrl)
        {
            Delay_s = 3;
            Floor = ".header--1ZTfS";
            Link = ".cardLink--3KbME";
            Area = ".header--1WFWC";
            Address = ".address-path--12tl2";
            Price = ".header--2lxlC";
        }
        #endregion

        #region Методы
        /// <summary>
        /// Парсит результаты запроса
        /// </summary>
        /// <returns>Дататейбл с результатами запроса</returns>
        public override DataTable Parse()
        {
            var dt = new DataTable();
            dt = base.Parse();

            return dt;
        }
        #endregion

    }
}
